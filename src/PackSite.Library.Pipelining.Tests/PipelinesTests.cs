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
    using PackSite.Library.Pipelining.Tests.Data.Args;
    using PackSite.Library.Pipelining.Tests.Data.Extensions;
    using PackSite.Library.Pipelining.Tests.Data.Initializers;
    using PackSite.Library.Pipelining.Tests.Data.Steps;
    using Xunit;

    public class PipelinesTests
    {
        private const string DefaultName = "tests-demo";
        private const string DefaultDescription = "Invoked in tests.";

        [Fact]
        public async Task Should_create_with_initializer_and_invoke_with_DI_container()
        {
            // Arrange
            using ServiceProvider services = new ServiceCollection()
                .AddOptions()
                .AddLogging(builder => builder.AddDebug().SetMinimumLevel(LogLevel.Trace))
                .AddPipelining(builder =>
                {
                    builder.AddInitializer<SamplePipelineInitializer>();
                })
                .BuildServiceProvider(true);

            using IServiceScope scope = services.CreateScope();
            IPipelineCollection pipelines = scope.ServiceProvider.GetRequiredService<IPipelineCollection>();
            IInvokablePipelineFactory pipelineFactory = scope.ServiceProvider.GetRequiredService<IInvokablePipelineFactory>();

            pipelines.Names.Should().BeEmpty();

            await services.FakeHostLifecycleAsync(async (ct) =>
            {
                pipelines.Names.Should().Contain(SamplePipelineInitializer.Names);
                pipelines.Names.Should().HaveCount(SamplePipelineInitializer.Names.Length);

                SampleArgs args = new();

                foreach (PipelineName name in SamplePipelineInitializer.Names)
                {
                    IInvokablePipeline<SampleArgs> invokablePipeline = pipelineFactory.GetRequiredPipeline<SampleArgs>(name);
                    await invokablePipeline.InvokeAsync(args, ct);
                }
            });
        }

        [Fact]
        public async Task Should_create_with_simple_delegate_initializer_and_invoke_with_DI_container()
        {
            // Arrange
            using ServiceProvider services = new ServiceCollection()
                .AddOptions()
                .AddLogging(builder => builder.AddDebug().SetMinimumLevel(LogLevel.Trace))
                .AddPipelining(builder =>
                {
                    builder.AddInitializer(DelegatePipelineInitializer.Simple);
                })
                .BuildServiceProvider(true);

            using IServiceScope scope = services.CreateScope();
            IPipelineCollection pipelines = scope.ServiceProvider.GetRequiredService<IPipelineCollection>();
            IInvokablePipelineFactory pipelineFactory = scope.ServiceProvider.GetRequiredService<IInvokablePipelineFactory>();

            pipelines.Names.Should().BeEmpty();

            await services.FakeHostLifecycleAsync(async (ct) =>
            {
                pipelines.Names.Should().Contain(SamplePipelineInitializer.Names);
                pipelines.Names.Should().HaveCount(SamplePipelineInitializer.Names.Length);

                SampleArgs args = new();

                foreach (PipelineName name in SamplePipelineInitializer.Names)
                {
                    IInvokablePipeline<SampleArgs> invokablePipeline = pipelineFactory.GetRequiredPipeline<SampleArgs>(name);
                    await invokablePipeline.InvokeAsync(args, ct);
                }
            });
        }

        [Fact]
        public async Task Should_create_with_coomplex_delegate_initializer_and_invoke_with_DI_container()
        {
            // Arrange
            using ServiceProvider services = new ServiceCollection()
                .AddOptions()
                .AddLogging(builder => builder.AddDebug().SetMinimumLevel(LogLevel.Trace))
                .AddPipelining(builder =>
                {
                    builder.AddInitializer(DelegatePipelineInitializer.Complex);
                })
                .BuildServiceProvider(true);

            using IServiceScope scope = services.CreateScope();
            IPipelineCollection pipelines = scope.ServiceProvider.GetRequiredService<IPipelineCollection>();
            IInvokablePipelineFactory pipelineFactory = scope.ServiceProvider.GetRequiredService<IInvokablePipelineFactory>();

            pipelines.Names.Should().BeEmpty();

            await services.FakeHostLifecycleAsync(async (ct) =>
            {
                pipelines.Names.Should().Contain(SamplePipelineInitializer.Names);
                pipelines.Names.Should().HaveCount(SamplePipelineInitializer.Names.Length);

                SampleArgs args = new();

                foreach (PipelineName name in SamplePipelineInitializer.Names)
                {
                    IInvokablePipeline<SampleArgs> invokablePipeline = pipelineFactory.GetRequiredPipeline<SampleArgs>(name);
                    await invokablePipeline.InvokeAsync(args, ct);
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
            pipelineName ??= typeof(IPipeline<SampleArgs>).FullName!;

            using ServiceProvider services = new ServiceCollection()
                .AddPipelining()
                .BuildServiceProvider(true);

            using IServiceScope scope = services.CreateScope();

            IPipelineCollection pipelines = scope.ServiceProvider.GetRequiredService<IPipelineCollection>();

            // Act
            IPipeline pipeline = PipelineBuilder.Create<SampleArgs>()
                .Name(pipelineName)
                .Description(DefaultDescription)
                .Step<StepWithArgs1>()
                .Step<StepWithArgs2>()
                .Step(new StepWithArgs3())
                .Step<GenericStep>()
                .Lifetime(lifetime)
                .Build();

            // Assert
            pipeline.Should().NotBeNull();
            pipeline.Name.Should().Be(pipelineName);
            pipeline.Description.Should().Be(DefaultDescription);
            pipeline.Lifetime.Should().Be(lifetime);
            pipeline.Steps.Should().ContainInOrder(typeof(StepWithArgs1), typeof(StepWithArgs2), typeof(StepWithArgs3), typeof(GenericStep));

            // Act & Assert
            pipelines.Should().BeEmpty();
            pipeline.TryAddTo(pipelines, out IPipeline pipeline2).Should().BeTrue();
            pipeline.Should().Be(pipeline2);

            pipelines.Names.Should().Contain(pipelineName);
            pipelines.Count.Should().Be(1);
            pipelines.Should().Contain(pipeline);

            pipelines.GetOrDefault<SampleArgs>("invalid-name").Should().BeNull();

            IInvokablePipelineFactory pipelineFactory = scope.ServiceProvider.GetRequiredService<IInvokablePipelineFactory>();
            IInvokablePipeline<SampleArgs> invokablePipeline = useDefault ?
                pipelineFactory.GetRequiredPipeline<SampleArgs>() :
                pipelineFactory.GetRequiredPipeline<SampleArgs>(pipelineName);

            SampleArgs args = new();
            await invokablePipeline.InvokeAsync(args, CancellationToken.None);

            // Assert
            args.DataIn.Should().ContainInOrder(typeof(StepWithArgs1), typeof(StepWithArgs2), typeof(StepWithArgs3));
            args.DataIn.Should().NotContain(typeof(GenericStep));

            args.DataOut.Should().ContainInOrder(typeof(StepWithArgs3), typeof(StepWithArgs2), typeof(StepWithArgs1));
            args.DataOut.Should().NotContain(typeof(GenericStep));

            // Act & Assert
            pipelines.TryRemove(pipelineName).Should().BeTrue();
            pipelines.Count.Should().Be(0);
        }

        [Theory]
        [InlineData(DefaultName, InvokablePipelineLifetime.Transient, typeof(ActivatorStepActivator))]
        [InlineData(DefaultName, InvokablePipelineLifetime.Scoped, typeof(ActivatorStepActivator))]
        [InlineData(DefaultName, InvokablePipelineLifetime.Singleton, typeof(ActivatorStepActivator))]
        [InlineData(null, InvokablePipelineLifetime.Transient, typeof(ActivatorStepActivator))]
        [InlineData(null, InvokablePipelineLifetime.Scoped, typeof(ActivatorStepActivator))]
        [InlineData(null, InvokablePipelineLifetime.Singleton, typeof(ActivatorStepActivator))]

        [InlineData(DefaultName, InvokablePipelineLifetime.Transient, typeof(ActivatorUtilitiesStepActivator))]
        [InlineData(DefaultName, InvokablePipelineLifetime.Scoped, typeof(ActivatorUtilitiesStepActivator))]
        [InlineData(DefaultName, InvokablePipelineLifetime.Singleton, typeof(ActivatorUtilitiesStepActivator))]
        [InlineData(null, InvokablePipelineLifetime.Transient, typeof(ActivatorUtilitiesStepActivator))]
        [InlineData(null, InvokablePipelineLifetime.Scoped, typeof(ActivatorUtilitiesStepActivator))]
        [InlineData(null, InvokablePipelineLifetime.Singleton, typeof(ActivatorUtilitiesStepActivator))]
        public async Task Should_create_and_invoke_without_DI_container(string? pipelineName, InvokablePipelineLifetime lifetime, Type activatorType)
        {
            // Arrange
            bool useDefault = pipelineName is null;
            pipelineName ??= typeof(IPipeline<SampleArgs>).FullName!;

            IPipelineCollection pipelines = new PipelineCollection();

            // Act
            IPipeline pipeline = PipelineBuilder.Create<SampleArgs>()
                .Name(pipelineName)
                .Description(DefaultDescription)
                .Step<StepWithArgs1>()
                .Step<StepWithArgs2>()
                .Step(new StepWithArgs3())
                .Step<GenericStep>()
                .Lifetime(lifetime)
                .Build();

            // Assert
            pipeline.Should().NotBeNull();
            pipeline.Name.Should().Be(pipelineName);
            pipeline.Description.Should().Be(DefaultDescription);
            pipeline.Lifetime.Should().Be(lifetime);
            pipeline.Steps.Should().ContainInOrder(typeof(StepWithArgs1), typeof(StepWithArgs2), typeof(StepWithArgs3), typeof(GenericStep));

            // Act & Assert
            pipelines.Should().BeEmpty();
            pipeline.TryAddTo(pipelines).Should().BeTrue();
            pipelines.Names.Should().Contain(pipelineName);
            pipelines.Count.Should().Be(1);
            pipelines.Should().Contain(pipeline);

            pipelines.GetOrDefault<SampleArgs>("invalid-name").Should().BeNull();

            IPipeline<SampleArgs> pipelineFromCollection = useDefault ?
                pipelines.Get<SampleArgs>() :
                pipelines.Get<SampleArgs>(pipelineName);

            IStepActivator stepActivator = (IStepActivator)Activator.CreateInstance(activatorType)!;

            IInvokablePipeline<SampleArgs>? invokablePipeline = pipelineFromCollection?.CreateInvokable(stepActivator);
            invokablePipeline.Should().NotBeNull();

            SampleArgs args = new();
            await invokablePipeline!.InvokeAsync(args, CancellationToken.None);

            // Assert
            args.DataIn.Should().ContainInOrder(typeof(StepWithArgs1), typeof(StepWithArgs2), typeof(StepWithArgs3));
            args.DataIn.Should().NotContain(typeof(GenericStep));

            args.DataOut.Should().ContainInOrder(typeof(StepWithArgs3), typeof(StepWithArgs2), typeof(StepWithArgs1));
            args.DataOut.Should().NotContain(typeof(GenericStep));

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
            pipelineName ??= typeof(IPipeline<SampleArgs>).FullName!;

            using ServiceProvider services = new ServiceCollection()
                .AddPipelining()
                .BuildServiceProvider(true);

            using IServiceScope scope = services.CreateScope();

            IPipelineCollection pipelines = scope.ServiceProvider.GetRequiredService<IPipelineCollection>();

            // Act
            IPipeline pipeline = PipelineBuilder.Create<SampleArgs>()
                .Name(pipelineName)
                .Step<StepWithArgs1>()
                .Step<StepWithArgsThatThrowsException>()
                .Step(new StepWithArgs2())
                .Step<GenericStep>()
                .Lifetime(lifetime)
                .Build();

            pipelines.TryAdd(pipeline).Should().BeTrue();

            // Assert
            pipeline.Steps.Should().ContainInOrder(typeof(StepWithArgs1), typeof(StepWithArgsThatThrowsException), typeof(StepWithArgs2), typeof(GenericStep));

            // Act
            IInvokablePipelineFactory pipelineFactory = scope.ServiceProvider.GetRequiredService<IInvokablePipelineFactory>();
            IInvokablePipeline<SampleArgs> invokablePipeline = useDefault ?
                pipelineFactory.GetRequiredPipeline<SampleArgs>() :
                pipelineFactory.GetRequiredPipeline<SampleArgs>(pipelineName);

            SampleArgs args = new();
            Func<Task> throwable = async () =>
            {
                await invokablePipeline.InvokeAsync(args, CancellationToken.None);
            };

            await throwable.Should().ThrowAsync<PipelineInvocationException>()
                .Where(x => x.Pipeline == pipeline && x.Args == args)
                .WithInnerException<PipelineInvocationException, InvalidOperationException>().WithMessage(StepWithArgsThatThrowsException.ExceptionMessage);

            // Assert
            args.DataIn.Should().ContainInOrder(typeof(StepWithArgs1), typeof(StepWithArgsThatThrowsException), typeof(StepWithArgs2));
            args.DataIn.Should().NotContain(typeof(GenericStep));

            args.DataOut.Should().ContainInOrder(typeof(StepWithArgs2));
            args.DataOut.Should().NotContain(typeof(GenericStep));
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
            IPipeline pipeline = PipelineBuilder.Create<SampleArgs>()
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
