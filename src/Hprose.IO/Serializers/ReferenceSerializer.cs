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
 * ReferenceSerializer.cs                                 *
 *                                                        *
 * hprose ReferenceSerializer class for C#.               *
 *                                                        *
 * LastModified: Apr 7, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

namespace Hprose.IO.Serializers {
    public abstract class ReferenceSerializer<T> : Serializer<T> {

        // write your actual serialization code in sub class
        public override void Write(Writer writer, T obj) => writer.SetRef(obj);

        public override void Serialize(Writer writer, T obj) {
            if (obj != null) {
                if (!writer.WriteRef(obj)) {
                    Write(writer, obj);
                }
            }
            else {
                writer.Stream.WriteByte(Tags.TagNull);
            }
        }

    }
}