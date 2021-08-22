namespace PackSite.Library.Pipelining.Tests.Data.Steps
{
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining.Tests.Data.Contexts;

    public class StepWithContext2 : IStep<SampleContext>
    {
        /// <inheritdoc/>
        public async ValueTask ExecuteAsync(SampleContext context, StepDelegate next, IInvokablePipeline<SampleContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            context.DataIn.Add(GetType());

            await next();

            context.DataOut.Add(GetType());
        }
    }
}
