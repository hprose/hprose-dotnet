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
 * ResultMode.cs                                          *
 *                                                        *
 * hprose ResultMode enum for C#.                         *
 *                                                        *
 * LastModified: May 7, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

namespace Hprose.Common {
    public enum ResultMode {
        Normal, Serialized, Raw, RawWithEndTag
    }
}