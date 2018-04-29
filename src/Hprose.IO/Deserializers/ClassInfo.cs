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
 * ClassInfo.cs                                           *
 *                                                        *
 * ClassInfo class for C#.                                *
 *                                                        *
 * LastModified: Apr 29, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
using System;

namespace Hprose.IO.Deserializers {
    public class ClassInfo {
        public readonly string name;
        public readonly string[] names;
        public readonly Type type;
        public readonly string key;
        public ClassInfo(string name, string[] names) {
            this.name = name;
            this.names = names;
            type = ClassManager.GetType(name);
            key = string.Join(" ", names);
        }
    }
}
