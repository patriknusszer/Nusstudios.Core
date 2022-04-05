using System;
using System.Collections.Generic;
using Nusstudios.Core.Mapping.Collections.Generic;

namespace Nusstudios.Core.Mapping.Collections
{
    public class StringContainer : Container, IEnumerable<KeyValuePair<string, Object>>
    {
        public readonly HashMap<string, Object> map = new HashMap<string, object>();

        // abstract class implementation

        public override object this[object key]
        {
            get => Get(key);
            set => Set(key, value);
        }

        public override bool Contains(object item) => map.Contains(item);
        public override bool Contains(KeyValuePair<object, object> item) => Contains<Object>(item);

        public override bool Contains<TKey>(KeyValuePair<TKey, object> item)
        {
            if (item.Key is String s) return Contains(new KeyValuePair<string, object>(s, item.Value));
            else throw new ArgumentException();
        }

        public override void Add(object key, object value) => Add<Object>(key, value);

        public override void Add<TKey>(TKey key, object value)
        {
            if (key is String s) Add(s, value);
            else throw new ArgumentException();
        }

        public override void Add(KeyValuePair<object, object> item) => Add<object>(item);

        public override void Add<TKey>(KeyValuePair<TKey, object> item)
        {
            if (item.Key is String s) Add(new KeyValuePair<string, object>(s, item.Value));
            else throw new ArgumentException();
        }

        public override void CopyTo(Array array, int arrayIndex) => map.CopyTo(array, arrayIndex);
        public override void CopyTo(KeyValuePair<object, object>[] array, int arrayIndex) => CopyTo<object>(array, arrayIndex);

        public override void CopyTo<TKey>(KeyValuePair<TKey, object>[] array, int arrayIndex)
        {
            KeyValuePair<string, object>[] tmp = new KeyValuePair<string, object>[array.Length];
            for (int i = 0; i < tmp.Length; i++) tmp[i] = new KeyValuePair<string, object>((string)(object)array[i].Key, array[i].Value);
            CopyTo(tmp, arrayIndex);
        }

        public override object Get(object key) => Get<object>(key);

        public override TKey Get<TKey>(TKey key)
        {
            if (key is String s) return (TKey)this[s];
            else throw new ArgumentException();
        }

        public override ref object GetAlias(object key)
        {
            if (key is String s) return ref GetAlias<object>(s);
            else throw new ArgumentException();
        }

        public override ref object GetAlias<TKey>(TKey key)
        {
            if (key is String s) return ref map.GetAlias(s);
            else throw new ArgumentException();
        }

        public override bool ContainsKey(object key) => ContainsKey<object>(key);

        public override bool ContainsKey<TKey>(TKey key)
        {
            if (key is String s) return ContainsKey(s);
            else throw new ArgumentException();
        }

        public override void Remove(object key) => Remove<object>(key);

        public override bool Remove<TKey>(TKey key)
        {
            if (key is String s) return Remove(s);
            else throw new ArgumentException();
        }

        public override bool Remove(KeyValuePair<object, object> item) => Remove<object>(item);

        public override bool Remove<TKey>(KeyValuePair<TKey, object> item)
        {
            if (item.Key is String s) return Remove(new KeyValuePair<string, object>(s, item.Value));
            else throw new ArgumentException();
        }

        public override void Set(object key, object value) => Set<object>(key, value);

        public override void Set<TKey>(TKey key, object value)
        {
            if (key is String s) this[s] = value;
            else throw new ArgumentException();
        }

        public override ICollection<TKey> GetKeys<TKey>()
        {
            if (!(typeof(TKey) == typeof(String))) throw new ArgumentException();
            return (ICollection<TKey>)(object)map.Keys;
        }

        public override ICollection<object> GetValues() => map.Values;
        public new IEnumerator<KeyValuePair<string, object>> GetEnumerator() => map.GetEnumerator();
        new public ICollection<string> Keys => map.Keys;
        public override int Count => map.Count;
        public override bool IsReadOnly => map.IsReadOnly;
        public override bool IsFixedSize => map.IsFixedSize;
        public override bool IsSynchronized => map.IsSynchronized;
        public override object SyncRoot => map.SyncRoot;
        public override void Clear() => map.Clear();

        // abstract class implementation

        public object this[string key] { get => map[key]; set => map[key] = value; }
        public void Add(string key, object value) => map[key] = value;
        public void Add(KeyValuePair<string, object> item) => map.Add(item);
        public ref object GetAlias(String key) => ref map.GetAlias(key);
        public bool Contains(KeyValuePair<string, object> item) => map.Contains(item);
        public bool ContainsKey(string key) => map.ContainsKey(key);
        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex) => map.CopyTo(array, arrayIndex);
        public bool Remove(string key) => map.TryRemove(key);
        public bool Remove(KeyValuePair<string, object> item) => map.Remove(item);
        public bool TryGetValue(string key, out object value) => map.TryGetValue(key, out value);
    }
}
