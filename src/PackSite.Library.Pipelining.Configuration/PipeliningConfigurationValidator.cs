namespace PackSite.Library.Pipelining.Configuration
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Pipelining configuration validator.
    /// </summary>
    public sealed class PipeliningConfigurationValidator : IValidateOptions<PipeliningConfiguration>
    {
        /// <inheritdoc/>
        public ValidateOptionsResult Validate(string name, PipeliningConfiguration options)
        {
            List<string> failures = new();

            if (options.Pipelines is not null)
            {
                foreach (var p in options.Pipelines)
                {
                    if (p.Key is not null && string.IsNullOrWhiteSpace(p.Key))
                    {
                        failures.Add($"Pipeline key must be null or non-whitespace string");
                    }

                    if (p.Value is null)
                    {
                        failures.Add($"{nameof(PipelineDefinition)} of '{p.Key}' cannot be null");
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(p.Value.ArgsType))
                    {
                        failures.Add($"{nameof(PipelineDefinition.ArgsType)} of '{p.Key}' cannot be null or whitespace");
                    }

                    if (!Enum.IsDefined(typeof(InvokablePipelineLifetime), p.Value.Lifetime))
                    {
                        failures.Add($"{nameof(PipelineDefinition.Lifetime)} of '{p.Key}' out of range");
                    }

                    if (p.Value.Steps is not null)
                    {
                        foreach (string? stepAssemblyQualifiedName in p.Value.Steps)
                        {
                            if (string.IsNullOrWhiteSpace(stepAssemblyQualifiedName))
                            {
                                failures.Add($"{nameof(PipelineDefinition.Steps)} of '{p.Key}' cannot contain null or whitespace step type");
                            }
                        }
                    }
                }
            }

            return failures.Count > 0 ? ValidateOptionsResult.Fail(failures) : ValidateOptionsResult.Success;
        }
    }
}
