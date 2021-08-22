namespace SampleApp.Pipelines.OtherDemoData
{
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;

    public class OtherDemoDataStep2 : IStep<OtherDemoDataContext>
    {
        public async ValueTask ExecuteAsync(OtherDemoDataContext context, StepDelegate next, IInvokablePipeline<OtherDemoDataContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            if (context.Value > 5)
            {
                context.Value = 0;
            }

            await next();
        }
    }
}
