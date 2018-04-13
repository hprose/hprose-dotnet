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
 * DBNullDeserializer.cs                                  *
 *                                                        *
 * DBNullDeserializer class for C#.                       *
 *                                                        *
 * LastModified: Apr 13, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Deserializers {
    class DBNullDeserializer : Deserializer<DBNull> {
        public override DBNull Read(Reader reader, int tag) {
            var stream = reader.Stream;
            switch (tag) {
                case TagNull:
                case TagEmpty:
                    return DBNull.Value;
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
