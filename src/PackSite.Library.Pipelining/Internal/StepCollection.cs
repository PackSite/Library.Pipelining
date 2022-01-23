namespace PackSite.Library.Pipelining.Internal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class StepCollection<TArgs> : IStepCollection
        where TArgs : class
    {
        private readonly List<object> _steps = new();
        private readonly List<Type> _stepTypes = new();

        /// <inheritdoc/>
        public int Count => _steps.Count;

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <inheritdoc/>
        public IReadOnlyList<object> StepInstances => _steps;

        /// <inheritdoc/>
        public IReadOnlyList<Type> StepTypes => _stepTypes;

        /// <inheritdoc/>
        public Type this[int index]
        {
            get => _stepTypes[index];
            set => Insert(index, value);
        }

        /// <inheritdoc/>
        public int IndexOf(Type stepType)
        {
            return _stepTypes.IndexOf(stepType);
        }

        /// <inheritdoc/>
        public int IndexOf(Type stepType, int index)
        {
            return _stepTypes.IndexOf(stepType, index);
        }

        /// <inheritdoc/>
        public void Insert(int index, Type stepType)
        {
            _ = stepType ?? throw new ArgumentNullException(nameof(stepType));

            Type[] stepInterfaces = stepType.GetInterfaces();

            if (!stepInterfaces.Contains(typeof(IStep)) && !stepInterfaces.Contains(typeof(IStep<TArgs>)))
            {
                throw new ArgumentException(nameof(TArgs), $"Invalid step instance type '{stepType}'. Must implement '{typeof(IStep).FullName}' or '{typeof(IStep<TArgs>).FullName}'.");
            }

            _steps.Insert(index, stepType);
            _stepTypes.Insert(index, stepType);
        }

        /// <inheritdoc/>
        public void Insert(int index, IBaseStep instance)
        {
            _ = instance ?? throw new ArgumentNullException(nameof(instance));

            if (instance is not (IStep or IStep<TArgs>))
            {
                throw new ArgumentException(nameof(TArgs), $"Invalid step instance type '{instance.GetType()}'. Must implement '{typeof(IStep).FullName}' or '{typeof(IStep<TArgs>).FullName}'.");
            }

            _steps.Insert(index, instance);
            _stepTypes.Insert(index, instance.GetType());
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
                throw new ArgumentException(nameof(TArgs), $"Invalid step instance type '{stepType}'. Must implement '{typeof(IStep).FullName}' or '{typeof(IStep<TArgs>).FullName}'.");
            }

            _steps.Add(stepType);
            _stepTypes.Add(stepType);
        }

        /// <inheritdoc/>
        public void Add(IBaseStep instance)
        {
            _ = instance ?? throw new ArgumentNullException(nameof(instance));

            if (instance is not (IStep or IStep<TArgs>))
            {
                throw new ArgumentException($"Invalid step instance type '{instance.GetType()}'. Must implement '{typeof(IStep).FullName}' or '{typeof(IStep<TArgs>).FullName}'.", nameof(instance));
            }

            _steps.Add(instance);
            _stepTypes.Add(instance.GetType());
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
            int index = IndexOf(stepType);

            if (index >= 0)
            {
                RemoveAt(index);
            }
            else
            {
                return false;
            }

            while (true)
            {
                index = IndexOf(stepType);

                if (index >= 0)
                {
                    RemoveAt(index);
                }
                else
                {
                    return true;
                }
            }
        }

        /// <inheritdoc/>
        public IEnumerator<Type> GetEnumerator()
        {
            return _stepTypes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _stepTypes.GetEnumerator();
        }
    }
}
