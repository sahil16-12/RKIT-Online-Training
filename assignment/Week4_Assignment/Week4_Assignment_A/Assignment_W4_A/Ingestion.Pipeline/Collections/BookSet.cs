using Assignment_W4_A.Ingestion.Pipeline.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_W4_A.Ingestion.Pipeline.Collections
{
    /// <summary>
    /// Small generic collection for storing unique books with optional capacity limit.
    /// Prevents duplicates by ISBN.
    /// </summary>
    public class BookSet<T> : IEnumerable<T> where T : Book
    {
        private readonly List<T> _items = new List<T>();

        /// <summary>Optional capacity that limits number of items.</summary>
        public int? Capacity { get; }

        /// <summary>Creates a new BookSet with optional capacity.</summary>
        public BookSet(int? capacity)
        {
            Capacity = capacity;
        }

        /// <summary>Adds an item to the collection if it passes validation rules.</summary>
        public bool Add(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (Capacity.HasValue && _items.Count >= Capacity.Value)
            {
                return false;
            }

            bool exists = _items.Any(x => x.ISBN == item.ISBN);
            if (exists)
            {
                return false;
            }

            _items.Add(item);
            return true;
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}
