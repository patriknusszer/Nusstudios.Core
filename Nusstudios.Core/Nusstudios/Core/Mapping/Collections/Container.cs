using System;
using System.Collections;
using System.Collections.Generic;
using Nusstudios.Core.Mapping.Collections.Generic;
using Nusstudios.Core.Parsing.Unicode;

namespace Nusstudios.Core.Mapping.Collections
{
    public abstract class Container : IDictionary
    {
        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            if (this is ArrayContainer ac) return ((IDictionary)ac.map).GetEnumerator();
            else if (this is NumericContainer nc) return ((IDictionary)nc.map).GetEnumerator();
            return ((IDictionary)((StringContainer)this).map).GetEnumerator();
        }

        public ICollection Keys
        {
            get
            {
                if (this is ArrayContainer ac) return ((IDictionary)ac.map).Keys;
                else if (this is NumericContainer nc) return ((IDictionary)nc.map).Keys;
                return ((IDictionary)((StringContainer)this).map).Keys;
            }
        }

        public ICollection Values
        {
            get
            {
                if (this is ArrayContainer ac) return ((IDictionary)ac.map).Values;
                else if (this is NumericContainer nc) return ((IDictionary)nc.map).Values;
                return ((IDictionary)((StringContainer)this).map).Values;
            }
        }

        public abstract ICollection<TKey> GetKeys<TKey>();
        public abstract ICollection<object> GetValues();
        public abstract int Count { get; }
        public abstract bool IsReadOnly { get; }
        public abstract bool IsFixedSize { get; }
        public abstract bool IsSynchronized { get; }
        public abstract Object SyncRoot { get; }
        public abstract void Clear();
        public abstract object this[object key] { get; set; }
        public abstract object Get(object key);
        public abstract TKey Get<TKey>(TKey key);
        public abstract ref object GetAlias(object key);
        public abstract ref object GetAlias<TKey>(TKey key);
        public abstract void Set(object key, object value);
        public abstract void Set<TKey>(TKey key, object value);
        public abstract void Add(object key, object value);
        public abstract void Add<TKey>(TKey key, object value);
        public abstract void Add<TKey>(KeyValuePair<TKey, object> item);
        public abstract void Add(KeyValuePair<object, object> item);
        public abstract bool Contains(object item);
        public abstract bool Contains(KeyValuePair<object, object> item);
        public abstract bool Contains<TKey>(KeyValuePair<TKey, object> item);
        public abstract bool ContainsKey(object key);
        public abstract bool ContainsKey<TKey>(TKey key);
        public abstract void CopyTo(Array array, int arrayIndex);
        public abstract void CopyTo(KeyValuePair<object, object>[] array, int arrayIndex);
        public abstract void CopyTo<TKey>(KeyValuePair<TKey, object>[] array, int arrayIndex);
        public abstract void Remove(object key);
        public abstract bool Remove<TKey>(TKey key);
        public abstract bool Remove(KeyValuePair<object, object> item);
        public abstract bool Remove<TKey>(KeyValuePair<TKey, object> item);

        public IEnumerator<KeyValuePair<TKey, object>> GetEnumerator<TKey>()
        {
            if (this is ArrayContainer ^ this is NumericContainer)
            {
                if (!(typeof(TKey) == typeof(Int32))) throw new ArgumentException();

                if (this is ArrayContainer ac) return (IEnumerator<KeyValuePair<TKey, object>>)ac.map.GetEnumerator();
                else return (IEnumerator<KeyValuePair<TKey, object>>)((NumericContainer)this).map.GetEnumerator();
            }
            else
            {
                if (!(typeof(TKey) == typeof(String))) throw new ArgumentException();
                return (IEnumerator<KeyValuePair<TKey, object>>)((StringContainer)this).map.GetEnumerator();
            }
        }

        public IEnumerator GetEnumerator()
        {
            if (this is ArrayContainer ^ this is NumericContainer) return GetEnumerator<int>();
            else return GetEnumerator<String>();
        }
    }
}
