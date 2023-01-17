using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinknose.Utilities
{
    public class ExtendedList<T> : ICollection<T>, IEnumerable<T>, IList<T>, IReadOnlyCollection<T>, IReadOnlyList<T>, IList
    {
        public event EventHandler<ItemAddedEventArgs<T>> ItemAdded;

        private List<T> _list = new();

        public int Count => _list.Count;

        public bool IsReadOnly => false;

        public bool IsFixedSize => ((IList)_list).IsFixedSize;

        public object SyncRoot => ((ICollection)_list).SyncRoot;

        public bool IsSynchronized => ((ICollection)_list).IsSynchronized;

        object IList.this[int index] { get => ((IList)_list)[index]; set => ((IList)_list)[index] = value; }
        public T this[int index] { get => ((IList<T>)_list)[index]; set => ((IList<T>)_list)[index] = value; }

        public void Add(T item)
        {
            _list.Add(item);

            ItemAdded?.Invoke(this, new ItemAddedEventArgs<T>(item));
        }

        public void AddRange(IEnumerable<T> items)
        {
            _list.AddRange(items);

            foreach (var item in items)
            {
                ItemAdded?.Invoke(this, new ItemAddedEventArgs<T>(item));
            }
        }

        public void Clear() => _list.Clear();
        
        public bool Contains(T item) => _list.Contains(item);
        
        public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);
        
        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();
        
        public bool Remove(T item) => _list.Remove(item);
        
        IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public int Add(object value)
        {
            return ((IList)_list).Add(value);
        }

        public bool Contains(object value)
        {
            return ((IList)_list).Contains(value);
        }

        public int IndexOf(object value)
        {
            return ((IList)_list).IndexOf(value);
        }

        public void Insert(int index, object value)
        {
            ((IList)_list).Insert(index, value);
        }

        public void Remove(object value)
        {
            ((IList)_list).Remove(value);
        }

        public void CopyTo(Array array, int index)
        {
            ((ICollection)_list).CopyTo(array, index);
        }
    }
}
