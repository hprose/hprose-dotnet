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
 * CharSerializer.cs                                      *
 *                                                        *
 * CharSerializer class for C#.                           *
 *                                                        *
 * LastModified: Apr 7, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

namespace Hprose.IO.Serializers {
    class CharSerializer : Serializer<char> {
        public override void Write(Writer writer, char obj) => ValueWriter.Write(writer.Stream, obj);
    }
}