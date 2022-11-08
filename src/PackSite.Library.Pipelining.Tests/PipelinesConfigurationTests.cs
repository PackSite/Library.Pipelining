namespace PackSite.Library.Pipelining.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using PackSite.Library.Pipelining;
    using PackSite.Library.Pipelining.Configuration;
    using PackSite.Library.Pipelining.Tests.Data.Args;
    using PackSite.Library.Pipelining.Tests.Data.Extensions;
    using PackSite.Library.Pipelining.Tests.Data.Steps;
    using Xunit;

    public class PipelinesConfigurationTests
    {
        private const string DefaultSectionName = "Pipelining";

        private const string DefaultName = "tests-demo";
        private const string DefaultDescription = "Invoked in tests.";

        [Fact]
        public async Task Should_add_pipelines_from_options()
        {
            string pipelinesSection = $"{DefaultSectionName}:{nameof(PipeliningConfiguration.Pipelines)}";

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>()
                {
                    [$"{DefaultSectionName}:{nameof(PipeliningConfiguration.ThrowOnReloadError)}"] = true.ToString(),

                    [$"{pipelinesSection}:{DefaultName}:{nameof(PipelineDefinition.Enabled)}"] = true.ToString(),
                    [$"{pipelinesSection}:{DefaultName}:{nameof(PipelineDefinition.ArgsType)}"] = typeof(SampleArgs).AssemblyQualifiedName ?? "null",
                    [$"{pipelinesSection}:{DefaultName}:{nameof(PipelineDefinition.Description)}"] = DefaultDescription,
                    [$"{pipelinesSection}:{DefaultName}:{nameof(PipelineDefinition.Lifetime)}"] = InvokablePipelineLifetime.Singleton.ToString(),
                    [$"{pipelinesSection}:{DefaultName}:{nameof(PipelineDefinition.Steps)}:0"] = typeof(StepWithArgs1).AssemblyQualifiedName ?? "null",
                    [$"{pipelinesSection}:{DefaultName}:{nameof(PipelineDefinition.Steps)}:1"] = typeof(StepWithArgs2).AssemblyQualifiedName ?? "null",
                    [$"{pipelinesSection}:{DefaultName}:{nameof(PipelineDefinition.Steps)}:2"] = typeof(StepWithArgs3).AssemblyQualifiedName ?? "null",
                    [$"{pipelinesSection}:{DefaultName}:{nameof(PipelineDefinition.Steps)}:3"] = typeof(GenericStep).AssemblyQualifiedName ?? "null",

                    [$"{pipelinesSection}:test-pipeline2:{nameof(PipelineDefinition.Enabled)}"] = true.ToString(),
                    [$"{pipelinesSection}:test-pipeline2:{nameof(PipelineDefinition.ArgsType)}"] = typeof(SampleArgs).AssemblyQualifiedName ?? "null",
                    [$"{pipelinesSection}:test-pipeline2:{nameof(PipelineDefinition.Description)}"] = DefaultDescription,
                    [$"{pipelinesSection}:test-pipeline2:{nameof(PipelineDefinition.Lifetime)}"] = InvokablePipelineLifetime.Singleton.ToString(),
                    [$"{pipelinesSection}:test-pipeline2:{nameof(PipelineDefinition.Steps)}:0"] = typeof(StepWithArgs1).AssemblyQualifiedName ?? "null",
                    [$"{pipelinesSection}:test-pipeline2:{nameof(PipelineDefinition.Steps)}:1"] = typeof(StepWithArgs2).AssemblyQualifiedName ?? "null",
                    [$"{pipelinesSection}:test-pipeline2:{nameof(PipelineDefinition.Steps)}:2"] = typeof(StepWithArgs3).AssemblyQualifiedName ?? "null",
                    [$"{pipelinesSection}:test-pipeline2:{nameof(PipelineDefinition.Steps)}:3"] = typeof(GenericStep).AssemblyQualifiedName ?? "null",

                    [$"{pipelinesSection}:test-pipeline2:{nameof(PipelineDefinition.Enabled)}"] = true.ToString(),
                    [$"{pipelinesSection}:test-pipeline2:{nameof(PipelineDefinition.ArgsType)}"] = typeof(SampleArgs).AssemblyQualifiedName ?? "null",
                    [$"{pipelinesSection}:test-pipeline2:{nameof(PipelineDefinition.Description)}"] = DefaultDescription,
                    [$"{pipelinesSection}:test-pipeline2:{nameof(PipelineDefinition.Lifetime)}"] = InvokablePipelineLifetime.Scoped.ToString(),
                    [$"{pipelinesSection}:test-pipeline2:{nameof(PipelineDefinition.Steps)}:0"] = typeof(StepWithArgs1).AssemblyQualifiedName ?? "null",
                    [$"{pipelinesSection}:test-pipeline2:{nameof(PipelineDefinition.Steps)}:1"] = typeof(StepWithArgs2).AssemblyQualifiedName ?? "null",
                    [$"{pipelinesSection}:test-pipeline2:{nameof(PipelineDefinition.Steps)}:2"] = typeof(StepWithArgs3).AssemblyQualifiedName ?? "null",
                    [$"{pipelinesSection}:test-pipeline2:{nameof(PipelineDefinition.Steps)}:3"] = typeof(GenericStep).AssemblyQualifiedName ?? "null",
                })
                .Build();

            // Arrange
            using ServiceProvider services = new ServiceCollection()
                .AddSingleton<IConfiguration>(configuration)
                .AddOptions()
                .AddLogging(builder => builder.AddDebug().SetMinimumLevel(LogLevel.Trace))
                .Configure<PipeliningConfiguration>(configuration.GetSection(DefaultSectionName))
                .AddPipelining(builder => builder.AddConfiguration())
                .BuildServiceProvider(true);

            await using AsyncServiceScope scope = services.CreateAsyncScope();
            IPipelineCollection pipelines = scope.ServiceProvider.GetRequiredService<IPipelineCollection>();

            await services.FakeHostLifecycleAsync((ct) =>
            {
                pipelines.Names.Should().Contain(DefaultName, "test-pipeline2");
                pipelines.Names.Should().HaveCount(2);

                return Task.CompletedTask;
            });

            pipelines.Names.Should().BeEmpty();
        }

        [Fact]
        public async Task Should_not_add_disabled_pipeline()
        {
            string pipelinesSection = $"{DefaultSectionName}:{nameof(PipeliningConfiguration.Pipelines)}";

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>()
                {
                    [$"{DefaultSectionName}:{nameof(PipeliningConfiguration.ThrowOnReloadError)}"] = true.ToString(),

                    [$"{pipelinesSection}:test:{nameof(PipelineDefinition.Enabled)}"] = false.ToString(),
                    [$"{pipelinesSection}:test:{nameof(PipelineDefinition.ArgsType)}"] = typeof(SampleArgs).AssemblyQualifiedName ?? "null",
                    [$"{pipelinesSection}:test:{nameof(PipelineDefinition.Description)}"] = DefaultDescription,
                    [$"{pipelinesSection}:test:{nameof(PipelineDefinition.Lifetime)}"] = InvokablePipelineLifetime.Singleton.ToString(),

                    [$"{pipelinesSection}:test2:{nameof(PipelineDefinition.Enabled)}"] = true.ToString(),
                    [$"{pipelinesSection}:test2:{nameof(PipelineDefinition.Description)}"] = DefaultDescription,
                    [$"{pipelinesSection}:test2:{nameof(PipelineDefinition.Lifetime)}"] = InvokablePipelineLifetime.Singleton.ToString(),
                })
                .Build();

            // Arrange
            using ServiceProvider services = new ServiceCollection()
                .AddSingleton<IConfiguration>(configuration)
                .AddOptions()
                .AddLogging(builder => builder.AddDebug().SetMinimumLevel(LogLevel.Trace))
                .Configure<PipeliningConfiguration>(configuration.GetSection(DefaultSectionName))
                .AddPipelining(builder =>
                {
                    builder.AddConfiguration(options =>
                    {
                        _ = options.Pipelines ?? throw new NullReferenceException("Pipelines must not be null");

                        options.Pipelines["test2"]!.SetArgsType(typeof(SampleArgs));
                        options.Pipelines["test2"]!.AddSteps(typeof(StepWithArgs1), typeof(StepWithArgs2));
                        options.Pipelines["test2"]!.AddSteps(new[] { typeof(StepWithArgs1), typeof(StepWithArgs2) });
                    });
                })
                .BuildServiceProvider(true);

            await using AsyncServiceScope scope = services.CreateAsyncScope();
            IPipelineCollection pipelineCollection = scope.ServiceProvider.GetRequiredService<IPipelineCollection>();

            await services.FakeHostLifecycleAsync((ct) =>
                {
                    pipelineCollection.Names.Should().NotContain("test");
                    pipelineCollection.Names.Should().Contain("test2");
                    pipelineCollection.Names.Should().HaveCount(1);

                    return Task.CompletedTask;
                });

            pipelineCollection.Names.Should().BeEmpty();
        }
    }
}
