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

            if (options.Pipelines is Dictionary<string, PipelineDefinition?> pipelinesToBuild)
            {
                foreach (var pipelineDefinition in pipelinesToBuild)
                {
                    if (pipelineDefinition.Value?.Enabled ?? false)
                    {
                        IPipeline pipeline = BuildPipeline(pipelineDefinition.Key, pipelineDefinition.Value);
                        pipelines.Add(pipeline);
                    }
                }
            }

            return pipelines;
        }

        private static IPipeline BuildPipeline(string pipelineName, PipelineDefinition pipelineDefinition)
        {
            _ = pipelineDefinition.ArgsType ?? throw new NullReferenceException($"Args type cannot be null in '{pipelineName}' pipeline.");

            Type argsType = Type.GetType(pipelineDefinition.ArgsType, AssemblyResolver, null) ??
                throw new NullReferenceException($"Invalid args type '{pipelineDefinition.ArgsType}' in '{pipelineName}' pipeline.");

            IPipelineBuilder builder = PipelineBuilder.Create(argsType)
                .Name(pipelineName ?? string.Empty)
                .Description(pipelineDefinition.Description ?? string.Empty)
                .Lifetime(pipelineDefinition.Lifetime);

            if (pipelineDefinition.Steps is not null)
            {
                foreach (string? stepAssemblyQualifiedName in pipelineDefinition.Steps)
                {
                    _ = stepAssemblyQualifiedName ?? throw new NullReferenceException($"Step type cannot be null in '{pipelineName}' pipeline.");

                    Type stepType = Type.GetType(stepAssemblyQualifiedName, AssemblyResolver, null) ??
                        throw new NullReferenceException($"Invalid step type '{stepAssemblyQualifiedName}' in '{pipelineName}' pipeline.");

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
