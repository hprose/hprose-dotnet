/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  DictType.cs                                             |
|                                                          |
|  hprose DictType enum for C#.                            |
|                                                          |
|  LastModified: Jan 10, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO {
    public enum DictType {
        NullableKeyDictionary, Dictionary, ExpandoObject, Hashtable
    }
}
