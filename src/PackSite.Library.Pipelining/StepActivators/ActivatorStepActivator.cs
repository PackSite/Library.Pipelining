namespace PackSite.Library.Pipelining.StepActivators
{
    using System;

    /// <summary>
    /// <see cref="Activator"/> based step activator.
    /// </summary>
    public sealed class ActivatorStepActivator : IStepActivator
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ServicesStepActivator"/>
        /// </summary>
        public ActivatorStepActivator()
        {

        }

        /// <inheritdoc/>
        public IBaseStep Create(Type stepType)
        {
            return Activator.CreateInstance(stepType) as IBaseStep ??
                throw new InvalidOperationException($"Failed to activate '{stepType.FullName ?? stepType.Name}'");
        }
    }
}
