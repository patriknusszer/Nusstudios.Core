using System;
using System.Collections.Generic;
using Nusstudios.Core.Parsing.Unicode;
using Nusstudios.Core.Mapping.Collections.Generic;

namespace Nusstudios.Core.Mapping.Collections
{
    /*class UnicodeContainer : Container
    {
        public readonly Map<UnicodeStream, Object> dict = new Map<UnicodeStream, Object>();

        // abstract class implementation

        public override object this[object key]
        {
            get
            {
                return Get(key);
            }
            set
            {
                Set(key, value);
            }
        }

        public override bool Contains(object item)
        {
            return dict.Contains(item);
        }

        public override bool Contains(KeyValuePair<object, object> item)
        {
            return Contains<Object>(item);
        }

        public override bool Contains<TKey>(KeyValuePair<TKey, object> item)
        {
            if (!(item.Key is UnicodeStream))
                throw new ArgumentException();

            return Contains(new KeyValuePair<UnicodeStream, object>((UnicodeStream)(object)item.Key, item.Value));
        }

        public override void Add(object key, object value)
        {
            Add<Object>(key, value);
        }

        public override void Add<TKey>(TKey key, object value)
        {
            if (!(key is String))
                throw new ArgumentException();

            Add((UnicodeStream)(object)key, value);
        }

        public override void Add(KeyValuePair<object, object> item)
        {
            Add<object>(item);
        }

        public override void Add<TKey>(KeyValuePair<TKey, object> item)
        {
            if (!(item.Key is String))
                throw new ArgumentException();

            Add(new KeyValuePair<UnicodeStream, object>((UnicodeStream)(object)item.Key, item.Value));
        }

        public override void CopyTo(Array array, int arrayIndex)
        {
            dict.CopyTo(array, arrayIndex);
        }

        public override void CopyTo(KeyValuePair<object, object>[] array, int arrayIndex)
        {
            CopyTo<object>(array, arrayIndex);
        }

        public override void CopyTo<TKey>(KeyValuePair<TKey, object>[] array, int arrayIndex)
        {
            KeyValuePair<UnicodeStream, object>[] tmp = new KeyValuePair<UnicodeStream, object>[array.Length];

            for (int i = 0; i < tmp.Length; i++)
                tmp[i] = new KeyValuePair<UnicodeStream, object>((UnicodeStream)(object)array[i].Key, array[i].Value);

            CopyTo(tmp, arrayIndex);
        }

        public override object Get(object key)
        {
            return Get<object>(key);
        }

        public override TKey Get<TKey>(TKey key)
        {
            if (!(key is UnicodeStream))
                throw new ArgumentException();

            return (TKey)this[(UnicodeStream)(object)key];
        }

        public override ref object GetAlias(object key)
        {
            if (!(key is UnicodeStream))
                throw new ArgumentException();

            return ref GetAlias<object>(key);
        }

        public override ref object GetAlias<TKey>(TKey key)
        {
            if (!(key is UnicodeStream))
                throw new ArgumentException();

            return ref dict.GetAlias((UnicodeStream)(object)key);
        }

        public override bool ContainsKey(object key)
        {
            return ContainsKey<object>(key);
        }

        public override bool ContainsKey<TKey>(TKey key)
        {
            if (!(key is UnicodeStream))
                throw new ArgumentException();

            return ContainsKey((UnicodeStream)(object)key);
        }

        public override void Remove(object key)
        {
            Remove<object>(key);
        }

        public override bool Remove<TKey>(TKey key)
        {
            if (!(key is UnicodeStream))
                throw new ArgumentException();

            return Remove((UnicodeStream)(object)key);
        }

        public override bool Remove(KeyValuePair<object, object> item)
        {
            return Remove<object>(item);
        }

        public override bool Remove<TKey>(KeyValuePair<TKey, object> item)
        {
            if (!(item.Key is UnicodeStream))
                throw new ArgumentException();

            return Remove(new KeyValuePair<UnicodeStream, object>((UnicodeStream)(object)item.Key, item.Value));
        }

        public override void Set(object key, object value)
        {
            Set<object>(key, value);
        }

        public override void Set<TKey>(TKey key, object value)
        {
            if (!(key is UnicodeStream))
                throw new ArgumentException();

            this[(UnicodeStream)(object)key] = value;
        }

        public override ICollection<TKey> GetKeys<TKey>()
        {
            if (!(typeof(TKey) == typeof(UnicodeStream)))
                throw new ArgumentException();

            return (ICollection<TKey>)(object)dict.Keys;
        }


        public override ICollection<object> GetValues()
        {
            return dict.Values;
        }

        public new IEnumerator<KeyValuePair<UnicodeStream, object>> GetEnumerator()
        {
            return dict.GetEnumerator();
        }

        new public ICollection<UnicodeStream> Keys => dict.Keys;
        public override int Count => dict.Count;
        public override bool IsReadOnly => dict.IsReadOnly;
        public override bool IsFixedSize => dict.IsFixedSize;
        public override bool IsSynchronized => dict.IsSynchronized;
        public override object SyncRoot => dict.SyncRoot;

        public override void Clear()
        {
            dict.Clear();
        }

        // abstract class implementation

        public object this[UnicodeStream key] { get => dict[key]; set => dict[key] = value; }

        public void Add(UnicodeStream key, object value)
        {
            dict[key] = value;
        }

        public void Add(KeyValuePair<UnicodeStream, object> item)
        {
            dict.Add(item);
        }

        public ref object GetAlias(UnicodeStream key)
        {
            return ref dict.GetAlias(key);
        }

        public bool Contains(KeyValuePair<UnicodeStream, object> item)
        {
            return dict.Contains(item);
        }

        public bool ContainsKey(UnicodeStream key)
        {
            return dict.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<UnicodeStream, object>[] array, int arrayIndex)
        {
            dict.CopyTo(array, arrayIndex);
        }

        public bool Remove(UnicodeStream key)
        {
            return dict.Remove(key);
        }

        public bool Remove(KeyValuePair<UnicodeStream, object> item)
        {
            return dict.Remove(item);
        }

        public bool TryGetValue(UnicodeStream key, out object value)
        {
            return dict.TryGetValue(key, out value);
        }
    } */
}
