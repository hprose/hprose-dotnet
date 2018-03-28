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
 * NullableKeyDictionary.cs                               *
 *                                                        *
 * NullableKeyDictionary class for C#.                    *
 *                                                        *
 * LastModified: Mar 28, 2018                             *
 * Authors: Ma Bingyao <andot@hprose.com>                 *
 *                                                        *
\**********************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Hprose.Collections.Generic {
    [ComVisible(false)]
    [Serializable]
    public class NullableKeyDictionary<TKey, TValue> : Dictionary<NullableKey<TKey>, TValue>,
        ICollection<KeyValuePair<TKey, TValue>>,
        IDictionary<TKey, TValue>,
        IReadOnlyCollection<KeyValuePair<TKey, TValue>>,
        IReadOnlyDictionary<TKey, TValue>,
        IEnumerable<KeyValuePair<TKey, TValue>> {

        public NullableKeyDictionary() : this(0, null) { }
        public NullableKeyDictionary(int capacity) : this(capacity, null) { }
        public NullableKeyDictionary(IEqualityComparer<TKey> comparer) : this(0, comparer) { }
        public NullableKeyDictionary(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, new NullableKeyEqualityComparer<TKey>(comparer)) { }
        public NullableKeyDictionary(IDictionary<TKey, TValue> dictionary) : this(dictionary, null) { }
        public NullableKeyDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) : base((dictionary != null) ? dictionary.Count : 0, new NullableKeyEqualityComparer<TKey>(comparer)) {
            if (dictionary != null) {
                foreach (KeyValuePair<TKey, TValue> keyValuePair in dictionary) {
                    base.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
        }

        public new IEqualityComparer<TKey> Comparer => base.Comparer == null ? null : ((NullableKeyEqualityComparer<TKey>)(base.Comparer)).Comparer;

        public bool IsReadOnly => false;

        private KeyCollection _keys;

        public new KeyCollection Keys {
            get {
                if (_keys == null) {
                    _keys = new KeyCollection(this);
                }
                return _keys;
            }
        }

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => Keys;

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

        ICollection<TValue> IDictionary<TKey, TValue>.Values => Values;

        public TValue this[TKey key] { get => base[key]; set => base[key] = value; }

        public void Add(TKey key, TValue value) => base.Add(key, value);

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> keyValuePair) => base.Add(keyValuePair.Key, keyValuePair.Value);

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> keyValuePair) => (this as ICollection<KeyValuePair<NullableKey<TKey>, TValue>>).Contains(new KeyValuePair<NullableKey<TKey>, TValue>(keyValuePair.Key, keyValuePair.Value));

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> keyValuePair) => (this as ICollection<KeyValuePair<NullableKey<TKey>, TValue>>).Remove(new KeyValuePair<NullableKey<TKey>, TValue>(keyValuePair.Key, keyValuePair.Value));

        public bool ContainsKey(TKey key) => base.ContainsKey(key);

        private Enumerator CreateEnumerator() => new Enumerator(base.GetEnumerator());

        public new Enumerator GetEnumerator() => CreateEnumerator();

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => CreateEnumerator();

        public bool Remove(TKey key) => base.Remove(key);

        public bool TryGetValue(TKey key, out TValue value) => base.TryGetValue(key, out value);

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int index) {
            ICollection<KeyValuePair<NullableKey<TKey>, TValue>> collection = this;
            if (array == null) {
                collection.CopyTo(null, index);
            }
            KeyValuePair<NullableKey<TKey>, TValue>[] arr = new KeyValuePair<NullableKey<TKey>, TValue>[array.Length - index];
            collection.CopyTo(arr, 0);
            int count = collection.Count;
            for (int i = 0; i < count; i++) {
                array[index++] = new KeyValuePair<TKey, TValue>(arr[i].Key, arr[i].Value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => CreateEnumerator();

        [Serializable]
        public new struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IDisposable, IEnumerator, IDictionaryEnumerator {
            private Dictionary<NullableKey<TKey>, TValue>.Enumerator _enumerator;

            internal Enumerator(Dictionary<NullableKey<TKey>, TValue>.Enumerator enumerator) => _enumerator = enumerator;

            public bool MoveNext() => _enumerator.MoveNext();

            public KeyValuePair<TKey, TValue> Current {
                get {
                    KeyValuePair<NullableKey<TKey>, TValue> current = _enumerator.Current;
                    return new KeyValuePair<TKey, TValue>(current.Key, current.Value);
                }
            }

            public void Dispose() { }

            object IEnumerator.Current {
                get {
                    object current = ((IEnumerator)_enumerator).Current;
                    if (current is DictionaryEntry current1) {
                        current1.Key = ((NullableKey<TKey>)current1.Key).Value;
                        return current1;
                    }
                    KeyValuePair<NullableKey<TKey>, TValue> current2 = (KeyValuePair<NullableKey<TKey>, TValue>)current;
                    return new KeyValuePair<TKey, TValue>(current2.Key, current2.Value);
                }
            }

            void IEnumerator.Reset() => ((IEnumerator)_enumerator).Reset();

            DictionaryEntry IDictionaryEnumerator.Entry {
                get {
                    DictionaryEntry entry = ((IDictionaryEnumerator)_enumerator).Entry;
                    entry.Key = ((NullableKey<TKey>)entry.Key).Value;
                    return entry;
                }
            }

            object IDictionaryEnumerator.Key => ((NullableKey<TKey>)((IDictionaryEnumerator)_enumerator).Key).Value;

            object IDictionaryEnumerator.Value => ((IDictionaryEnumerator)_enumerator).Value;
        }

        [Serializable]
        public sealed new class KeyCollection : ICollection<TKey>,
            IReadOnlyCollection<TKey>,
            IEnumerable<TKey>, IEnumerable, ICollection {

            private Dictionary<NullableKey<TKey>, TValue> _dictionary;
            private Dictionary<NullableKey<TKey>, TValue>.KeyCollection _keyCollection;

            public KeyCollection(NullableKeyDictionary<TKey, TValue> dictionary) {
                _dictionary = dictionary;
                _keyCollection = new Dictionary<NullableKey<TKey>, TValue>.KeyCollection(dictionary);
            }

            public Enumerator GetEnumerator() => new Enumerator(((IEnumerable<NullableKey<TKey>>)_keyCollection).GetEnumerator());

            public void CopyTo(TKey[] array, int index) {
                if (array == null) {
                    _keyCollection.CopyTo(null, index);
                }
                NullableKey<TKey>[] arr = new NullableKey<TKey>[array.Length - index];
                _keyCollection.CopyTo(arr, 0);
                int count = _keyCollection.Count;
                for (int i = 0; i < count; i++) {
                    array[index++] = arr[i];
                }
            }

            public int Count => _keyCollection.Count;

            bool ICollection<TKey>.IsReadOnly => true;

            void ICollection<TKey>.Add(TKey item) => ((ICollection<NullableKey<TKey>>)_keyCollection).Add(item);

            void ICollection<TKey>.Clear() => ((ICollection<NullableKey<TKey>>)_keyCollection).Clear();

            bool ICollection<TKey>.Contains(TKey item) => ((ICollection<NullableKey<TKey>>)_keyCollection).Contains(item);

            bool ICollection<TKey>.Remove(TKey item) => ((ICollection<NullableKey<TKey>>)_keyCollection).Remove(item);

            IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator() => new Enumerator(((IEnumerable<NullableKey<TKey>>)_keyCollection).GetEnumerator());

            IEnumerator IEnumerable.GetEnumerator() => new Enumerator(((IEnumerable<NullableKey<TKey>>)_keyCollection).GetEnumerator());

            void ICollection.CopyTo(Array array, int index) {
                if (array is TKey[] array2) {
                    CopyTo(array2, index);
                    return;
                }
                if (array is NullableKey<TKey>[] array3) {
                    _keyCollection.CopyTo(array3, index);
                    return;
                }
                ((ICollection)_keyCollection).CopyTo(array, index);
                object[] array4 = array as object[];
                int count = _dictionary.Count;
                for (int i = index; i < count; i++) {
                    array4[i] = ((NullableKey<TKey>)array4[i]).Value;
                }
            }

            bool ICollection.IsSynchronized => false;

            object ICollection.SyncRoot => ((ICollection)_dictionary).SyncRoot;

            [Serializable]
            public struct Enumerator : IEnumerator<TKey>, IDisposable, IEnumerator {
                private IEnumerator<NullableKey<TKey>> _enumerator;

                internal Enumerator(IEnumerator<NullableKey<TKey>> enumerator) => _enumerator = enumerator;

                public void Dispose() { }

                public bool MoveNext() => _enumerator.MoveNext();

                public TKey Current => _enumerator.Current.Value;

                object IEnumerator.Current => _enumerator.Current.Value;

                void IEnumerator.Reset() => _enumerator.Reset();
            }
        }
    }
}
