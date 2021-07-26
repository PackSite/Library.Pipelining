namespace PackSite.Library.Pipelining
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Pipeline.
    /// </summary>
    public interface IPipeline
    {
        /// <summary>
        /// Pipeline lifetime.
        /// </summary>
        InvokablePipelineLifetime Lifetime { get; }

        /// <summary>
        /// Pipeline name.
        /// </summary>
        PipelineName Name { get; }

        /// <summary>
        /// Pipeline description.
        /// </summary>
        string? Description { get; }

        /// <summary>
        /// Steps.
        /// </summary>
        IReadOnlyList<Type> Steps { get; }
    }

    /// <summary>
    /// Pipeline.
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface IPipeline<TContext> : IPipeline
        where TContext : class
    {
        /// <summary>
        /// Creates an invokable pipeline.
        /// </summary>
        /// <param name="stepActivator"></param>
        /// <returns></returns>
        IInvokablePipeline<TContext> CreateInvokable(IStepActivator stepActivator);
    }
}