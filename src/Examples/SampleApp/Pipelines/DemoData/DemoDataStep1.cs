namespace SampleApp.Pipelines.DemoData
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;

    public class DemoDataStep1 : IStep<DemoDataContext>
    {
        public async ValueTask ExecuteAsync(DemoDataContext context, StepDelegate next, IInvokablePipeline<DemoDataContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            context.Value++;

            await next();

            if (context.Value > 5)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
