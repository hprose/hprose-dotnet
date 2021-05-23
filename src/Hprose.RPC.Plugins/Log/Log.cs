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
|  LastModified: Mar 29, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Hprose.RPC.Plugins.Log {
    public class Log {
        private static readonly Log instance = new();
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
        private static string ToString(MemoryStream stream) {
            var data = stream.GetArraySegment();
            try {
                return Encoding.UTF8.GetString(data.Array, data.Offset, data.Count);
            }
            catch {
                return Encoding.Default.GetString(data.Array, data.Offset, data.Count);
            }
        }
        private static string Stringify(object obj) {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
        public static async Task<Stream> IOHandler(this Log log, Stream request, Context context, NextIOHandler next) {
            bool enabled = context.Contains("log") ? (bool)context["log"] : log.Enabled;
            if (!enabled) return await next(request, context).ConfigureAwait(false);
            var stream = await request.ToMemoryStream().ConfigureAwait(false);
#if !NET35_CF
            Trace.TraceInformation(ToString(stream));
#else
            Trace.WriteLine(ToString(stream));
#endif
            try {
                var response = await next(stream, context).ConfigureAwait(false);
                stream = await response.ToMemoryStream().ConfigureAwait(false);
#if !NET35_CF
                Trace.TraceInformation(ToString(stream));
#else
                Trace.WriteLine(ToString(stream));
#endif
                return stream;
            }
            catch (Exception e) {
#if !NET35_CF
                Trace.TraceError(e.StackTrace);
#else
                Trace.WriteLine(e.StackTrace);
#endif
                throw;
            }
        }
        public static async Task<object> InvokeHandler(this Log log, string name, object[] args, Context context, NextInvokeHandler next) {
            bool enabled = context.Contains("log") ? (bool)context["log"] : log.Enabled;
            if (!enabled) return await next(name, args, context).ConfigureAwait(false);
            string a = "";
            try {
                a = Stringify(args);
            }
            catch (Exception e) {
#if !NET35_CF
                Trace.TraceError(e.StackTrace);
#else
                Trace.WriteLine(e.StackTrace);
#endif
            }
            try {
                var result = await next(name, args, context).ConfigureAwait(false);
                try {
#if !NET35_CF
                    Trace.TraceInformation(name + "(" + a.Substring(1, a.Length - 2) + ") = " + Stringify(result));
#else
                    Trace.WriteLine(name + "(" + a.Substring(1, a.Length - 2) + ") = " + Stringify(result));
#endif
                }
                catch (Exception e) {
#if !NET35_CF
                    Trace.TraceError(e.StackTrace);
#else
                    Trace.WriteLine(e.StackTrace);
#endif
                }
                return result;
            }
            catch (Exception e) {
#if !NET35_CF
                Trace.TraceError(e.StackTrace);
#else
                Trace.WriteLine(e.StackTrace);
#endif
                throw;
            }
        }
    }
}
