/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  Context.cs                                              |
|                                                          |
|  Context class for C#.                                   |
|                                                          |
|  LastModified: Mar 29, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Generic;

namespace Hprose.RPC {
    public class Context : ICloneable {
        public IDictionary<string, object> Items { get; private set; } = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
        public IDictionary<string, object> RequestHeaders { get; private set; } = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
        public IDictionary<string, object> ResponseHeaders { get; private set; } = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
        public object this[string name] {
            get => Items[name];
            set => Items[name] = value;
        }
        protected static void Copy(IDictionary<string, object> src, IDictionary<string, object> dist) {
            if (src != null) {
                foreach (var p in src) dist[p.Key] = p.Value;
            }
        }
        public bool Contains(string name) {
            return Items.ContainsKey(name);
        }
        public virtual void CopyTo(Context context) {
            context.Items = new Dictionary<string, object>(Items, StringComparer.InvariantCultureIgnoreCase);
            context.RequestHeaders = new Dictionary<string, object>(RequestHeaders, StringComparer.InvariantCultureIgnoreCase);
            context.ResponseHeaders = new Dictionary<string, object>(ResponseHeaders, StringComparer.InvariantCultureIgnoreCase);
        }
        public virtual object Clone() {
            var context = MemberwiseClone() as Context;
            CopyTo(context);
            return context;
        }
    }
}