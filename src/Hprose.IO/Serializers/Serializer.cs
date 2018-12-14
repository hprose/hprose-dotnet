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
 * Serializer.cs                                          *
 *                                                        *
 * hprose Serializer class for C#.                        *
 *                                                        *
 * LastModified: Dec 14, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

namespace Hprose.IO.Serializers {
    internal interface ISerializer {
        void Write(Writer writer, object obj);
        void Serialize(Writer writer, object obj);
    }

    public abstract class Serializer<T> : ISerializer {
        static Serializer() => Serializers.Initialize();
        private static volatile Serializer<T> instance;
        public static Serializer<T> Instance {
            get {
                if (instance == null) {
                    instance = Serializers.GetInstance(typeof(T)) as Serializer<T>;
                }
                return instance;
            }
        }
        public abstract void Write(Writer writer, T obj);
        public virtual void Serialize(Writer writer, T obj) => Write(writer, obj);
        void ISerializer.Write(Writer writer, object obj) => Write(writer, (T)obj);
        void ISerializer.Serialize(Writer writer, object obj) => Serialize(writer, (T)obj);
    }

    public class Serializer : Serializer<object> {
        public override void Serialize(Writer writer, object obj) {
            if (obj == null) {
                writer.Stream.WriteByte(Tags.TagNull);
            }
            else {
                Serializers.GetInstance(obj.GetType()).Serialize(writer, obj);
            }
        }

        public override void Write(Writer writer, object obj) {
            if (obj == null) {
                writer.Stream.WriteByte(Tags.TagNull);
            }
            else {
                Serializers.GetInstance(obj.GetType()).Write(writer, obj);
            }
        }
    }
}
