namespace PackSite.Library.Pipelining.Configuration
{
    /// <summary>
    /// Pipeline definition.
    /// </summary>
    public sealed class PipelineDefinition
    {
        /// <summary>
        /// Whether pipeline is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Args type.
        /// </summary>
        public string? ArgsType { get; set; }

        /// <summary>
        /// Whether to use default name instead of name defined in <see cref="PipeliningConfiguration.Pipelines"/> keys.
        /// </summary>
        public bool UseDefaultName { get; set; }

        /// <summary>
        /// Pipeline description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Pipeline lifetime.
        /// </summary>
        public InvokablePipelineLifetime Lifetime { get; set; }

        /// <summary>
        /// Pipeline steps.
        /// </summary>
        public List<string?>? Steps { get; set; }

        /// <summary>
        /// Sets args type.
        /// </summary>
        /// <param name="type"></param>
        public void SetArgsType(Type type)
        {
            ArgumentNullException.ThrowIfNull(type);

            ArgsType = type?.AssemblyQualifiedName;
        }

        /// <summary>
        /// Adds steps from types.
        /// </summary>
        /// <param name="steps"></param>
        public void AddSteps(IEnumerable<Type> steps)
        {
            ArgumentNullException.ThrowIfNull(steps);

            Steps ??= [];
            Steps.AddRange(steps.Select(x => x.AssemblyQualifiedName));
        }

        /// <summary>
        /// Adds steps from types.
        /// </summary>
        /// <param name="steps"></param>
        public void AddSteps(params Type[] steps)
        {
            ArgumentNullException.ThrowIfNull(steps);

            Steps ??= [];
            Steps.AddRange(steps.Select(x => x.AssemblyQualifiedName));
        }
    }
}
