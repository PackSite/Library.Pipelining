namespace PackSite.Library.Pipelining.Tests
{
    using System;
    using FluentAssertions;
    using PackSite.Library.Pipelining;
    using Xunit;

    public class PipelineNameTests
    {
        [Fact]
        public void Should_create_from_string()
        {
            PipelineName name = "abc";
            (name.Value == "abc").Should().BeTrue();
            (name.Value != "abc").Should().BeFalse();

            name.ToString().Should().Be("abc");
        }

        [Fact]
        public void Should_create_with_ctor()
        {
            PipelineName name = new("abc");
            (name.Value == "abc").Should().BeTrue();
            (name.Value != "abc").Should().BeFalse();

            name.ToString().Should().Be("abc");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null!)]
        public void Should_not_create_with_ctor(string pipelineNameStr)
        {
            Action action = () =>
            {
                _ = new PipelineName(pipelineNameStr);
            };

            action.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData("1", "1", 0)]
        [InlineData("1", "2", -1)]
        [InlineData("2", "1", 1)]
        [InlineData("a", "b", -1)]
        [InlineData("b", "a", 1)]
        public void Should_compare(string name0, string name1, int result)
        {
            PipelineName n0 = new(name0);
            PipelineName n1 = new(name1);

            n0.CompareTo(n1).Should().Be(result);
        }
    }
}
