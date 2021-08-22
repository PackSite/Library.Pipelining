namespace PackSite.Library.Pipelining.Internal.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using PackSite.Library.Pipelining.Configuration;

    internal static class PipeliningConfigurationExtensions
    {
        /// <summary>
        /// Builds pipelnies from options.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IReadOnlyList<IPipeline> BuildPipelines(this PipeliningConfiguration options)
        {
            List<IPipeline> pipelines = new();

            if (options.Pipelines is List<PipelineDefinition> pipelinesToBuild)
            {
                foreach (PipelineDefinition pipelineDefinition in pipelinesToBuild)
                {
                    if (pipelineDefinition.Enabled)
                    {
                        IPipeline pipeline = BuildPipeline(pipelineDefinition);
                        pipelines.Add(pipeline);
                    }
                }
            }

            return pipelines;
        }

        private static IPipeline BuildPipeline(PipelineDefinition pipelineDefinition)
        {
            _ = pipelineDefinition.ContextType ?? throw new NullReferenceException($"Context type cannot be null in '{pipelineDefinition.Name}' pipeline.");

            Type contextType = Type.GetType(pipelineDefinition.ContextType, AssemblyResolver, null) ??
                throw new NullReferenceException($"Invalid context type '{pipelineDefinition.ContextType}' in '{pipelineDefinition.Name}' pipeline.");

            IPipelineBuilder builder = PipelineBuilder.Create(contextType)
                .Name(pipelineDefinition.Name ?? string.Empty)
                .Description(pipelineDefinition.Description ?? string.Empty)
                .Lifetime(pipelineDefinition.Lifetime);

            if (pipelineDefinition.Steps is not null)
            {
                foreach (string? step in pipelineDefinition.Steps)
                {
                    _ = step ?? throw new NullReferenceException($"Step type cannot be null in '{pipelineDefinition.Name}' pipeline.");

                    Type stepType = Type.GetType(step, AssemblyResolver, null) ??
                        throw new NullReferenceException($"Invalid step type '{step}' in '{pipelineDefinition.Name}' pipeline.");

                    builder.Add(stepType);
                }
            }

            return builder.Build();
        }

        private static Assembly AssemblyResolver(AssemblyName assemblyName)
        {
            assemblyName.Version = null;
            return Assembly.Load(assemblyName);
        }
    }
}
