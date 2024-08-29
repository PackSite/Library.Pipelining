namespace PackSite.Library.Pipelining.Tests.Data.Steps
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining.Tests.Data.Args;

    public class StepWithArgsThatThrowsException : IStep<SampleArgs>
    {
        public const string ExceptionMessage = "Test exception" + nameof(StepWithArgsThatThrowsException);

        /// <inheritdoc/>
        public async Task ExecuteAsync(SampleArgs args, StepDelegate next, IInvokablePipeline<SampleArgs> invokablePipeline, CancellationToken cancellationToken = default)
        {
            args.DataIn.Add(GetType());

            await next();

            throw new InvalidOperationException(ExceptionMessage);
        }
    }
}
