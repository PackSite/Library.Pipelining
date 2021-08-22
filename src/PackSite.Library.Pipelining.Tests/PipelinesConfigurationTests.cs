namespace PackSite.Library.Pipelining.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using PackSite.Library.Pipelining;
    using PackSite.Library.Pipelining.Configuration;
    using PackSite.Library.Pipelining.Tests.Data.Contexts;
    using PackSite.Library.Pipelining.Tests.Data.Extensions;
    using PackSite.Library.Pipelining.Tests.Data.Steps;
    using Xunit;

    public class PipelinesConfigurationTests
    {
        private const string DefaultSectionName = "Pipelining";

        private const string DefaultName = "tests:demo";
        private const string DefaultDescription = "Invoked in tests.";

        [Fact]
        public async Task Should_add_pipelines_from_options()
        {
            string pipelinesSection = $"{DefaultSectionName}:{nameof(PipeliningConfiguration.Pipelines)}";

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>()
                {
                    [$"{DefaultSectionName}:{nameof(PipeliningConfiguration.EnableProfiling)}"] = true.ToString(),
                    [$"{DefaultSectionName}:{nameof(PipeliningConfiguration.ThrowOnReloadError)}"] = true.ToString(),

                    [$"{pipelinesSection}:0:{nameof(PipelineDefinition.Enabled)}"] = true.ToString(),
                    [$"{pipelinesSection}:0:{nameof(PipelineDefinition.ContextType)}"] = typeof(SampleContext).AssemblyQualifiedName ?? "null",
                    [$"{pipelinesSection}:0:{nameof(PipelineDefinition.Name)}"] = DefaultName,
                    [$"{pipelinesSection}:0:{nameof(PipelineDefinition.Description)}"] = DefaultDescription,
                    [$"{pipelinesSection}:0:{nameof(PipelineDefinition.Lifetime)}"] = InvokablePipelineLifetime.Singleton.ToString(),
                    [$"{pipelinesSection}:0:{nameof(PipelineDefinition.Steps)}:0"] = typeof(StepWithContext1).AssemblyQualifiedName ?? "null",
                    [$"{pipelinesSection}:0:{nameof(PipelineDefinition.Steps)}:1"] = typeof(StepWithContext2).AssemblyQualifiedName ?? "null",
                    [$"{pipelinesSection}:0:{nameof(PipelineDefinition.Steps)}:2"] = typeof(StepWithContext3).AssemblyQualifiedName ?? "null",
                    [$"{pipelinesSection}:0:{nameof(PipelineDefinition.Steps)}:3"] = typeof(GenericStep).AssemblyQualifiedName ?? "null",

                    [$"{pipelinesSection}:1:{nameof(PipelineDefinition.Enabled)}"] = true.ToString(),
                    [$"{pipelinesSection}:1:{nameof(PipelineDefinition.ContextType)}"] = typeof(SampleContext).AssemblyQualifiedName ?? "null",
                    [$"{pipelinesSection}:1:{nameof(PipelineDefinition.Name)}"] = "test:pipeline2",
                    [$"{pipelinesSection}:1:{nameof(PipelineDefinition.Description)}"] = DefaultDescription,
                    [$"{pipelinesSection}:1:{nameof(PipelineDefinition.Lifetime)}"] = InvokablePipelineLifetime.Singleton.ToString(),
                    [$"{pipelinesSection}:1:{nameof(PipelineDefinition.Steps)}:0"] = typeof(StepWithContext1).AssemblyQualifiedName ?? "null",
                    [$"{pipelinesSection}:1:{nameof(PipelineDefinition.Steps)}:1"] = typeof(StepWithContext2).AssemblyQualifiedName ?? "null",
                    [$"{pipelinesSection}:1:{nameof(PipelineDefinition.Steps)}:2"] = typeof(StepWithContext3).AssemblyQualifiedName ?? "null",
                    [$"{pipelinesSection}:1:{nameof(PipelineDefinition.Steps)}:3"] = typeof(GenericStep).AssemblyQualifiedName ?? "null",

                    [$"{pipelinesSection}:3:{nameof(PipelineDefinition.Enabled)}"] = true.ToString(),
                    [$"{pipelinesSection}:3:{nameof(PipelineDefinition.ContextType)}"] = typeof(SampleContext).AssemblyQualifiedName ?? "null",
                    [$"{pipelinesSection}:3:{nameof(PipelineDefinition.Name)}"] = "test:pipeline2",
                    [$"{pipelinesSection}:3:{nameof(PipelineDefinition.Description)}"] = DefaultDescription,
                    [$"{pipelinesSection}:3:{nameof(PipelineDefinition.Lifetime)}"] = InvokablePipelineLifetime.Scoped.ToString(),
                    [$"{pipelinesSection}:3:{nameof(PipelineDefinition.Steps)}:0"] = typeof(StepWithContext1).AssemblyQualifiedName ?? "null",
                    [$"{pipelinesSection}:3:{nameof(PipelineDefinition.Steps)}:1"] = typeof(StepWithContext2).AssemblyQualifiedName ?? "null",
                    [$"{pipelinesSection}:3:{nameof(PipelineDefinition.Steps)}:2"] = typeof(StepWithContext3).AssemblyQualifiedName ?? "null",
                    [$"{pipelinesSection}:3:{nameof(PipelineDefinition.Steps)}:3"] = typeof(GenericStep).AssemblyQualifiedName ?? "null",
                })
                .Build();

            // Arrange
            using ServiceProvider services = new ServiceCollection()
                .AddSingleton<IConfiguration>(configuration)
                .AddOptions()
                .AddLogging(builder => builder.AddDebug().SetMinimumLevel(LogLevel.Trace))
                .Configure<PipeliningConfiguration>(configuration.GetSection(DefaultSectionName))
                .AddPipelining()
                .BuildServiceProvider(true);

            using IServiceScope scope = services.CreateScope();
            IPipelineCollection pipelines = scope.ServiceProvider.GetRequiredService<IPipelineCollection>();

            await services.FakeHostStartupAsync((ct) =>
            {
                pipelines.Names.Should().Contain(DefaultName, "test:pipeline2");
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
                .AddInMemoryCollection(new Dictionary<string, string>()
                {
                    [$"{DefaultSectionName}:{nameof(PipeliningConfiguration.EnableProfiling)}"] = true.ToString(),
                    [$"{DefaultSectionName}:{nameof(PipeliningConfiguration.ThrowOnReloadError)}"] = true.ToString(),

                    [$"{pipelinesSection}:0:{nameof(PipelineDefinition.Enabled)}"] = false.ToString(),
                    [$"{pipelinesSection}:0:{nameof(PipelineDefinition.ContextType)}"] = typeof(SampleContext).AssemblyQualifiedName ?? "null",
                    [$"{pipelinesSection}:0:{nameof(PipelineDefinition.Name)}"] = "test",
                    [$"{pipelinesSection}:0:{nameof(PipelineDefinition.Description)}"] = DefaultDescription,
                    [$"{pipelinesSection}:0:{nameof(PipelineDefinition.Lifetime)}"] = InvokablePipelineLifetime.Singleton.ToString(),

                    [$"{pipelinesSection}:2:{nameof(PipelineDefinition.Enabled)}"] = true.ToString(),
                    [$"{pipelinesSection}:2:{nameof(PipelineDefinition.Name)}"] = "test2",
                    [$"{pipelinesSection}:2:{nameof(PipelineDefinition.Description)}"] = DefaultDescription,
                    [$"{pipelinesSection}:2:{nameof(PipelineDefinition.Lifetime)}"] = InvokablePipelineLifetime.Singleton.ToString(),
                })
                .Build();

            // Arrange
            using ServiceProvider services = new ServiceCollection()
                .AddSingleton<IConfiguration>(configuration)
                .AddOptions()
                .AddLogging(builder => builder.AddDebug().SetMinimumLevel(LogLevel.Trace))
                .Configure<PipeliningConfiguration>(configuration.GetSection(DefaultSectionName))
                .AddPipelining(options =>
                {
                    options.Pipelines![1].SetContext(typeof(SampleContext));
                    options.Pipelines![1].AddSteps(typeof(StepWithContext1), typeof(StepWithContext2));
                    options.Pipelines![1].AddSteps(new[] { typeof(StepWithContext1), typeof(StepWithContext2) });
                })
                .BuildServiceProvider(true);

            using IServiceScope scope = services.CreateScope();
            IPipelineCollection pipelineCollection = scope.ServiceProvider.GetRequiredService<IPipelineCollection>();

            await services.FakeHostStartupAsync((ct) =>
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
