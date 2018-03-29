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
 * SingleSerializer.cs                                    *
 *                                                        *
 * SingleSerializer class for C#.                         *
 *                                                        *
 * LastModified: Mar 29, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

namespace Hprose.IO.Serializers {
    class SingleSerializer : Serializer<float> {
        private static readonly SingleSerializer _instance = new SingleSerializer();
        public static SingleSerializer Instance => _instance;
        public override void Write(Writer writer, float obj) => ValueWriter.Write(writer.Stream, obj);
    }
}
