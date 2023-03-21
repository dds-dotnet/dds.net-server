# ConfigurationProvider

DDS.Net.Server.PublicHelpers.**ConfigurationProvider** provides static methods for reading configuration from files and provide configuration objects.


### GetServerConfiguration

The static method takes following signature:

```csharp
// returns: (enabled, ServerConfiguration)
Tuple<bool, ServerConfiguration?>
    GetServerConfiguration(
        string filename, ILogger logger)
```

It reads an INI file with following format and returns a [ServerConfiguration](./ServerConfiguration.md) object when the configuration is enabled.

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



### GetVariablesConfiguration

The static method takes following signature:

```csharp
VariablesConfiguration
    GetVariablesConfiguration(
        string filename, ILogger logger)
```

It reads an INI file with following format and returns a [VariablesConfiguration](./VariablesConfiguration.md) object.

```ini
; Format:
;    [Variable Name]
;    Settings...
;    ...
;    
;    [Another Variable Name]
;    ...
;    ...
;
; VariableType =
;       Primitive => Very basic type of data
;       RawBytes  => A sequence of bytes (unsigned bytes)
;
; PrimitiveType = 
;       String        => A String of characters
;
;       Boolean       => Boolean that can only have True or False value
;
;       Byte          => 1-byte Signed Integer number
;       UnsignedByte  => 1-byte Unsigned Integer number
;
;       Word          => 2-byte Signed Integer number
;       UnsignedWord  => 2-byte Unsigned Integer number
;
;       DWord         => 4-byte Signed Integer number
;       UnsignedDWord => 4-byte Unsigned Integer number
;
;       QWord         => 8-byte Signed Integer number
;       UnsignedQWord => 8-byte Unsigned Integer number
;
;       Single        => Single precision 4-byte Floating-point number
;       Double        => Double precision 8-byte Floating-point number
;

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
VariableType = RawBytes
Data = 1, 2, 3, 4, 5, 6, 7, 8, 9
```






