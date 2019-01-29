/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  Log.cs                                                  |
|                                                          |
|  Log plugin for C#.                                      |
|                                                          |
|  LastModified: Jan 30, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Hprose.RPC.Plugins {
    public class Log {
        private static async Task<MemoryStream> ToMemoryStreamAsync(Stream stream) {
            MemoryStream memoryStream;
            if (stream is MemoryStream) {
                memoryStream = (MemoryStream)stream;
            }
            else {
                memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
            }
            memoryStream.Position = 0;
            return memoryStream;
        }
        private static string ToString(MemoryStream stream) {
            byte[] data = stream.ToArray();
            try {
                return Encoding.UTF8.GetString(data);
            }
            catch {
                return Encoding.ASCII.GetString(data);
            }
        }
        public static async Task<Stream> IOHandler(Stream request, Context context, NextIOHandler next) {
            var stream = await ToMemoryStreamAsync(request);
            Trace.TraceInformation(ToString(stream));
            try {
                var response = await next(stream, context);
                stream = await ToMemoryStreamAsync(response);
                Trace.TraceInformation(ToString(stream));
                return stream;
            }
            catch (Exception e) {
                Trace.TraceError(e.StackTrace);
                throw e;
            }
        }
        public static string Stringify(object obj) {
            using (MemoryStream stream = new MemoryStream()) {
                DataContractJsonSerializer js = new DataContractJsonSerializer(obj.GetType());
                js.WriteObject(stream, obj);
                stream.Position = 0;
                return ToString(stream);
            }
        }
        public static async Task<object> InvokeHandler(string name, object[] args, Context context, NextInvokeHandler next) {
            string a = Stringify(args);
            try {
                var result = await next(name, args, context);
                Trace.TraceInformation(name + "(" + a.Substring(1, a.Length - 2) + ") = " + Stringify(result));
                return result;
            }
            catch (Exception e) {
                Trace.TraceError(e.StackTrace);
                throw e;
            }
        }
    }
}
