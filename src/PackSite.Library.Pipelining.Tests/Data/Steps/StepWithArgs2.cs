namespace PackSite.Library.Pipelining.Tests.Data.Steps
{
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining.Tests.Data.Args;

    public class StepWithArgs2 : IStep<SampleArgs>
    {
        /// <inheritdoc/>
        public async ValueTask ExecuteAsync(SampleArgs args, StepDelegate next, IInvokablePipeline<SampleArgs> invokablePipeline, CancellationToken cancellationToken = default)
        {
            args.DataIn.Add(GetType());

            await next();

            args.DataOut.Add(GetType());
        }
    }
}
