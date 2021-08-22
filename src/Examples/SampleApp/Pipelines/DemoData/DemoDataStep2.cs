namespace SampleApp.Pipelines.DemoData
{
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;

    public class DemoDataStep2 : IStep<DemoDataContext>
    {
        public async ValueTask ExecuteAsync(DemoDataContext context, StepDelegate next, IInvokablePipeline<DemoDataContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            if (context.Value > 10)
            {
                context.Value = 0;
            }

            await next();
        }
    }
}
