using System;

namespace LogicInterpreter.DataStructures
{
    /// <summary>
    /// Custom implementation of a HashTable with separate chaining
    /// </summary>
    public class HashTable<TKey, TValue> where TKey : notnull
    {
        private class Entry
        {
            public TKey Key { get; set; }
            public TValue Value { get; set; }
            public Entry? Next { get; set; }

            public Entry(TKey key, TValue value)
            {
                Key = key;
                Value = value;
                Next = null;
            }
        }

        private Entry?[] buckets;
        private int count;
        private const int DefaultCapacity = 16;
        private const double LoadFactorThreshold = 0.75;

        public HashTable() : this(DefaultCapacity) { }

        public HashTable(int capacity)
        {
            if (capacity < 1)
                capacity = DefaultCapacity;
            buckets = new Entry?[capacity];
            count = 0;
        }

        public int Count => count;

        private int GetBucketIndex(TKey key)
        {
            int hash = key.GetHashCode() & 0x7fffffff;
            return hash % buckets.Length;
        }

        public void Add(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (ContainsKey(key))
                throw new ArgumentException("Key already exists");

            int index = GetBucketIndex(key);
            Entry newEntry = new Entry(key, value);
            newEntry.Next = buckets[index];
            buckets[index] = newEntry;
            count++;

            // Resize if load factor exceeds threshold
            if ((double)count / buckets.Length > LoadFactorThreshold)
                Resize();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            int index = GetBucketIndex(key);
            Entry? current = buckets[index];

            while (current != null)
            {
                if (current.Key.Equals(key))
                {
                    value = current.Value;
                    return true;
                }
                current = current.Next;
            }

            value = default(TValue)!;
            return false;
        }

        public TValue Get(TKey key)
        {
            if (TryGetValue(key, out TValue value))
                return value;
            throw new ArgumentException("Key not found");
        }

        public void Set(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            int index = GetBucketIndex(key);
            Entry? current = buckets[index];

            while (current != null)
            {
                if (current.Key.Equals(key))
                {
                    current.Value = value;
                    return;
                }
                current = current.Next;
            }

            throw new ArgumentException("Key not found");
        }

        public bool ContainsKey(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            int index = GetBucketIndex(key);
            Entry? current = buckets[index];

            while (current != null)
            {
                if (current.Key.Equals(key))
                    return true;
                current = current.Next;
            }

            return false;
        }

        public bool Remove(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            int index = GetBucketIndex(key);
            Entry? current = buckets[index];
            Entry? previous = null;

            while (current != null)
            {
                if (current.Key.Equals(key))
                {
                    if (previous == null)
                        buckets[index] = current.Next;
                    else
                        previous.Next = current.Next;
                    
                    count--;
                    return true;
                }
                previous = current;
                current = current.Next;
            }

            return false;
        }

        public DynamicArray<TKey> GetKeys()
        {
            DynamicArray<TKey> keys = new DynamicArray<TKey>();
            for (int i = 0; i < buckets.Length; i++)
            {
                Entry? current = buckets[i];
                while (current != null)
                {
                    keys.Add(current.Key);
                    current = current.Next;
                }
            }
            return keys;
        }

        public DynamicArray<TValue> GetValues()
        {
            DynamicArray<TValue> values = new DynamicArray<TValue>();
            for (int i = 0; i < buckets.Length; i++)
            {
                Entry? current = buckets[i];
                while (current != null)
                {
                    values.Add(current.Value);
                    current = current.Next;
                }
            }
            return values;
        }

        private void Resize()
        {
            int newCapacity = buckets.Length * 2;
            Entry?[] oldBuckets = buckets;
            buckets = new Entry?[newCapacity];
            count = 0;

            // Rehash all entries
            for (int i = 0; i < oldBuckets.Length; i++)
            {
                Entry? current = oldBuckets[i];
                while (current != null)
                {
                    Add(current.Key, current.Value);
                    current = current.Next;
                }
            }
        }

        public void Clear()
        {
            for (int i = 0; i < buckets.Length; i++)
            {
                buckets[i] = null;
            }
            count = 0;
        }
    }
}
