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
|  LastModified: Jan 27, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Hprose.RPC {
    public class Context : DynamicObject, ICloneable {
        public ExpandoObject RequestHeaders { get; } = new ExpandoObject();
        public ExpandoObject ResponseHeaders { get; } = new ExpandoObject();
        protected readonly Dictionary<string, object> items = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        protected void Copy(IDictionary<string, object> src, IDictionary<string, object> dist) {
            if (src != null) {
                foreach (var p in src) dist[p.Key] = p.Value;
            }
        }
        public override bool TryGetMember(GetMemberBinder binder, out object result) {
            return items.TryGetValue(binder.Name, out result);

        }
        public override bool TrySetMember(SetMemberBinder binder, object value) {
            items[binder.Name] = value;
            return true;
        }
        public override bool TryDeleteMember(DeleteMemberBinder binder) {
            try {
                items.Remove(binder.Name);
                return true;
            }
            catch {
                return false;
            }
        }
        public bool Contains(string name) {
            return items.ContainsKey(name);
        }

        public object Clone() {
            return MemberwiseClone();
        }
    }
}