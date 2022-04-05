using System;
using System.Collections.Generic;
using Nusstudios.Core.Mapping.Collections;
using Nusstudios.Core.Mapping.Collections.Generic;

namespace Nusstudios.Core.Mapping.Collections
{
    public class NumericContainer : Container, IEnumerable<KeyValuePair<int, Object>>
    {
        internal readonly HashMap<int, Object> map = new HashMap<int, Object>();

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
            if (item.Key is Int32 i) return Contains(new KeyValuePair<int, object>(i, item.Value));
            else throw new ArgumentException();
        }

        public override void Add(object key, object value) => Add<Object>(key, value);

        public override void Add<TKey>(TKey key, object value)
        {
            if (key is Int32 i) Add(i, value);
            else throw new ArgumentException();
        }

        public override void Add(KeyValuePair<object, object> item) => Add<object>(item);

        public override void Add<TKey>(KeyValuePair<TKey, object> item)
        {
            if (item.Key is Int32 i) Add(new KeyValuePair<int, object>(i, item.Value));
            else throw new ArgumentException();
        }

        public override void CopyTo(Array array, int arrayIndex) => map.CopyTo(array, arrayIndex);
        public override void CopyTo(KeyValuePair<object, object>[] array, int arrayIndex) => CopyTo<object>(array, arrayIndex);

        public override void CopyTo<TKey>(KeyValuePair<TKey, object>[] array, int arrayIndex)
        {
            KeyValuePair<int, object>[] tmp = new KeyValuePair<int, object>[array.Length];
            for (int i = 0; i < tmp.Length; i++) tmp[i] = new KeyValuePair<int, object>((int)(object)array[i].Key, array[i].Value);
            CopyTo(tmp, arrayIndex);
        }

        public override object Get(object key) => Get<object>(key);

        public override TKey Get<TKey>(TKey key)
        {
            if (key is Int32 i) return (TKey)this[i];
            else throw new ArgumentException();
        }

        public override ref object GetAlias(object key) => ref GetAlias<object>(key);

        public override ref object GetAlias<TKey>(TKey key)
        {
            if (key is Int32 i) return ref map.GetAlias(i);
            else throw new ArgumentException();
        }

        public override bool ContainsKey(object key) => ContainsKey<object>(key);

        public override bool ContainsKey<TKey>(TKey key)
        {
            if (key is Int32 i) return ContainsKey(i);
            else throw new ArgumentException();
        }

        public override void Remove(object key) => Remove<object>(key);

        public override bool Remove<TKey>(TKey key)
        {
            if (key is Int32 i) return Remove(i);
            else throw new ArgumentException();
        }

        public override bool Remove(KeyValuePair<object, object> item) => Remove<object>(item);

        public override bool Remove<TKey>(KeyValuePair<TKey, object> item)
        {
            if (item.Key is Int32 i) return Remove(new KeyValuePair<int, object>(i, item.Value));
            else throw new ArgumentException();
        }

        public override void Set(object key, object value) => Set<object>(key, value);

        public override void Set<TKey>(TKey key, object value)
        {
            if (key is Int32 i) this[i] = value;
            else throw new ArgumentException();
        }

        public override ICollection<TKey> GetKeys<TKey>()
        {
            if (!(typeof(TKey) == typeof(Int32))) throw new ArgumentException();
            return (ICollection<TKey>)(object)map.Keys;
        }

        public override ICollection<object> GetValues() => map.Values;
        public new IEnumerator<KeyValuePair<int, object>> GetEnumerator() => map.GetEnumerator();
        new public ICollection<int> Keys => map.Keys;
        public override int Count => map.Count;
        public override bool IsReadOnly => map.IsReadOnly;
        public override bool IsFixedSize => map.IsFixedSize;
        public override bool IsSynchronized => map.IsSynchronized;
        public override object SyncRoot => map.SyncRoot;
        public override void Clear() => map.Clear();

        // abstract class implementation

        public object this[int key] { get => map[key]; set => map[key] = value; }
        public void Add(int key, object value) => map[key] = value;
        public void Add(KeyValuePair<int, object> item) => map.Add(item);
        public ref object GetAlias(int key) => ref map.GetAlias(key);
        public bool Contains(KeyValuePair<int, object> item) => map.Contains(item);
        public bool ContainsKey(int key) => map.ContainsKey(key);
        public void CopyTo(KeyValuePair<int, object>[] array, int arrayIndex) => map.CopyTo(array, arrayIndex);
        public bool Remove(int key) => map.TryRemove(key);
        public bool Remove(KeyValuePair<int, object> item) => map.Remove(item);
        public bool TryGetValue(int key, out object value) => map.TryGetValue(key, out value);
    }
}
