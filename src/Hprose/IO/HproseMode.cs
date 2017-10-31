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
 * HproseMode.cs                                          *
 *                                                        *
 * hprose mode enum for C#.                               *
 *                                                        *
 * LastModified: Jan 20, 2016                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
#if !dotNETMF
namespace Hprose.IO {
    public enum HproseMode {
        FieldMode, PropertyMode, MemberMode
    }
}
#endif