namespace PackSite.Library.Pipelining.Tests
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using PackSite.Library.Pipelining;
    using PackSite.Library.Pipelining.Tests.Data.Args;
    using PackSite.Library.Pipelining.Tests.Data.Steps;
    using Xunit;

    public class PipelineBuilderTests
    {
        private const string DefaultName = "tests-demo";
        private const string DefaultDescription = "Invoked in tests.";

        [Theory]
        [InlineData(InvokablePipelineLifetime.Transient)]
        [InlineData(InvokablePipelineLifetime.Scoped)]
        [InlineData(InvokablePipelineLifetime.Singleton)]
        public void Should_create_pipeline(InvokablePipelineLifetime lifetime)
        {
            // Act
            IPipeline pipeline = PipelineBuilder.Create<SampleArgs>()
                .Name(DefaultName)
                .Description(DefaultDescription)
                .Add<StepWithArgs1>()
                .Add<StepWithArgs1>()
                .AddRange(new StepWithArgs2(), new StepWithArgs2())
                .AddRange(new List<IBaseStep> { new StepWithArgs2(), new StepWithArgs1() })
                .Insert<StepWithArgs3>(0)
                .Add<GenericStep>()
                .Add<GenericStep>()
                .Lifetime(lifetime)
                .Build();

            // Assert
            pipeline.Should().NotBeNull();
            pipeline.Name.Should().Be(DefaultName);
            pipeline.Description.Should().Be(DefaultDescription);
            pipeline.Lifetime.Should().Be(lifetime);

            pipeline.Steps.Should().ContainInOrder(
                typeof(StepWithArgs3),
                typeof(StepWithArgs1),
                typeof(StepWithArgs1),
                typeof(StepWithArgs2),
                typeof(StepWithArgs2),
                typeof(StepWithArgs2),
                typeof(StepWithArgs1),
                typeof(GenericStep),
                typeof(GenericStep));
            pipeline.Steps.Should().HaveCount(9);

            pipeline.ToString().Should().NotBeNull();
            pipeline.ToString("steps", null).Should().ContainAll(
                typeof(StepWithArgs1).FullName,
                typeof(StepWithArgs2).FullName,
                typeof(StepWithArgs3).FullName,
                typeof(GenericStep).FullName,
                "[0]", "[1]", "[2]", "[3]", "[4]", "[5]");
        }

        [Theory]
        [InlineData(InvokablePipelineLifetime.Transient)]
        [InlineData(InvokablePipelineLifetime.Scoped)]
        [InlineData(InvokablePipelineLifetime.Singleton)]
        public void Should_create_pipeline_with_after(InvokablePipelineLifetime lifetime)
        {
            // Act
            IPipeline pipeline = PipelineBuilder.Create<SampleArgs>()
                .Name(DefaultName)
                .Description(DefaultDescription)
                .Add<StepWithArgs1>()
                .Insert<StepWithArgs3>(1)
                .InsertAfter<StepWithArgs2, StepWithArgs1>()
                .Lifetime(lifetime)
                .Build();

            // Assert
            pipeline.Should().NotBeNull();
            pipeline.Name.Should().Be(DefaultName);
            pipeline.Description.Should().Be(DefaultDescription);
            pipeline.Lifetime.Should().Be(lifetime);
            pipeline.Steps.Should().ContainInOrder(typeof(StepWithArgs1), typeof(StepWithArgs2), typeof(StepWithArgs3));
            pipeline.Steps.Should().HaveCount(3);

            pipeline.ToString().Should().NotBeNull();
            pipeline.ToString("steps", null).Should().ContainAll(
                typeof(StepWithArgs1).FullName,
                typeof(StepWithArgs2).FullName,
                typeof(StepWithArgs3).FullName,
                "[0]", "[1]", "[2]");
        }

        [Theory]
        [InlineData(InvokablePipelineLifetime.Transient)]
        [InlineData(InvokablePipelineLifetime.Scoped)]
        [InlineData(InvokablePipelineLifetime.Singleton)]
        public void Should_create_pipeline_with_before(InvokablePipelineLifetime lifetime)
        {
            // Act
            IPipeline pipeline = PipelineBuilder.Create<SampleArgs>()
                .Name(DefaultName)
                .Description(DefaultDescription)
                .Add<StepWithArgs1>()
                .Insert<StepWithArgs3>(1)
                .InsertBefore<StepWithArgs2, StepWithArgs3>()
                .Lifetime(lifetime)
                .Build();

            // Assert
            pipeline.Should().NotBeNull();
            pipeline.Name.Should().Be(DefaultName);
            pipeline.Description.Should().Be(DefaultDescription);
            pipeline.Lifetime.Should().Be(lifetime);
            pipeline.Steps.Should().ContainInOrder(typeof(StepWithArgs1), typeof(StepWithArgs2), typeof(StepWithArgs3));
            pipeline.Steps.Should().HaveCount(3);

            pipeline.ToString().Should().NotBeNull();
            pipeline.ToString("s", null).Should().ContainAll(
                typeof(StepWithArgs1).FullName,
                typeof(StepWithArgs2).FullName,
                typeof(StepWithArgs3).FullName,
                "[0]", "[1]", "[2]");
        }

        [Theory]
        [InlineData(InvokablePipelineLifetime.Transient)]
        [InlineData(InvokablePipelineLifetime.Scoped)]
        [InlineData(InvokablePipelineLifetime.Singleton)]
        public void Should_create_pipeline_with_cleared_steps(InvokablePipelineLifetime lifetime)
        {
            // Act
            IPipelineBuilder<SampleArgs> pipelineBuilder = PipelineBuilder.Create<SampleArgs>()
                .Name(DefaultName)
                .Description(DefaultDescription)
                .Add<StepWithArgs1>()
                .Add(typeof(StepWithArgs1))
                .Insert<StepWithArgs3>(1)
                .Insert(1, typeof(StepWithArgs3))
                .Insert(1, new StepWithArgs3())
                .InsertBefore(new StepWithArgs2(), typeof(StepWithArgs3))
                .InsertBefore<StepWithArgs3>(new StepWithArgs2())
                .InsertAfter(new StepWithArgs2(), typeof(StepWithArgs3))
                .InsertBefore<StepWithArgs2, StepWithArgs3>();

            pipelineBuilder.Steps.Clear();

            IPipeline<SampleArgs> pipeline = pipelineBuilder
                .Lifetime(lifetime)
                .Build();

            // Assert
            pipeline.Should().NotBeNull();
            pipeline.Name.Should().Be(DefaultName);
            pipeline.Description.Should().Be(DefaultDescription);
            pipeline.Lifetime.Should().Be(lifetime);

            pipeline.Steps.Should().BeEmpty();

            pipeline.ToString().Should().NotBeNull();
            pipeline.ToString().Should().NotContainAll(
                typeof(StepWithArgs1).FullName,
                typeof(StepWithArgs2).FullName,
                typeof(StepWithArgs3).FullName,
                "[0]", "[1]", "[2]");
        }

        [Theory]
        [InlineData(InvokablePipelineLifetime.Transient)]
        [InlineData(InvokablePipelineLifetime.Scoped)]
        [InlineData(InvokablePipelineLifetime.Singleton)]
        public void Should_create_pipeline_with_no_steps(InvokablePipelineLifetime lifetime)
        {
            // Act
            IPipeline pipeline = PipelineBuilder.Create<SampleArgs>()
                .Name(DefaultName)
                .Description(DefaultDescription)
                .Lifetime(lifetime)
                .Build();

            // Assert
            pipeline.Should().NotBeNull();
            pipeline.Name.Should().Be(expected: DefaultName);
            pipeline.Description.Should().Be(DefaultDescription);
            pipeline.Lifetime.Should().Be(lifetime);
            pipeline.Steps.Should().BeEmpty();
        }

        [Fact]
        public void Should_create_pipeline_with_default_name()
        {
            // Act
            IPipeline pipeline = PipelineBuilder.Create<SampleArgs>()
                .Description(DefaultDescription)
                .Build();

            // Assert
            pipeline.Name.Should().Be(IPipeline<SampleArgs>.DefaultName);
            pipeline.Description.Should().Be(DefaultDescription);
            pipeline.Lifetime.Should().Be(InvokablePipelineLifetime.Singleton);
            pipeline.Steps.Should().BeEmpty();
        }

        [Fact]
        public void Should_create_pipeline_without_setting_any_options()
        {
            // Act
            IPipeline pipeline = PipelineBuilder.Create<SampleArgs>()
                .Build();

            // Assert
            pipeline.Name.Should().Be(IPipeline<SampleArgs>.DefaultName);
            pipeline.Description.Should().BeEmpty();
            pipeline.Lifetime.Should().Be(InvokablePipelineLifetime.Singleton);
            pipeline.Steps.Should().BeEmpty();
            pipeline.ToString().Should().NotBeNull();
        }

        [Fact]
        public void Should_not_create_pipeline_when_step_is_IBaseStep()
        {
            // Act
            Action action = () =>
            {
                IPipeline pipeline = PipelineBuilder.Create<SampleArgs>()
                    .Add<InvalidStep>()
                    .Build();
            };

            // Assert
            action.Should().Throw<ArgumentException>().WithMessage("*Invalid step instance type*");

            // Act
            action = () =>
            {
                IPipeline pipeline = PipelineBuilder.Create<SampleArgs>()
                    .Insert<InvalidStep>(0)
                    .Build();
            };

            // Assert
            action.Should().Throw<ArgumentException>().WithMessage("*Invalid step instance type*");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null!)]
        public void Should_not_create_pipeline_with_empty_or_whitespace_name(string? name)
        {
            // Act
            Action action = () =>
            {
                IPipeline pipeline = PipelineBuilder.Create<SampleArgs>()
                    .Name(name!)
                    .Build();
            };

            // Assert
            action.Should().Throw<ArgumentException>().WithMessage("*name*cannot be null or whitespace*");
        }

        [Fact]
        public void Should_create_pipeline_twice()
        {
            // Act
            Action action = () =>
            {
                IPipelineBuilder<SampleArgs> builder = PipelineBuilder.Create<SampleArgs>();
                builder.Build();
                builder.Build();
            };

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Should_change_builder_after_build()
        {
            // Act
            Action action = () =>
            {
                IPipelineBuilder<SampleArgs> builder = PipelineBuilder.Create<SampleArgs>();
                builder.Build();
                builder.Name("test");
            };

            // Assert
            action.Should().NotThrow();
        }
    }
}
