namespace PackSite.Library.Pipelining.Internal
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A collection of steps.
    /// </summary>
    public interface IStepCollection : IList<Type>
    {
        /// <summary>
        /// Step instances.
        /// </summary>
        IReadOnlyList<object> StepInstances { get; }

        /// <summary>
        /// Step types.
        /// </summary>
        IReadOnlyList<Type> StepTypes { get; }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first
        /// occurrence within the range of elements in the System.Collections.Generic.List`1
        /// that extends from the specified index to the last element.
        /// </summary>
        /// <param name="stepType"></param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <returns>
        /// The zero-based index of the first occurrence of item within the range of elements
        /// in the System.Collections.Generic.List`1 that extends from index to the last
        /// element, if found; otherwise, -1.
        /// </returns>
        int IndexOf(Type stepType, int index);

        /// <summary>
        /// Adds an item to the System.Collections.Generic.ICollection`1
        /// </summary>
        /// <param name="instance">The object to add to the System.Collections.Generic.ICollection`1.</param>
        void Add(IBaseStep instance);

        /// <summary>
        /// Inserts an item to the System.Collections.Generic.IList`1 at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="instance">The object to insert into the System.Collections.Generic.IList`1.</param>
        void Insert(int index, IBaseStep instance);
    }
}