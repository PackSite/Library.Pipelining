namespace PackSite.Library.Pipelining.Tests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using PackSite.Library.Pipelining;
    using PackSite.Library.Pipelining.StepActivators;
    using PackSite.Library.Pipelining.Tests.Data;
    using PackSite.Library.Pipelining.Tests.Data.Contexts;
    using PackSite.Library.Pipelining.Tests.Data.Extensions;
    using PackSite.Library.Pipelining.Tests.Data.Steps;
    using Xunit;

    public class PipelinesTests
    {
        private const string DefaultName = "tests:demo";
        private const string DefaultDescription = "Invoked in tests.";

        [Fact]
        public async Task Should_create_with_initializer_and_invoke_with_DI_container()
        {
            // Arrange
            using ServiceProvider services = new ServiceCollection()
                .AddOptions()
                .AddLogging(builder => builder.AddDebug().SetMinimumLevel(LogLevel.Trace))
                .AddPipelining()
                .AddPipelineInitializer<SamplePipelineInitializers>()
                .BuildServiceProvider(true);

            using IServiceScope scope = services.CreateScope();
            IPipelineCollection pipelines = scope.ServiceProvider.GetRequiredService<IPipelineCollection>();
            IInvokablePipelineFactory pipelineFactory = scope.ServiceProvider.GetRequiredService<IInvokablePipelineFactory>();

            pipelines.Names.Should().BeEmpty();

            await services.FakeHostStartupAsync(async (ct) =>
            {
                pipelines.Names.Should().Contain(SamplePipelineInitializers.Names);
                pipelines.Names.Should().HaveCount(SamplePipelineInitializers.Names.Length);

                SampleContext context = new();

                foreach (PipelineName name in SamplePipelineInitializers.Names)
                {
                    IInvokablePipeline<SampleContext> invokablePipeline = pipelineFactory.GetRequiredPipeline<SampleContext>(name);
                    await invokablePipeline.InvokeAsync(context, ct);
                }
            });
        }

        [Theory]
        [InlineData(DefaultName, InvokablePipelineLifetime.Transient)]
        [InlineData(DefaultName, InvokablePipelineLifetime.Scoped)]
        [InlineData(DefaultName, InvokablePipelineLifetime.Singleton)]
        [InlineData(null, InvokablePipelineLifetime.Transient)]
        [InlineData(null, InvokablePipelineLifetime.Scoped)]
        [InlineData(null, InvokablePipelineLifetime.Singleton)]
        public async Task Should_create_and_invoke_with_DI_container(string? pipelineName, InvokablePipelineLifetime lifetime)
        {
            // Arrange
            bool useDefault = pipelineName is null;
            pipelineName ??= typeof(IPipeline<SampleContext>).FullName!;

            using ServiceProvider services = new ServiceCollection()
                .AddPipelining()
                .BuildServiceProvider(true);

            using IServiceScope scope = services.CreateScope();

            IPipelineCollection pipelines = scope.ServiceProvider.GetRequiredService<IPipelineCollection>();

            // Act
            IPipeline pipeline = PipelineBuilder.Create<SampleContext>()
                .Name(pipelineName)
                .Description(DefaultDescription)
                .Add<StepWithContext1>()
                .Add<StepWithContext2>()
                .Add(new StepWithContext3())
                .Add<GenericStep>()
                .Lifetime(lifetime)
                .Build();

            // Assert
            pipeline.Should().NotBeNull();
            pipeline.Name.Should().Be(pipelineName);
            pipeline.Description.Should().Be(DefaultDescription);
            pipeline.Lifetime.Should().Be(lifetime);
            pipeline.Steps.Should().ContainInOrder(typeof(StepWithContext1), typeof(StepWithContext2), typeof(StepWithContext3), typeof(GenericStep));

            // Act & Assert
            pipelines.Should().BeEmpty();
            pipeline.TryAddTo(pipelines, out IPipeline pipeline2).Should().BeTrue();
            pipeline.Should().Be(pipeline2);

            pipelines.Names.Should().Contain(pipelineName);
            pipelines.Count.Should().Be(1);
            pipelines.Should().Contain(pipeline);

            pipelines.GetOrDefault<SampleContext>("invalid:name").Should().BeNull();

            IInvokablePipelineFactory pipelineFactory = scope.ServiceProvider.GetRequiredService<IInvokablePipelineFactory>();
            IInvokablePipeline<SampleContext> invokablePipeline = useDefault ?
                pipelineFactory.GetRequiredPipeline<SampleContext>() :
                pipelineFactory.GetRequiredPipeline<SampleContext>(pipelineName);

            SampleContext context = new();
            await invokablePipeline.InvokeAsync(context, CancellationToken.None);

            // Assert
            context.DataIn.Should().ContainInOrder(typeof(StepWithContext1), typeof(StepWithContext2), typeof(StepWithContext3));
            context.DataIn.Should().NotContain(typeof(GenericStep));

            context.DataOut.Should().ContainInOrder(typeof(StepWithContext3), typeof(StepWithContext2), typeof(StepWithContext1));
            context.DataOut.Should().NotContain(typeof(GenericStep));

            // Act & Assert
            pipelines.TryRemove(pipelineName).Should().BeTrue();
            pipelines.Count.Should().Be(0);
        }

        [Theory]
        [InlineData(DefaultName, InvokablePipelineLifetime.Transient)]
        [InlineData(DefaultName, InvokablePipelineLifetime.Scoped)]
        [InlineData(DefaultName, InvokablePipelineLifetime.Singleton)]
        [InlineData(null, InvokablePipelineLifetime.Transient)]
        [InlineData(null, InvokablePipelineLifetime.Scoped)]
        [InlineData(null, InvokablePipelineLifetime.Singleton)]
        public async Task Should_create_and_invoke_without_DI_container(string? pipelineName, InvokablePipelineLifetime lifetime)
        {
            // Arrange
            bool useDefault = pipelineName is null;
            pipelineName ??= typeof(IPipeline<SampleContext>).FullName!;

            IPipelineCollection pipelines = new PipelineCollection();

            // Act
            IPipeline pipeline = PipelineBuilder.Create<SampleContext>()
                .Name(pipelineName)
                .Description(DefaultDescription)
                .Add<StepWithContext1>()
                .Add<StepWithContext2>()
                .Add(new StepWithContext3())
                .Add<GenericStep>()
                .Lifetime(lifetime)
                .Build();

            // Assert
            pipeline.Should().NotBeNull();
            pipeline.Name.Should().Be(pipelineName);
            pipeline.Description.Should().Be(DefaultDescription);
            pipeline.Lifetime.Should().Be(lifetime);
            pipeline.Steps.Should().ContainInOrder(typeof(StepWithContext1), typeof(StepWithContext2), typeof(StepWithContext3), typeof(GenericStep));

            // Act & Assert
            pipelines.Should().BeEmpty();
            pipeline.TryAddTo(pipelines).Should().BeTrue();
            pipelines.Names.Should().Contain(pipelineName);
            pipelines.Count.Should().Be(1);
            pipelines.Should().Contain(pipeline);

            pipelines.GetOrDefault<SampleContext>("invalid:name").Should().BeNull();

            IPipeline<SampleContext> pipelineFromCollection = useDefault ?
                pipelines.Get<SampleContext>() :
                pipelines.Get<SampleContext>(pipelineName);

            IStepActivator stepActivator = new ActivatorStepActivator();
            IInvokablePipeline<SampleContext>? invokablePipeline = pipelineFromCollection?.CreateInvokable(stepActivator);
            invokablePipeline.Should().NotBeNull();

            SampleContext context = new();
            await invokablePipeline!.InvokeAsync(context, CancellationToken.None);

            // Assert
            context.DataIn.Should().ContainInOrder(typeof(StepWithContext1), typeof(StepWithContext2), typeof(StepWithContext3));
            context.DataIn.Should().NotContain(typeof(GenericStep));

            context.DataOut.Should().ContainInOrder(typeof(StepWithContext3), typeof(StepWithContext2), typeof(StepWithContext1));
            context.DataOut.Should().NotContain(typeof(GenericStep));

            // Act & Assert
            pipelines.TryRemove(pipelineName).Should().BeTrue();
            pipelines.Count.Should().Be(0);
        }

        [Theory]
        [InlineData(DefaultName, InvokablePipelineLifetime.Transient)]
        [InlineData(DefaultName, InvokablePipelineLifetime.Scoped)]
        [InlineData(DefaultName, InvokablePipelineLifetime.Singleton)]
        [InlineData(null, InvokablePipelineLifetime.Transient)]
        [InlineData(null, InvokablePipelineLifetime.Scoped)]
        [InlineData(null, InvokablePipelineLifetime.Singleton)]
        public async Task Should_create_and_invoke_that_throws_exception(string? pipelineName, InvokablePipelineLifetime lifetime)
        {
            // Arrange
            bool useDefault = pipelineName is null;
            pipelineName ??= typeof(IPipeline<SampleContext>).FullName!;

            using ServiceProvider services = new ServiceCollection()
                .AddPipelining()
                .BuildServiceProvider(true);

            using IServiceScope scope = services.CreateScope();

            IPipelineCollection pipelines = scope.ServiceProvider.GetRequiredService<IPipelineCollection>();

            // Act
            IPipeline pipeline = PipelineBuilder.Create<SampleContext>()
                .Name(pipelineName)
                .Add<StepWithContext1>()
                .Add<StepWithContextThatThrowsException>()
                .Add(new StepWithContext2())
                .Add<GenericStep>()
                .Lifetime(lifetime)
                .Build();

            pipelines.TryAdd(pipeline).Should().BeTrue();

            // Assert
            pipeline.Steps.Should().ContainInOrder(typeof(StepWithContext1), typeof(StepWithContextThatThrowsException), typeof(StepWithContext2), typeof(GenericStep));

            // Act
            IInvokablePipelineFactory pipelineFactory = scope.ServiceProvider.GetRequiredService<IInvokablePipelineFactory>();
            IInvokablePipeline<SampleContext> invokablePipeline = useDefault ?
                pipelineFactory.GetRequiredPipeline<SampleContext>() :
                pipelineFactory.GetRequiredPipeline<SampleContext>(pipelineName);

            SampleContext context = new();
            Func<Task> throwable = async () =>
            {
                await invokablePipeline.InvokeAsync(context, CancellationToken.None);
            };

            await throwable.Should().ThrowAsync<PipelineInvocationException>()
                .Where(x => x.Pipeline == pipeline && x.Context == context)
                .WithInnerException<PipelineInvocationException, InvalidOperationException>().WithMessage(StepWithContextThatThrowsException.ExceptionMessage);

            // Assert
            context.DataIn.Should().ContainInOrder(typeof(StepWithContext1), typeof(StepWithContextThatThrowsException), typeof(StepWithContext2));
            context.DataIn.Should().NotContain(typeof(GenericStep));

            context.DataOut.Should().ContainInOrder(typeof(StepWithContext2));
            context.DataOut.Should().NotContain(typeof(GenericStep));
        }

        [Fact]
        public void Should_fire_events()
        {
            // Arrange
            using ServiceProvider services = new ServiceCollection()
                .AddPipelining()
                .BuildServiceProvider(true);

            using IServiceScope scope = services.CreateScope();

            IPipelineCollection pipelines = scope.ServiceProvider.GetRequiredService<IPipelineCollection>();

            PipelineName? lastAdded = null;
            PipelineName? lastRemoved = null;

            int addedCount = 0;
            int removedCount = 0;
            int clearedCount = 0;

            pipelines.Added += (sender, e) =>
            {
                lastAdded = e.PipelineName;
                ++addedCount;
            };

            pipelines.Removed += (sender, e) =>
            {
                lastRemoved = e.PipelineName;
                ++removedCount;
            };

            pipelines.Cleared += (sender, e) =>
            {
                ++clearedCount;
            };

            // Act
            IPipeline pipeline = PipelineBuilder.Create<SampleContext>()
                .Name(DefaultName)
                .Build();

            pipelines.TryAdd(pipeline).Should().BeTrue();
            lastAdded.Should().Be(DefaultName);
            addedCount.Should().Be(1);
            removedCount.Should().Be(0);
            clearedCount.Should().Be(0);

            pipelines.TryRemove(DefaultName);
            lastRemoved.Should().Be(DefaultName);
            addedCount.Should().Be(1);
            removedCount.Should().Be(1);
            clearedCount.Should().Be(0);

            pipelines.TryAdd(pipeline).Should().BeTrue();
            addedCount.Should().Be(2);
            removedCount.Should().Be(1);
            clearedCount.Should().Be(0);

            pipelines.Clear();
            addedCount.Should().Be(2);
            removedCount.Should().Be(1);
            clearedCount.Should().Be(1);
        }
    }
}
