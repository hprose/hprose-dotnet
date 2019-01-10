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
 * DictType.cs                                            *
 *                                                        *
 * hprose DictType enum for C#.                           *
 *                                                        *
 * LastModified: Jan 10, 2019                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

namespace Hprose.IO {
    public enum DictType {
        NullableKeyDictionary, Dictionary, ExpandoObject, Hashtable
    }
}
