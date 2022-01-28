namespace PackSite.Library.Pipelining.Validation.Internal.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// <see cref="IList{T}"/> extensions.
    /// </summary>
    internal static class IListExtensions
    {
        /// <summary>
        /// Add range of items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="items"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            _ = list ?? throw new ArgumentNullException(nameof(list));
            _ = items ?? throw new ArgumentNullException(nameof(items));

            if (list is List<T> asList)
            {
                asList.AddRange(items);
            }
            else
            {
                foreach (var item in items)
                {
                    list.Add(item);
                }
            }
        }

        /// <summary>
        /// Converts <paramref name="collection"/> to <see cref="IReadOnlyList{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IReadOnlyList<T> AsReadOnly<T>(this IList<T> collection)
        {
            _ = collection ?? throw new ArgumentNullException(nameof(collection));

            // Type-sniff, no need to create a wrapper when collection
            // is an IReadOnlyList<T> *already*.
            IReadOnlyList<T>? list = collection as IReadOnlyList<T>;

            if (list is not null)
            {
                return list;
            }

            return new ReadOnlyWrapper<T>(collection);
        }

        private sealed class ReadOnlyWrapper<T> : IReadOnlyList<T>
        {
            private readonly IList<T> _list;

            /// <inheritdoc/>
            public int Count => _list.Count;

            /// <inheritdoc/>
            public T this[int index] => _list[index];

            public ReadOnlyWrapper(IList<T> list)
            {
                _list = list;
            }

            /// <inheritdoc/>
            public IEnumerator<T> GetEnumerator()
            {
                return _list.GetEnumerator();
            }

            /// <inheritdoc/>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
