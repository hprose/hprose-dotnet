/* HashMap class.
 * This library is free. You can redistribute it and/or modify it.
 */
#if !(SILVERLIGHT || WINDOWS_PHONE || Core || PORTABLE)
using System;
namespace System.Collections {
    public class HashMap: Hashtable, IDictionary, ICollection, IEnumerable, ICloneable {
        private object valueOfNullKey = null;
        private bool hasNullKey = false;
        public HashMap(): base() {
        }
        public HashMap(int capacity): base(capacity) {
        }
#if !dotNETMF
        public HashMap(int capacity, float loadFactor) : base(capacity, loadFactor) {
        }
        public HashMap(IDictionary value) : base(value) {
        }
#else
        public HashMap(int capacity, int maxLoadFactor) : base(capacity, maxLoadFactor) {
        }
#endif
#if !dotNETMF
        public override object Clone() {
#else
        public new object Clone() {
#endif
            HashMap m = (HashMap)base.Clone();
            m.valueOfNullKey = valueOfNullKey;
            m.hasNullKey = hasNullKey;
            return m;
        }
#if !dotNETMF
        public override object this[object key] {
#else
        public new object this[object key] {
#endif
            get {
                if (key == null) return valueOfNullKey;
                return base[key];
            }
            set {
                if (key == null) {
                    valueOfNullKey = value;
                    hasNullKey = true;
                }
                else {
                    base[key] = value;
                }
            }
        }
#if !dotNETMF
        public override void Add(object key, object value) {
#else
        public new void Add(object key, object value) {
#endif
            if (key == null) {
                if (hasNullKey) return;
                valueOfNullKey = value;
                hasNullKey = true;
            }
            else {
                base.Add(key, value);
            }
        }
#if !dotNETMF
        public override bool Contains(object key) {
#else
        public new bool Contains(object key) {
#endif
            if (key == null) return hasNullKey;
            return base.Contains(key);
        }
#if !dotNETMF
        public override void CopyTo(Array array, int arrayIndex) {
#else
        public new void CopyTo(Array array, int arrayIndex) {
#endif
            if (hasNullKey) {
                base.CopyTo(array, arrayIndex + 1);
#if !dotNETMF
                array.SetValue(new DictionaryEntry(null, valueOfNullKey), arrayIndex);
#else
                ((IList)array)[arrayIndex] = new DictionaryEntry(null, valueOfNullKey);
#endif
            }
            else {
                base.CopyTo(array, arrayIndex);
            }
        }
#if !dotNETMF
        public override bool ContainsKey(object key) {
            if (key == null) return hasNullKey;
            return base.ContainsKey(key);
        }
        public override bool ContainsValue(object value) {
            if (hasNullKey && (valueOfNullKey == value)) return true;
            return base.ContainsValue(value);
        }
#endif
#if !dotNETMF
        public override void Remove(object key) {
#else
        public new void Remove(object key) {
#endif
            if (key == null) {
                valueOfNullKey = null;
                hasNullKey = false;
            }
            else {
                base.Remove(key);
            }
        }
#if !dotNETMF
        public override int Count {
#else
        public new int Count {
#endif
            get {
                return base.Count + (hasNullKey ? 1 : 0);
            }
        }
#if !dotNETMF
        public override void Clear() {
#else
        public new void Clear() {
#endif
            valueOfNullKey = null;
            hasNullKey = false;
            base.Clear();
        }
#if !dotNETMF
        public override IDictionaryEnumerator GetEnumerator() {
            IDictionaryEnumerator e = base.GetEnumerator();
            if (hasNullKey) {
                return new HashMapEnumerator(e, valueOfNullKey, 3);
            }
            else {
                return e;
            }
        }
#endif
        IEnumerator IEnumerable.GetEnumerator() {
            IEnumerator e = base.GetEnumerator();
            if (hasNullKey) {
                return new HashMapEnumerator(e, valueOfNullKey, 3);
            }
            else {
                return e;
            }
        }
#if !dotNETMF
        public override ICollection Keys {
#else
        public new ICollection Keys {
#endif
            get {
                return new KeysCollection(this, base.Keys);
            }
        }
#if !dotNETMF
        public override ICollection Values {
#else
        public new ICollection Values {
#endif
            get {
                return new ValuesCollection(this, base.Values);
            }
        }

        private class HashMapEnumerator:
#if !dotNETMF
        IDictionaryEnumerator, 
#endif
        IEnumerator, ICloneable {
            private IEnumerator e;
            private object v;
            private int p;
            private int t;

            internal HashMapEnumerator(IEnumerator e, object v, int t) {
                this.e = e;
                this.v = v;
                this.t = t;
                this.p = -1;
            }
            private HashMapEnumerator(IEnumerator e, object v, int t, int p) {
                this.e = e;
                this.v = v;
                this.t = t;
                this.p = p;
            }
            public object Clone() {
                return new HashMapEnumerator(e, v, t, p);
            }
            public virtual bool MoveNext() {
                if (++p > 0) {
                    return e.MoveNext();
                }
                return true;
            }
            public virtual void Reset() {
                p = -1;
                e.Reset();
            }
            public virtual object Current {
                get {
                    if (p == 0) {
                        if (t == 1) {
                            return null;
                        }
                        if (t == 2) {
                            return v;
                        }
                        return new DictionaryEntry(null, v);
                    }
                    return e.Current;
                }
            }
            public virtual DictionaryEntry Entry {
                get {
                    if (p == 0) {
                        return new DictionaryEntry(null, v);
                    }
                    return (DictionaryEntry)e.Current;
                }
            }
            public virtual object Key {
                get {
                    if (p == 0) {
                        return null;
                    }
#if !dotNETMF
                    return ((IDictionaryEnumerator)e).Key;
#else
                    return ((DictionaryEntry)e.Current).Key;
#endif
                }
            }
            public virtual object Value {
                get {
                    if (p == 0) {
                        return v;
                    }
#if !dotNETMF
                    return ((IDictionaryEnumerator)e).Value;
#else
                    return ((DictionaryEntry)e.Current).Value;
#endif
                }
            }
        }

        private class KeysCollection : ICollection, IEnumerable {
            private HashMap m;
            private ICollection keys;

            internal KeysCollection(HashMap m, ICollection keys) {
                this.m = m;
                this.keys = keys;
            }
            public virtual void CopyTo(Array array, int arrayIndex) {
                if (m.hasNullKey) {
                    keys.CopyTo(array, arrayIndex + 1);
#if !dotNETMF
                    array.SetValue(null, arrayIndex);
#else
                    ((IList)array)[arrayIndex] = null;
#endif
                }
                else {
                    keys.CopyTo(array, arrayIndex);
                }
            }
            public virtual IEnumerator GetEnumerator() {
                IEnumerator e = keys.GetEnumerator();
                if (m.hasNullKey) {
                    return new HashMapEnumerator(e, m.valueOfNullKey, 1);
                }
                else {
                    return e;
                }
            }
            public virtual int Count {
                get {
                    return keys.Count + (m.hasNullKey ? 1 : 0);
                }
            }
            public virtual bool IsSynchronized {
                get {
                    return keys.IsSynchronized;
                }
            }
            public virtual object SyncRoot {
                get {
                    return keys.SyncRoot;
                }
            }
        }
        private class ValuesCollection : ICollection, IEnumerable {
            private HashMap m;
            private ICollection values;

            internal ValuesCollection(HashMap m, ICollection values) {
                this.m = m;
                this.values = values;
            }
            public virtual void CopyTo(Array array, int arrayIndex) {
                if (m.hasNullKey) {
                    values.CopyTo(array, arrayIndex + 1);
#if !dotNETMF
                    array.SetValue(m.valueOfNullKey, arrayIndex);
#else
                    ((IList)array)[arrayIndex] = m.valueOfNullKey;
#endif
                }
                else {
                    values.CopyTo(array, arrayIndex);
                }
            }
            public virtual IEnumerator GetEnumerator() {
                IEnumerator e = values.GetEnumerator();
                if (m.hasNullKey) {
                    return new HashMapEnumerator(e, m.valueOfNullKey, 2);
                }
                else {
                    return e;
                }
            }
            public virtual int Count {
                get {
                    return values.Count + (m.hasNullKey ? 1 : 0);
                }
            }
            public virtual bool IsSynchronized {
                get {
                    return values.IsSynchronized;
                }
            }
            public virtual object SyncRoot {
                get {
                    return values.SyncRoot;
                }
            }
        }
    }
}
#endif