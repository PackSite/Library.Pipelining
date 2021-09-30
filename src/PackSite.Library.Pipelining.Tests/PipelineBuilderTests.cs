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
                .AddStep<StepWithArgs1>()
                .AddStep<StepWithArgs1>()
                .AddSteps(new StepWithArgs2(), new StepWithArgs2())
                .AddSteps(new List<IBaseStep> { new StepWithArgs2(), new StepWithArgs1() })
                .InsertStep<StepWithArgs3>(0)
                .AddStep<GenericStep>()
                .AddStep<GenericStep>()
                .Lifetime(lifetime)
                .Build();

            // Assert
            pipeline.Should().NotBeNull();
            pipeline.Name.Should().Be(DefaultName);
            pipeline.Description.Should().Be(DefaultDescription);
            pipeline.Lifetime.Should().Be(lifetime);
            pipeline.Steps.Should().ContainInOrder(typeof(StepWithArgs1), typeof(StepWithArgs1), typeof(StepWithArgs2), typeof(GenericStep), typeof(GenericStep));

            pipeline.ToString().Should().NotBeNull();
            pipeline.ToString().Should().ContainAll(
                typeof(StepWithArgs1).FullName,
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
            pipeline.Name.Should().Be(DefaultName);
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
            pipeline.Name.Should().Be(typeof(IPipeline<SampleArgs>).FullName!);
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
            pipeline.Name.Should().Be(typeof(IPipeline<SampleArgs>).FullName!);
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
                    .AddStep<InvalidStep>()
                    .Build();
            };

            // Assert
            action.Should().Throw<ArgumentException>().WithMessage("*Invalid step instance type.*");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null!)]
        public void Should_not_create_pipeline_with_empty_or_whitespace_name(string name)
        {
            // Act
            Action action = () =>
            {
                IPipeline pipeline = PipelineBuilder.Create<SampleArgs>()
                    .Name(name)
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
