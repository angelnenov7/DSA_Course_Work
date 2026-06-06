using System;

namespace LogicInterpreter.DataStructures
{
    /// <summary>
    /// Custom implementation of a dynamic resizable array
    /// </summary>
    public class DynamicArray<T>
    {
        private T[] items;
        private int count;
        private const int DefaultCapacity = 4;

        public DynamicArray()
        {
            items = new T[DefaultCapacity];
            count = 0;
        }

        public DynamicArray(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentException("Capacity cannot be negative");
            items = new T[capacity];
            count = 0;
        }

        public int Count => count;
        public int Capacity => items.Length;

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= count)
                    throw new IndexOutOfRangeException();
                return items[index];
            }
            set
            {
                if (index < 0 || index >= count)
                    throw new IndexOutOfRangeException();
                items[index] = value;
            }
        }

        public void Add(T item)
        {
            if (count == items.Length)
                Resize();
            items[count++] = item;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= count)
                throw new IndexOutOfRangeException();

            // Shift elements left
            for (int i = index; i < count - 1; i++)
            {
                items[i] = items[i + 1];
            }
            items[--count] = default(T)!;
        }

        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }
            return false;
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < count; i++)
            {
                if (items[i] != null && items[i]!.Equals(item))
                    return i;
                if (items[i] == null && item == null)
                    return i;
            }
            return -1;
        }

        public bool Contains(T item)
        {
            return IndexOf(item) >= 0;
        }

        public void Clear()
        {
            for (int i = 0; i < count; i++)
            {
                items[i] = default(T)!;
            }
            count = 0;
        }

        private void Resize()
        {
            int newCapacity = items.Length * 2;
            T[] newArray = new T[newCapacity];
            for (int i = 0; i < count; i++)
            {
                newArray[i] = items[i];
            }
            items = newArray;
        }

        public T[] ToArray()
        {
            T[] result = new T[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = items[i];
            }
            return result;
        }
    }
}
