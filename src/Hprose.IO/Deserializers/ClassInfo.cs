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
 * LastModified: Apr 27, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
using System;

namespace Hprose.IO.Deserializers {
    public class ClassInfo {
        public string name;
        public Type type;
        public string[] names;
    }
}
