# INIConfigIO

DDS.Net.Server.PublicHelpers.**INIConfigIO** provides an easy to use interface for reading and writing INI files. Its constructor simply takes the INI file name and an optional implementation of [ILogger](./ILogger.md) interface:

```csharp
INIConfigIO(string filename, ILogger? logger = null)
```

It can be simply instantiated, like:

```csharp
INIConfigIO iniFile = new INIConfigIO("file.ini");
```


Instance functions help in reading and writing the INI file:

&nbsp;

  * *Clear*() - Clears the in-memory data but does not effect the file till it is saved.
  * *SaveFile*() - Saves the in-memory data into the given file.


  * IEnumerable&lt;string&gt; *GetSectionNames*() - Gets all the section names present in the file.
  
  * string *GetString* (string *key*, string *defaultValue* = "") - Gets a value as a string, default value is returned when the key is not present in the file.
  * int *GetInteger* (string *key*, int *defaultValue* = -1) - Gets a value as an integer, default value is returned when the key is not present in the file.
  * float *GetFloat* (string *key*, float *defaultValue* = 0) - Gets a value as a float, default value is returned when the key is not present in the file.
  * double *GetDouble* (string *key*, double *defaultValue* = 0) - Gets a value as a double, default value is returned when the key is not present in the file.


  * *SetString* (string *key*, string *value*) - Sets specified key's value to the given value
  * *SetInteger* (string *key*, int *value*) - Sets specified key's value to the given value
  * *SetFloat* (string *key*, float *value*) - Sets specified key's value to the given value
  * *SetDouble* (string *key*, double *value*) - Sets specified key's value to the given value

&nbsp;

For example, consider the following INI file:

```ini
[A]
a = 5
b = 6

[B]
a = 10
b = 20
```


Its values can be read as:

```csharp
INIConfigIO iniFile = new INIConfigIO("file.ini");

string value1 = iniFile.GetString("A/a"); // "5"
string value2 = iniFile.GetString("A/b"); // "6"

int value3 = iniFile.GetInteger("B/a"); // 10
int value4 = iniFile.GetInteger("B/b"); // 20
```



