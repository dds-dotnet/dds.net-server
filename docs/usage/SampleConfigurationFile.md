# Sample Configuration File

By default we are using "*.ini*" file for storing configuration. Server's configuration is as follows:

```ini
[DDS Connections]
Enabled = Yes
ListeningIPv4 = All
TCP-Enabled = Yes
TCP-ListeningPort = 44556
TCP-MaxClients = 100
UDP-Enabled = Yes
UDP-ListeningPort = 44556
UDP-MaxClients = 100
```

***INIConfigIO*** (inside *DDS.Net.Server.PublicHelpers* namespace) refers to the values as:
* ***DDS Connections/Enabled***
* ***DDS Connections/TCP-MaxClients***
* etc.
