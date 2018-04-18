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
 * LastModified: Apr 18, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

namespace Hprose.IO.Deserializers {
    public enum DictType {
        NullableKeyDictionary, Dictionary, ExpandoObject, Hashtable
    }
}
