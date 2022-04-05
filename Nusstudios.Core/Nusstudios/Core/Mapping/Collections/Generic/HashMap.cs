
using System;
using System.Linq;
using Nusstudios.Core;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Nusstudios.Core.Mapping.Collections.Generic
{
    public partial class HashMap<TKey, TValue> : IDictionary<TKey, TValue>, IDictionary
    {
        class DefaultHasher<T> : IEquComp<T>
        {
            ulong rnd;

            public DefaultHasher()
            {
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] buffer = new byte[8];
                rng.GetNonZeroBytes(buffer);

                for (int i = 1; i <= buffer.Length; i++)
                {
                    rnd |= buffer[i - 1];
                    rnd <<= (i * 8);
                }
            }

            int prime = HashCore.HashPrime;

            public bool Equals(T x, T y)
            {
                if (x is IEnumerable<byte> byteArr) return byteArr.SequenceEqual((IEnumerable<byte>)(object)y);
                if (x is IEnumerable<sbyte> sbyteArr) return sbyteArr.SequenceEqual((IEnumerable<sbyte>)(object)y);
                if (x is IEnumerable<ushort> usArr) return usArr.SequenceEqual((IEnumerable<ushort>)(object)y);
                if (x is IEnumerable<short> sArr) return sArr.SequenceEqual((IEnumerable<short>)(object)y);
                if (x is IEnumerable<uint> uiArr) return uiArr.SequenceEqual((IEnumerable<uint>)(object)y);
                if (x is IEnumerable<int> iArr) return iArr.SequenceEqual((IEnumerable<int>)(object)y);
                if (x is IEnumerable<ulong> ulArr) return ulArr.SequenceEqual((IEnumerable<ulong>)(object)y);
                if (x is IEnumerable<long> lArr) return lArr.SequenceEqual((IEnumerable<long>)(object)y);
                if (x is IEnumerable<char> cArr) return cArr.SequenceEqual((IEnumerable<char>)(object)y);
                if (x is IEnumerable<string> strArr) return strArr.SequenceEqual((IEnumerable<string>)(object)y);
                else return x.Equals(y);
            }

            public ulong GetHashCode(T x)
            {
                ulong hash;
                if (x is IEnumerable<byte> byteArr) hash = HashCore.RolloutHash32(byteArr, prime);
                else if (x is IEnumerable<sbyte> sbyteArr) hash = HashCore.RolloutHash32(sbyteArr, prime);
                else if (x is IEnumerable<ushort> usArr) hash = HashCore.RolloutHash32(usArr, prime);
                else if (x is IEnumerable<short> sArr) hash = HashCore.RolloutHash32(sArr, prime);
                else if (x is IEnumerable<uint> uiArr) hash = HashCore.RolloutHash32(uiArr, prime);
                else if (x is IEnumerable<int> iArr) hash = HashCore.RolloutHash32(iArr, prime);
                else if (x is IEnumerable<ulong> ulArr) hash = HashCore.RolloutHash32(ulArr, prime);
                else if (x is IEnumerable<long> lArr) hash = HashCore.RolloutHash32(lArr, prime);
                else if (x is IEnumerable<char> cArr) hash = HashCore.RolloutHash32(cArr, prime);
                else if (x is IEnumerable<string> strArr) hash = HashCore.RolloutHash32(strArr, prime);
                // else if (x is string str) return HashHelpers.RolloutHash32(str, prime);
                else hash = (ulong)x.GetHashCode();

                return HashCore.Mix64(hash, rnd);
            }

            public bool UpdateHashesDueToTablePrimeChange(int newModuloPrime) => false;
        }

        private class Entry
        {
            public int PreviousUnusedIndex = -1;
            public bool Used = false;
            public int PreviousUsedIndexInBucket = -1;
            public int PreviousUsedIndex = -1;
            public int NextUsedIndex = -1;
            public int NextUsedIndexInBucket = -1;
            public int SelfIndex = -1;
            public TKey Key;
            public ulong HashCode;
            public TValue Value;
        }

        private class Bucket
        {
            public int OldestUsedEntryIndex = -1;
            public int LatestUsedEntryIndex = -1;
            public int PreviousUsedBucketIndex = -1;
            public int NextUsedBucketIndex = -1;
            public int UsedCount = 0;
            public bool IsEmpty => UsedCount == 0;

            public void UnregisterEntry(EntryCollection entries, int index, out bool deactivated)
            {
                Entry e = entries[index];
                if (e.PreviousUsedIndexInBucket != -1)
                    entries[e.PreviousUsedIndexInBucket].NextUsedIndexInBucket = e.NextUsedIndexInBucket;

                if (e.NextUsedIndexInBucket != -1)
                    entries[e.NextUsedIndexInBucket].PreviousUsedIndexInBucket = e.PreviousUsedIndexInBucket;

                UsedCount--;

                if (index == LatestUsedEntryIndex) LatestUsedEntryIndex = e.PreviousUsedIndexInBucket;

                if (IsEmpty)
                    deactivated = true;

                else deactivated = false;
            }

            public void RegisterEntry(EntryCollection entries, int insertIndex, bool overrideRegistry, out bool activated)
            {
                if (IsEmpty)
                {
                    if (overrideRegistry)
                    {
                        Entry e = entries[insertIndex];
                        e.PreviousUsedIndexInBucket = -1;
                        e.NextUsedIndexInBucket = -1;
                    }

                    OldestUsedEntryIndex = insertIndex;
                    LatestUsedEntryIndex = insertIndex;
                    activated = true;
                }
                else
                {
                    entries[LatestUsedEntryIndex].NextUsedIndexInBucket = insertIndex;
                    Entry e = entries[insertIndex];
                    e.PreviousUsedIndexInBucket = LatestUsedEntryIndex;
                    if (overrideRegistry) e.NextUsedIndexInBucket = -1;
                    LatestUsedEntryIndex = insertIndex;
                    activated = false;
                }

                UsedCount++;
            }
        }

        class EntryCollection : IEnumerable<Entry>
        {
            public void Clear()
            {
                latestUnusedIndex = -1;
                unusedCount = entries.Length;

                foreach (Entry e in this)
                {
                    e.Used = false;
                    e.PreviousUnusedIndex = latestUnusedIndex;
                    latestUnusedIndex = e.SelfIndex;
                }
            }

            public IEnumerator<Entry> GetEnumerator() => new Iterator(entries, latestUsedIndex);
            IEnumerator IEnumerable.GetEnumerator() => new Iterator(entries, latestUsedIndex);

            private class Iterator : IEnumerator<Entry>
            {
                private int currentUsedIndex = -1;
                private int latestUsedIndex = -1;
                private Entry[] entries;

                public Iterator(Entry[] entries, int latestUsedIndex)
                {
                    this.entries = entries;
                    this.latestUsedIndex = latestUsedIndex;
                    currentUsedIndex = latestUsedIndex;
                }

                public Entry Current { get; private set; }
                object IEnumerator.Current => Current;

                public void Dispose() { }

                public bool MoveNext()
                {
                    if (currentUsedIndex != -1)
                    {
                        Current = entries[currentUsedIndex];
                        currentUsedIndex = Current.PreviousUsedIndex;
                        return true;
                    }
                    else return false;
                }

                public void Reset() => currentUsedIndex = latestUsedIndex;
            }

            public bool Resize(int newSize)
            {
                if (newSize <= entries.Length) return false;
                unusedCount += newSize - entries.Length;
                Entry[] tmp = new Entry[entries.Length];
                Array.Copy(entries, tmp, tmp.Length);
                entries = new Entry[newSize];
                Array.Copy(tmp, entries, tmp.Length);

                for (int i = tmp.Length; ++i < newSize;)
                {
                    Entry e = entries[i] = new Entry();
                    e.SelfIndex = i;
                    e.PreviousUnusedIndex = latestUnusedIndex;
                    latestUnusedIndex = i;
                }

                return true;
            }

            public EntryCollection(int capacity)
            {
                entries = new Entry[capacity];
                unusedCount = capacity;

                for (int i = -1; ++i < capacity;)
                {
                    Entry e = entries[i] = new Entry();
                    e.SelfIndex = i;
                    e.PreviousUnusedIndex = latestUnusedIndex;
                    latestUnusedIndex = i;
                }
            }

            private Entry[] entries;
            private bool married = false;

            public bool Marry()
            {
                if (married) return false;
                married = true;
                return true;
            }

            private int latestUnusedIndex = -1;
            private int latestUsedIndex = -1;
            private int unusedCount = 0;

            public int Count => entries.Length;
            public int UsedCount => entries.Length - unusedCount;

            public bool UpdateEntryValue(int entryIndex, TValue value)
            {
                Entry e = entries[entryIndex];

                if (!e.Used) return false;
                e.Value = value;
                return true;
            }

            public bool UpdateEntryHashCode(int entryIndex, ulong hashCode)
            {
                Entry e = entries[entryIndex];

                if (!e.Used) return false;
                e.HashCode = hashCode;
                return true;
            }

            public int PlaceEntry(TKey key, ulong hashCode, TValue value)
            {
                // same as unusedCount == 0
                if (latestUnusedIndex == -1) return -1;
                int insertIndex = latestUnusedIndex;
                Entry e = entries[insertIndex];
                e.Key = key;
                e.Value = value;
                e.HashCode = hashCode;
                e.Used = true;
                latestUnusedIndex = e.PreviousUnusedIndex;
                unusedCount--;

                e.PreviousUsedIndex = latestUsedIndex;

                if (e.PreviousUsedIndex != -1)
                    entries[e.PreviousUsedIndex].NextUsedIndex = insertIndex;

                latestUsedIndex = insertIndex;

                return insertIndex;
            }

            public bool RenderEntryUnused(int entryIndex)
            {
                Entry e = entries[entryIndex];

                if (e.Used)
                {
                    e.Used = false;
                    e.PreviousUnusedIndex = latestUnusedIndex;
                    latestUnusedIndex = entryIndex;
                    unusedCount++;

                    if (e.PreviousUsedIndex != -1)
                        entries[e.PreviousUsedIndex].NextUsedIndex = e.NextUsedIndex;

                    if (e.NextUsedIndex != -1)
                        entries[e.NextUsedIndex].PreviousUsedIndex = e.PreviousUsedIndex;

                    return true;
                }
                else return false;
            }

            public Entry this[int index]
            {
                get => entries[index];
                set => entries[index] = value;
            }
        }

        class BucketCollection : IEnumerable<Bucket>
        {
            public void Clear()
            {
                latestBucketWithUsedIndex = -1;
                foreach (Bucket b in this) b.UsedCount = 0;
            }

            public IEnumerator<Bucket> GetEnumerator() => new Iterator(buckets, latestBucketWithUsedIndex);
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            private class Iterator : IEnumerator<Bucket>
            {
                private int currentUsedIndex = -1;
                private int latestUsedIndex = -1;
                private Bucket[] entries;

                public Iterator(Bucket[] entries, int latestUsedIndex)
                {
                    this.entries = entries;
                    this.latestUsedIndex = latestUsedIndex;
                    this.currentUsedIndex = latestUsedIndex;
                }

                public Bucket Current { get; private set; }
                object IEnumerator.Current => Current;

                public void Dispose() { }

                public bool MoveNext()
                {
                    if (currentUsedIndex != -1)
                    {
                        Current = entries[latestUsedIndex];
                        currentUsedIndex = Current.PreviousUsedBucketIndex;
                        return true;
                    }
                    else return false;
                }

                public void Reset() => currentUsedIndex = latestUsedIndex;
            }

            public bool ReInit(int newSize)
            {
                if (newSize <= buckets.Length) return false;
                buckets = new Bucket[newSize];
                for (int i = -1; ++i < newSize;) buckets[i] = new Bucket();
                return true;
            }

            public int Count => buckets.Length;
            private EntryCollection ec;

            public BucketCollection(EntryCollection ec) : this(ec, ec.Count) { }

            public BucketCollection(EntryCollection ec, int capacity)
            {
                if (!ec.Marry())
                    throw new Exception("The passed EntryCollection is already married to another BucketCollection");

                this.ec = ec;
                buckets = new Bucket[capacity];
                for (int i = -1; ++i < capacity;) buckets[i] = new Bucket();
            }

            private int latestBucketWithUsedIndex = -1;
            // private int oldestBucketWithUsedIndex = -1;
            Bucket[] buckets;

            public Bucket this[int index]
            {
                get => buckets[index];
                set => buckets[index] = value;
            }

            public void UnregisterEntry(int entryIndex, int bucketIndex)
            {
                bool newlyDeactivatedBucket;
                Bucket b = buckets[bucketIndex];
                b.UnregisterEntry(ec, entryIndex, out newlyDeactivatedBucket);

                if (newlyDeactivatedBucket)
                {
                    if (b.PreviousUsedBucketIndex != -1)
                        buckets[b.PreviousUsedBucketIndex].NextUsedBucketIndex = b.NextUsedBucketIndex;
                    if (b.NextUsedBucketIndex != -1)
                        buckets[b.NextUsedBucketIndex].PreviousUsedBucketIndex = b.PreviousUsedBucketIndex;
                }
            }

            public void RegisterEntry(int entryIndex, int bucketIndex, bool overrideRegistry)
            {
                Bucket b = buckets[bucketIndex];
                bool newlyActivatedBucket;
                b.RegisterEntry(ec, entryIndex, overrideRegistry, out newlyActivatedBucket);

                if (newlyActivatedBucket)
                {
                    if (latestBucketWithUsedIndex != -1)
                        buckets[latestBucketWithUsedIndex].NextUsedBucketIndex = bucketIndex;

                    b.PreviousUsedBucketIndex = latestBucketWithUsedIndex;
                    latestBucketWithUsedIndex = bucketIndex;
                }
            }
        }

        class Iterator : IEnumerator<KeyValuePair<TKey, TValue>>, IDictionaryEnumerator
        {
            private IEnumerator<Entry> iterator;
            private KeyValuePair<TKey, TValue> iteration;
            public Iterator(IEnumerator<Entry> iterator) => this.iterator = iterator;
            public KeyValuePair<TKey, TValue> Current => iteration;
            public DictionaryEntry Entry => new DictionaryEntry(iteration.Key, iteration.Value);
            public object Key => iteration.Key;
            public object Value => iteration.Value;

            object IEnumerator.Current => iteration;
            public void Dispose() { }

            public bool MoveNext()
            {
                if (iterator.MoveNext())
                {
                    Entry e = iterator.Current;
                    iteration = new KeyValuePair<TKey, TValue>(e.Key, e.Value);
                    return true;
                }
                else return false;
            }

            public void Reset() => iterator.Reset();
        }

        private class KeyCollection : ICollection<TKey>, ICollection
        {
            private HashMap<TKey, TValue> map;
            public KeyCollection(HashMap<TKey, TValue> map) => this.map = map;

            public int Count => map.Count;
            public bool Contains(TKey item) => map.ContainsKey(item);
            public bool IsReadOnly => true;
            public bool IsSynchronized => false;
            public object SyncRoot => null;
            public bool Remove(TKey item) => throw new NotImplementedException();
            public void Add(TKey item) => throw new NotImplementedException();
            public void Clear() => throw new NotImplementedException();

            public void CopyTo(TKey[] array, int arrayIndex)
            {
                if (arrayIndex < 0) throw new Exception("Negative array index");
                else if (arrayIndex >= array.Length) throw new Exception("Provided index larger than max index");
                else if (arrayIndex + Count > array.Length) throw new Exception("Too few number of entries from index");

                foreach (KeyValuePair<TKey, TValue> e in map) array[arrayIndex++] = e.Key;
            }

            public void CopyTo(Array array, int index)
            {
                if (index < 0) throw new Exception("Negative array index");
                else if (index >= array.Length) throw new Exception("Provided index larger than max index");
                else if (index + Count > array.Length) throw new Exception("Too few number of entries from index");

                if (array is TKey[] keys) CopyTo(keys, index);
                else if (array is object[] objects) foreach (KeyValuePair<TKey, TValue> kvp in map) objects[index++] = kvp.Key;
                else throw new Exception("Array type mismatch");
            }

            public class Iterator : IEnumerator<TKey>
            {
                private IEnumerator<KeyValuePair<TKey, TValue>> iterator;
                public Iterator(IEnumerator<KeyValuePair<TKey, TValue>> iterator) => this.iterator = iterator;

                public TKey Current { get; private set; }
                object IEnumerator.Current => Current;

                public void Dispose() { }

                public bool MoveNext()
                {
                    if (iterator.MoveNext())
                    {
                        Current = iterator.Current.Key;
                        return true;
                    }
                    else return false;
                }

                public void Reset() => iterator.Reset();
            }

            public IEnumerator<TKey> GetEnumerator() => new Iterator(map.GetEnumerator());
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private class ValueCollection : ICollection<TValue>, ICollection
        {
            private HashMap<TKey, TValue> map;
            public ValueCollection(HashMap<TKey, TValue> map) => this.map = map;

            public int Count => map.Count;
            public bool Contains(TValue item) => map.ContainsValue(item);
            public bool IsReadOnly => true;
            public bool IsSynchronized => false;
            public object SyncRoot => null;
            public bool Remove(TValue item) => throw new NotImplementedException();
            public void Add(TValue item) => throw new NotImplementedException();
            public void Clear() => throw new NotImplementedException();

            public void CopyTo(TValue[] array, int arrayIndex)
            {
                if (arrayIndex < 0) throw new Exception("Negative array index");
                else if (arrayIndex >= array.Length) throw new Exception("Provided index larger than max index");
                else if (arrayIndex + Count > array.Length) throw new Exception("Too few number of entries from index");

                foreach (KeyValuePair<TKey, TValue> kvp in map) array[arrayIndex++] = kvp.Value;
            }

            public void CopyTo(Array array, int index)
            {
                if (index < 0) throw new Exception("Negative array index");
                else if (index >= array.Length) throw new Exception("Provided index larger than max index");
                else if (index + Count > array.Length) throw new Exception("Too few number of entries from index");

                if (array is TKey[] keys) CopyTo(keys, index);
                else if (array is object[] objects) foreach (KeyValuePair<TKey, TValue> kvp in map) objects[index++] = kvp.Value;
                else throw new Exception("Array type mismatch");
            }

            public class Iterator : IEnumerator<TValue>
            {
                private IEnumerator<KeyValuePair<TKey, TValue>> iterator;
                public Iterator(IEnumerator<KeyValuePair<TKey, TValue>> iterator) => this.iterator = iterator;

                public TValue Current { get; private set; }
                object IEnumerator.Current => Current;

                public void Dispose() { }

                public bool MoveNext()
                {
                    if (iterator.MoveNext())
                    {
                        Current = iterator.Current.Value;
                        return true;
                    }
                    else return false;
                }

                public void Reset() => iterator.Reset();
            }

            public IEnumerator<TValue> GetEnumerator() => new Iterator(map.GetEnumerator());
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }

    public partial class HashMap<TKey, TValue> : IDictionary<TKey, TValue>, IDictionary/*, IEnumerable<KeyValuePair<TKey, TValue>>*/
    {
        // HashMap management implementation below

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => new Iterator(ec.GetEnumerator());
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        EntryCollection ec;
        BucketCollection bc;

        public HashMap()
        {
            ec = new EntryCollection(3);
            bc = new BucketCollection(ec);
        }

        ICollection IDictionary.Keys => new KeyCollection(this);
        ICollection IDictionary.Values => new ValueCollection(this);
        public ICollection<TKey> Keys => new KeyCollection(this);
        public ICollection<TValue> Values => new ValueCollection(this);

        public bool IsReadOnly => false;
        public bool IsFixedSize => false;
        public bool IsSynchronized => true;
        private object syncRoot = new object();
        public object SyncRoot => syncRoot;

        private IEquComp<TKey> comparer = new DefaultHasher<TKey>();
        public int Count => ec.UsedCount;

        public object this[object key]
        {
            get
            {
                if (key is TKey k) return this[k];
                else throw new Exception("Invalid key type");
            }
            set
            {
                if (key is TKey k)
                {
                    if (value is TValue v) this[k] = v;
                    else throw new Exception("Invalid value type");
                }
                else throw new Exception("Invalid key type");
            }
        }

        public TValue this[TKey key]
        {
            get => Get(key);
            set => Update(key, value);
        }

        private ulong HashOf(TKey key) => comparer.GetHashCode(key);
        private int GetBucket(ulong hashCode) => (int)(hashCode % (ulong)bc.Count);
        private int GetBucket(TKey key) => (int)(HashOf(key) % (ulong)bc.Count);
        private int FindUsed(TKey key) => FindUsed(GetBucket(HashOf(key)), key);

        private int FindUsed(int bucket, TKey key)
        {
            for (int i = bc[bucket].LatestUsedEntryIndex; i != -1; i = ec[i].PreviousUsedIndexInBucket)
            {
                if (HashOf(key) == ec[i].HashCode && comparer.Equals(ec[i].Key, key)) return i;
            }

            return -1;
        }

        public bool ContainsKey(TKey key)
        {
            lock (syncRoot) return FindUsed(key) != -1;
        }
        private bool ContainsKey(TKey key, out int entryIndex) => (entryIndex = FindUsed(key)) != -1;

        public bool ContainsValue(TValue value)
        {
            foreach (KeyValuePair<TKey, TValue> kvp in this) if (kvp.Value.Equals(value)) return true;
            return false;
        }

        private void Redistribute(bool reHash)
        {
            foreach (Entry e in ec)
            {
                if (reHash) e.HashCode = HashOf(e.Key);
                bc.RegisterEntry(e.SelfIndex, GetBucket(e.HashCode), true);
            }

            return;
        }

        private void Resize()
        {
            int newSize = HashCore.ExpandPrime(bc.Count);
            ec.Resize(newSize);
            bc.ReInit(newSize);
            Redistribute(comparer.UpdateHashesDueToTablePrimeChange(newSize));
        }

        public void Remove(TKey key)
        {
            lock (syncRoot)
            {
                int entryIndex;
                int bucketIndex = GetBucket(key);
                if (!ContainsKey(key, out entryIndex)) throw new Exception("Key not found");
                ec.RenderEntryUnused(entryIndex);
                bc.UnregisterEntry(entryIndex, bucketIndex);
            }
        }

        public void Clear()
        {
            lock (syncRoot)
            {
                ec.Clear();
                bc.Clear();
            }
        }

        public void Add(TKey key, TValue value)
        {
            lock (syncRoot)
            {
                if (ContainsKey(key)) throw new Exception("Key already exists");
                ulong hashCode = HashOf(key);
                int bucketIndex = GetBucket(hashCode);
                int entryIndex = ec.PlaceEntry(key, hashCode, value);

                if (entryIndex == -1)
                {
                    Resize();
                    hashCode = HashOf(key);
                    bucketIndex = GetBucket(hashCode);
                    entryIndex = ec.PlaceEntry(key, hashCode, value);
                }

                bc.RegisterEntry(entryIndex, bucketIndex, false);
            }
        }

        public ref TValue GetAlias(TKey key)
        {
            lock (syncRoot)
            {
                int entryIndex;
                if (!ContainsKey(key, out entryIndex)) throw new Exception("Key does not exist");
                return ref ec[entryIndex].Value;
            }
        }

        public TValue Get(TKey key) => GetAlias(key);


        public void Replace(TKey key, TValue value)
        {
            lock (syncRoot)
            {
                int entryIndex;
                if (!ContainsKey(key, out entryIndex)) throw new Exception("Key not found");
                ec.UpdateEntryValue(entryIndex, value);
            }
        }

        public void Update(TKey key, TValue value)
        {
            lock (syncRoot)
            {
                if (ContainsKey(key)) Replace(key, value);
                else Add(key, value);
            }
        }

        bool IDictionary<TKey, TValue>.Remove(TKey key)
        {
            try
            {
                Remove(key);
                return true;
            }
            catch (Exception) { return false; }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            value = default(TValue);

            try
            {
                value = Get(key);
                return true;
            }
            catch (Exception) { return false; }
        }

        public bool TryAdd(KeyValuePair<TKey, TValue> item) => TryAdd(item.Key, item.Value);

        public bool TryAdd(TKey key, TValue value)
        {
            try
            {
                Add(key, value);
                return true;
            }
            catch (Exception) { return false; }
        }

        public void Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            lock (syncRoot)
            {
                int entryIndex;
                if (!ContainsKey(item.Key, out entryIndex)) return false;
                if (!ec[entryIndex].Value.Equals(item.Value)) return false;
                return true;
            }
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            lock (syncRoot)
            {
                if (arrayIndex < 0) throw new Exception("Negative array index");
                else if (arrayIndex >= array.Length) throw new Exception("Provided index larger than max index");
                else if (arrayIndex + Count > array.Length) throw new Exception("Too few number of entries from index");

                foreach (KeyValuePair<TKey, TValue> kvp in this) array[arrayIndex++] = kvp;
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (!Contains(item)) return false;
            Remove(item.Key);
            return true;
        }

        public bool TryRemove(TKey key)
        {
            if (!ContainsKey(key)) return false;
            Remove(key);
            return true;
        }

        public void Add(object key, object value)
        {
            if (key is TKey k)
            {
                if (value is TValue v) Add(k, v);
                else throw new Exception("Invalid value type");
            }
            else throw new Exception("Invalid key type");
        }

        public bool Contains(object key)
        {
            if (key is TKey k) return Contains(k);
            else throw new Exception("Invalid key type");
        }

        IDictionaryEnumerator IDictionary.GetEnumerator() => new Iterator(ec.GetEnumerator());

        public void Remove(object key)
        {
            if (key is TKey k) Remove(k);
            else throw new Exception("Invalid key type");
        }

        public void CopyTo(Array array, int index)
        {
            lock (syncRoot)
            {
                if (index < 0) throw new Exception("Negative array index");
                else if (index >= array.Length) throw new Exception("Provided index larger than max index");
                else if (index + Count > array.Length) throw new Exception("Too few number of entries from index");

                if (array is KeyValuePair<TKey, TValue>[] pairs) CopyTo(pairs, index);
                else if (array is DictionaryEntry[] dictEntryArray)
                {
                    foreach (KeyValuePair<TKey, TValue> kvp in this) dictEntryArray[index++] = new DictionaryEntry(kvp.Key, kvp.Value);
                }
                else
                {
                    if (array is object[] objects)
                    {
                        foreach (KeyValuePair<TKey, TValue> kvp in this) objects[index++] = new KeyValuePair<TKey, TValue>(kvp.Key, kvp.Value);
                    }
                    else throw new Exception("Invalid array type");
                }
            }
        }
    }
}
