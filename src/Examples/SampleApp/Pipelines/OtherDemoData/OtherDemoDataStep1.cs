namespace SampleApp.Pipelines.OtherDemoData
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;

    public class OtherDemoDataStep1 : IStep<OtherDemoDataContext>
    {
        public async ValueTask ExecuteAsync(OtherDemoDataContext context, StepDelegate next, IInvokablePipeline<OtherDemoDataContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            context.Value++;

            await next();

            if (context.Value > 2)
            {
                throw new InvalidOperationException("Demo");
            }
        }
    }
}
