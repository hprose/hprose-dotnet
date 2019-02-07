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
|  LastModified: Feb 8, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Hprose.RPC {
    public class Context : DynamicObject, ICloneable {
        public Dictionary<string, object> Items { get; } = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        public dynamic this[string name] {
            get => Items[name];
            set => Items[name] = value;
        }
        protected static void Copy(IDictionary<string, object> src, IDictionary<string, object> dist) {
            if (src != null) {
                foreach (var p in src) dist[p.Key] = p.Value;
            }
        }
        public override bool TryGetMember(GetMemberBinder binder, out object result) {
            return Items.TryGetValue(binder.Name, out result);

        }
        public override bool TrySetMember(SetMemberBinder binder, object value) {
            Items[binder.Name] = value;
            return true;
        }
        public override bool TryDeleteMember(DeleteMemberBinder binder) {
            try {
                Items.Remove(binder.Name);
                return true;
            }
            catch {
                return false;
            }
        }
        public bool Contains(string name) {
            return Items.ContainsKey(name);
        }

        public object Clone() {
            return MemberwiseClone();
        }
    }
}