/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  ExceptionSerializer.cs                                  |
|                                                          |
|  ExceptionSerializer class for C#.                       |
|                                                          |
|  LastModified: Feb 8, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;

namespace Hprose.IO.Serializers {
    using static Tags;

    internal class ExceptionSerializer<T> : ReferenceSerializer<T> where T : Exception {
        public override void Write(Writer writer, T obj) {
            // No reference to exception
            writer.AddReferenceCount(1);
            var stream = writer.Stream;
            stream.WriteByte(TagError);
            stream.WriteByte(TagString);
            ValueWriter.Write(stream, obj.Message);
        }
    }
}