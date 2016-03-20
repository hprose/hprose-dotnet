## Hprose Invocation Proxy Implementation Generator

Copyright (c) 2008-2016 http://hprose.com

**Simple**
```hipig ExampleAssambly.dll```
**Full**
```hipig -r ExampleAssambly.dll -i IExample,IExampleInterface -o ExampleAssambly.cs -n Hprose.InvocationProxy -t Proxy```

```
  -r:Assembly         Imports metadata from the specified assembly
  -i:InterfaceName1[..,InterfaceNameN]    Interfaces name
  -o:OutputFileName   Output file
  -n:NameSpace        generate class in namespace
  -t:ClassTail        generate class name append the tail
```
