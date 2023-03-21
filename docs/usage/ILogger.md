# ILogger

DDS.Net.Server.Interfaces.**ILogger** is the main logging interface used by the library. The interface is simple and includes only three methods:

  * void *Info* (string *message*) - Used to write information-level message to log.
  * void *Warning* (string *message*) - Used to write warning-level message to log.
  * void *Error* (string *message*) - Used to write error-level message to log.

&nbsp;

For simplicity, the library includes following default implementations for the interface:

  * DDS.Net.Server.Interfaces.DefaultLogger.**BlankLogger** - discards all the log messages.
  * DDS.Net.Server.Interfaces.DefaultLogger.**ConsoleLogger** - prints log messages on the standard console.
  * DDS.Net.Server.Interfaces.DefaultLogger.**FileLogger** - prints log messages into a given file.
  * DDS.Net.Server.Interfaces.DefaultLogger.**SplitLogger** - splits log messages between two or more *ILogger* implementations


