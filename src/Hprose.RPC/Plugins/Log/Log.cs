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
|  LastModified: Feb 2, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Hprose.RPC.Plugins.Log {
    public class Log {
        private static readonly Log instance = new Log();
        public bool Enabled { get; set; }
        public Log(bool enabled = true) {
            Enabled = enabled;
        }
        public static Task<Stream> IOHandler(Stream request, Context context, NextIOHandler next) {
            return LogExtensions.IOHandler(instance, request, context, next);
        }
        public static Task<object> InvokeHandler(string name, object[] args, Context context, NextInvokeHandler next) {
            return LogExtensions.InvokeHandler(instance, name, args, context, next);
        }
    }
    public static class LogExtensions {
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
        private static string Stringify(object obj) {
            using (MemoryStream stream = new MemoryStream()) {
                DataContractJsonSerializer js = new DataContractJsonSerializer(obj.GetType());
                js.WriteObject(stream, obj);
                stream.Position = 0;
                return ToString(stream);
            }
        }
        public static async Task<Stream> IOHandler(this Log log, Stream request, Context context, NextIOHandler next) {
            bool enabled = context.Contains("Log") ? (context as dynamic).Log : log.Enabled;
            if (!enabled) return await next(request, context);
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
        public static async Task<object> InvokeHandler(this Log log, string name, object[] args, Context context, NextInvokeHandler next) {
            bool enabled = context.Contains("Log") ? (context as dynamic).Log : log.Enabled;
            if (!enabled) return await next(name, args, context);
            string a = typeof(Context).IsAssignableFrom(args.Last().GetType()) ? Stringify(new List<object>(args.Take(args.Length - 1))) : Stringify(args);
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
