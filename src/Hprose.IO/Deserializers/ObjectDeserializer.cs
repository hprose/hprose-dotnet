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
 * ObjectDeserializer.cs                                  *
 *                                                        *
 * ObjectDeserializer class for C#.                       *
 *                                                        *
 * LastModified: Apr 18, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System.IO;
using System.Collections;

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Deserializers {
    class ObjectDeserializer<T> : Deserializer<T> where T : new() {
        public static T Read(Reader reader) {
            Stream stream = reader.Stream;
            return default;
        }
        public static T ReadMapAsObject(Reader reader) {
            Stream stream = reader.Stream;
            return default;
        }
        public override T Read(Reader reader, int tag) {
            var stream = reader.Stream;
            switch (tag) {
                case TagEmpty:
                    return new T();
                case TagObject:
                    return Read(reader);
                case TagMap:
                    return ReadMapAsObject(reader);
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}