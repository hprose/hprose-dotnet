/**********************************************************\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: http://www.hprose.com/                 |
|                   http://www.hprose.org/                 |
|                                                          |
\**********************************************************/
/**********************************************************\
 *                                                        *
 * TypeInfo.cs                                            *
 *                                                        *
 * TypeInfo class for C#.                                 *
 *                                                        *
 * LastModified: Dec 14, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
using System;

namespace Hprose.IO.Deserializers {
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
