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
 * LastModified: Apr 28, 2018                             *
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
        private readonly int hash;

        public ClassInfo(string name, string[] names) {
            this.name = name;
            this.names = names;
            type = ClassManager.GetType(name);
            key = string.Join(" ", names);
            hash = (type?.GetHashCode() ?? 0) * 31 + key.GetHashCode();
        }

        public static bool operator ==(ClassInfo i1, ClassInfo i2) {
            return (i1.type == i2.type) && (i1.key == i2.key);
        }

        public static bool operator !=(ClassInfo i1, ClassInfo i2) {
            return !(i1 == i2);
        }

        public override bool Equals(object obj) {
            return (obj is ClassInfo) &&  (this == (ClassInfo)obj);
        }

        public override int GetHashCode() {
            return hash;
        }
    }
}
