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
 * LastModified: Jan 10, 2019                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
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
