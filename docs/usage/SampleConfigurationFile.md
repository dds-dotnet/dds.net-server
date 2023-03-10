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
```

Additionally, Variables' configuration sample is as follows:

```ini
[Test Variable 1]
VariableType = Primitive
PrimitiveType = Single

[Test Variable 2]
VariableType = Primitive
PrimitiveType = Double

[Test Variable 3]
VariableType = Primitive
PrimitiveType = Boolean

[Test Variable 4]
VariableType = Primitive
PrimitiveType = String

[Test Variable 5]
VariableType = Compound
PrimitiveNames = Test Variable 1, Test Variable 3, Test Variable 2, Test Variable 4
```

The ***INIConfigIO*** (inside *DDS.Net.Server.PublicHelpers* namespace) refers to the values as:
* *DDS Connections/Enabled*
* *DDS Connections/TCP-MaxClients*
* etc.
