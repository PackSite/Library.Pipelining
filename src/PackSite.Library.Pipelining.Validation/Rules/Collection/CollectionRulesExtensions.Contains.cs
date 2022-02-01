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
        public static ContainsPipelineInstanceRule ContainsPipeline(this ValidatorContext context, IPipeline instance)
        {
            ContainsPipelineInstanceRule validator = new(instance);

            return validator;
        }

        /// <summary>
        /// Verifies whether pipelines collection has a pipeline with specific name.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        public static ContainsPipelineWithNameRule ContainsPipeline(this ValidatorContext context, PipelineName name)
        {
            ContainsPipelineWithNameRule validator = new(name);

            return validator;
        }

        /// <summary>
        /// Verifies whether pipelines collection has a pipeline with specific argument type.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="argumentType"></param>
        public static ContainsPipelineWithArgumentTypeRule ContainsPipeline(this ValidatorContext context, Type argumentType)
        {
            ContainsPipelineWithArgumentTypeRule validator = new(argumentType);

            return validator;
        }

        /// <summary>
        /// Verifies whether pipelines collection has a pipeline with specific argument type.
        /// </summary>
        /// <param name="context"></param>
        /// <typeparam name="TArg"></typeparam>
        public static ContainsPipelineWithArgumentTypeRule ContainsPipeline<TArg>(this ValidatorContext context)
            where TArg : class
        {
            ContainsPipelineWithArgumentTypeRule validator = new(typeof(TArg));

            return validator;
        }
    }
}
