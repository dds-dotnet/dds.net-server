﻿using DDS.Net.Server.Core.Internal.InterfaceImplementations;
using DDS.Net.Server.Core.Internal.Interfaces;

namespace DDS.Net.Server.Core.Internal.Base
{
    internal abstract class SinglePipedThread<T_Input, T_Output, T_Commands, T_Responses>
        : PipedThreadBase<T_Commands, T_Responses>, IDisposable

        where T_Input : class
        where T_Output : class
        where T_Commands : struct
        where T_Responses : struct
    {
        public ISyncDataWriterQueueEnd<T_Input> Input { get; private set; }
        public ISyncDataReaderQueueEnd<T_Output> Output { get; private set; }

        protected readonly SyncQueue<T_Input> inputQueue;
        protected readonly SyncQueue<T_Output> outputQueue;


        protected SinglePipedThread(
                    int inputQueueSize,
                    int outputQueueSize,
                    int commandsQueueSize,
                    int responsesQueueSize,
                    bool startThread = true)

            : base(commandsQueueSize, responsesQueueSize, false)
        {
            inputQueue = new SyncQueue<T_Input>(inputQueueSize);
            outputQueue = new SyncQueue<T_Output>(outputQueueSize);

            Input = inputQueue;
            Output = outputQueue;

            if (startThread)
            {
                StartThread();
            }
        }

        protected override void StartThread(Action? threadFunction = null)
        {
            lock (this)
            {
                if (_thread == null)
                {
                    _isThreadRunning = true;

                    if (threadFunction != null)
                    {
                        _thread = new Thread(threadFunction.Invoke);
                    }
                    else
                    {
                        _thread = new Thread(() =>
                        {
                            DoInit();

                            while (_isThreadRunning)
                            {
                                if (_isThreadRunning) DoWork();
                                if (_isThreadRunning && commandsQueue.CanDequeue()) ProcessCommand(commandsQueue.Dequeue());
                                if (_isThreadRunning) DoWork();
                                if (_isThreadRunning && inputQueue.CanDequeue()) CheckInputs();
                                if (_isThreadRunning) DoWork();
                                if (_isThreadRunning && outputQueue.CanEnqueue()) GenerateOutputs();

                                Thread.Yield();
                            }

                            DoCleanup();
                        });
                    }

                    _thread.Start();
                }
            }
        }
    }
}
