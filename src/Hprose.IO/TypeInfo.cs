/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  TypeInfo.cs                                             |
|                                                          |
|  TypeInfo class for C#.                                  |
|                                                          |
|  LastModified: Jan 10, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/
using System;

namespace Hprose.IO {
    public class TypeInfo {
        public readonly string name;
        public readonly string[] names;
        public readonly Type type;
        public readonly string key;
        public TypeInfo(string name, string[] names) {
            this.name = name;
            this.names = names;
            type = TypeManager.GetType(name);
            key = string.Join(" ", names);
        }
    }
}
