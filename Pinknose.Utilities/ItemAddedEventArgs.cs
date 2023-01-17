using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinknose.Utilities
{
    public class ItemAddedEventArgs<T> : EventArgs
    {
        public ItemAddedEventArgs(T item)
        {
            Item = item;
        }

        public T Item { get; private set; }
    }
}
