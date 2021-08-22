//namespace PackSite.Library.Pipelining.Configuration
//{
//    using System.Collections.Generic;
//    using Microsoft.Extensions.Options;

//    /// <summary>
//    /// Pipelining configuration validator.
//    /// </summary>
//    public sealed class PipeliningConfigurationValidator : IValidateOptions<PipeliningConfiguration>
//    {
//        /// <inheritdoc/>
//        public ValidateOptionsResult Validate(string name, PipeliningConfiguration options)
//        {
//            List<string> failures = new();

//            if (options.Pipelines is List<PipelineDefinition> pipelines)
//            {
//                foreach (PipelineDefinition p in pipelines)
//                {
//                    if (p.)
//                    {
//                        failures.Add($"{nameof(TheMovieDbDataProviderConfiguration.MaxRetryCount)} cannot be lower than 0");
//                    }
//                }
//            }

//            return failures.Count > 0 ? ValidateOptionsResult.Fail(failures) : ValidateOptionsResult.Success;
//        }
//    }
//}
