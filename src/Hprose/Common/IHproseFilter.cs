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
 * LastModified: Mar 17, 2014                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
using System.IO;

namespace Hprose.Common {
    public interface IHproseFilter {
        MemoryStream InputFilter(MemoryStream inStream, HproseContext context);
        MemoryStream OutputFilter(MemoryStream outStream, HproseContext context);
    }
}
