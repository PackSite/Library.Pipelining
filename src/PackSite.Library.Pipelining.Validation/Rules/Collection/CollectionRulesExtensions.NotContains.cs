namespace PackSite.Library.Pipelining.Validation.Validators
{
    using System;
    using PackSite.Library.Pipelining.Validation.Rules.Collection;

    /// <summary>
    /// Collection validator extensions.
    /// </summary>
    public static partial class CollectionRulesExtensions
    {
        /// <summary>
        /// Verifies whether pipelines collection has a specific pipeline instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="instance"></param>
        public static NotContainsPipelineInstanceRule NotContainsPipeline(this ValidatorContext context, IPipeline instance)
        {
            NotContainsPipelineInstanceRule validator = new(instance);

            return validator;
        }

        /// <summary>
        /// Verifies whether pipelines collection has a pipeline with specific name.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        public static NotContainsPipelineWithNameRule NotContainsPipeline(this ValidatorContext context, PipelineName name)
        {
            NotContainsPipelineWithNameRule validator = new(name);

            return validator;
        }

        /// <summary>
        /// Verifies whether pipelines collection has a pipeline with specific argument type.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="argumentType"></param>
        public static NotContainsPipelineWithArgumentTypeRule NotContainsPipeline(this ValidatorContext context, Type argumentType)
        {
            NotContainsPipelineWithArgumentTypeRule validator = new(argumentType);

            return validator;
        }

        /// <summary>
        /// Verifies whether pipelines collection has a pipeline with specific argument type.
        /// </summary>
        /// <param name="context"></param>
        /// <typeparam name="TArg"></typeparam>
        public static NotContainsPipelineWithArgumentTypeRule NotContainsPipeline<TArg>(this ValidatorContext context)
            where TArg : class
        {
            NotContainsPipelineWithArgumentTypeRule validator = new(typeof(TArg));

            return validator;
        }
    }
}
