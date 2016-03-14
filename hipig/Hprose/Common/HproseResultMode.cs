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
 * HproseResultMode.cs                                    *
 *                                                        *
 * hprose result mode enum for C#.                        *
 *                                                        *
 * LastModified: Feb 21, 2014                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
namespace Hprose.Common {
    public enum HproseResultMode {
        Normal, Serialized, Raw, RawWithEndTag
    }
}