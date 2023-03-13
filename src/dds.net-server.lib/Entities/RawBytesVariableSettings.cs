﻿namespace DDS.Net.Server.Entities
{
    public class RawBytesVariableSettings : VariableSettings
    {
        public byte[] Bytes { get; private set; }

        public RawBytesVariableSettings(string name, byte[] data) : base(name)
        {
            Bytes = data;
        }
    }
}
