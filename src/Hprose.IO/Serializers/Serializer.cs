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
 * LastModified: Mar 28, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

namespace Hprose.IO.Serializers {
    public abstract class Serializer {
        public abstract void Write(Writer writer, object obj);
    }
    public abstract class Serializer<T> : Serializer {
        public abstract void Write(Writer writer, T obj);
        public override void Write(Writer writer, object obj) => Write(writer, (T)obj);
    }
}
