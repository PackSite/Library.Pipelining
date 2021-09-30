namespace PackSite.Library.Pipelining.Internal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using PackSite.Library.Pipelining;

    internal sealed partial class PipelineBuilder<TArgs>
    {
        /// <inheritdoc/>
        public int IndexOf(Type stepType)
        {
            return _stepTypes.IndexOf(stepType);
        }

        /// <inheritdoc/>
        public void Insert(int index, Type stepType)
        {
            _ = stepType ?? throw new ArgumentNullException(nameof(stepType));

            Type[] stepInterfaces = stepType.GetInterfaces();

            if (!stepInterfaces.Contains(typeof(IStep)) && !stepInterfaces.Contains(typeof(IStep<TArgs>)))
            {
                throw new ArgumentException(nameof(TArgs), $"Invalid step instance type. Must implement '{typeof(IStep).FullName}' or '{typeof(IStep<TArgs>).FullName}'.");
            }

            _steps.Insert(index, stepType);
            _stepTypes.Insert(index, stepType);
        }

        /// <inheritdoc/>
        public void RemoveAt(int index)
        {
            _steps.RemoveAt(index);
            _stepTypes.RemoveAt(index);
        }

        /// <inheritdoc/>
        public void Add(Type stepType)
        {
            _ = stepType ?? throw new ArgumentNullException(nameof(stepType));

            Type[] stepInterfaces = stepType.GetInterfaces();

            if (!stepInterfaces.Contains(typeof(IStep)) && !stepInterfaces.Contains(typeof(IStep<TArgs>)))
            {
                throw new ArgumentException(nameof(TArgs), $"Invalid step instance type. Must implement '{typeof(IStep).FullName}' or '{typeof(IStep<TArgs>).FullName}'.");
            }

            _steps.Add(stepType);
            _stepTypes.Add(stepType);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            _steps.Clear();
            _stepTypes.Clear();
        }

        /// <inheritdoc/>
        public bool Contains(Type stepType)
        {
            return _stepTypes.Contains(stepType);
        }

        /// <inheritdoc/>
        public void CopyTo(Type[] array, int arrayIndex)
        {
            _stepTypes.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        public bool Remove(Type stepType)
        {
            var index = IndexOf(stepType);

            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public IEnumerator<Type> GetEnumerator()
        {
            return _stepTypes.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _stepTypes.GetEnumerator();
        }
    }
}
