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
 * LastModified: Apr 8, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
using System;

namespace Hprose.IO.Deserializers {
    public struct ClassInfo {
        public string Name;
        public Type Type;
        public string[] Members;
    }
}
