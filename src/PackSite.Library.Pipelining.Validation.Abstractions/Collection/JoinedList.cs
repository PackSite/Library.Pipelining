namespace PackSite.Library.Pipelining.Validation.Collection
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Sub list for a list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SubList<T> : IList<T>
    {
        private readonly List<T> _subList = new();
        private readonly IList<T> _mainList;

        /// <inheritdoc/>
        public T this[int index]
        {
            get => _subList[index];
            set
            {
                int mainIndex = _mainList.IndexOf(_subList[index]);

                _subList[index] = value;

                if (mainIndex >= 0)
                {
                    _mainList[mainIndex] = value;
                }
            }
        }

        /// <inheritdoc/>
        public int Count => _subList.Count;

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <summary>
        /// Initializes a new instance of <see cref="SubList{T}"/>.
        /// </summary>
        /// <param name="mainList"></param>
        public SubList(IList<T> mainList)
        {
            _mainList = mainList;
        }

        /// <inheritdoc/>
        public void Add(T item)
        {
            _subList.Add(item);
            _mainList.Add(item);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            while (_subList.Count > 0)
            {
                RemoveAt(0);
            }
        }

        /// <inheritdoc/>
        public bool Contains(T item)
        {
            return _subList.Contains(item);
        }

        /// <inheritdoc/>
        public void CopyTo(T[] array, int arrayIndex)
        {
            _subList.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        public int IndexOf(T item)
        {
            return _subList.IndexOf(item);
        }

        /// <inheritdoc/>
        public void Insert(int index, T item)
        {
            _subList.Insert(index, item);
            _mainList.Add(item);
        }

        /// <inheritdoc/>
        public bool Remove(T item)
        {
            return _subList.Remove(item) || _mainList.Remove(item);
        }

        /// <inheritdoc/>
        public void RemoveAt(int index)
        {
            T item = _subList[index];

            _subList.RemoveAt(index);
            _mainList.Remove(item);
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            return _subList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _subList.GetEnumerator();
        }
    }
}
