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
 * IHproseFilter.cs                                       *
 *                                                        *
 * hprose filter interface for C#.                        *
 *                                                        *
 * LastModified: Jan 16, 2016                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
using System;
using System.IO;

namespace Hprose.Common {
#if dotNETMF
    [CLSCompliantAttribute(false)]
#endif
    public interface IHproseFilter {
        MemoryStream InputFilter(MemoryStream inStream, HproseContext context);
        MemoryStream OutputFilter(MemoryStream outStream, HproseContext context);
    }
}
