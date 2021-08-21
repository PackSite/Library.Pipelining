namespace PackSite.Library.Pipelining.Tests.Data.Steps
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining.Tests.Data.Contexts;

    public class StepWithContextThatThrowsException : IStep<SampleContext>
    {
        public const string ExceptionMessage = "Test exception" + nameof(StepWithContextThatThrowsException);

        /// <inheritdoc/>
        public async ValueTask ExecuteAsync(SampleContext context, StepDelegate next, CancellationToken cancellationToken = default)
        {
            context.DataIn.Add(GetType());

            await next();

            throw new InvalidOperationException(ExceptionMessage);
        }
    }
}
