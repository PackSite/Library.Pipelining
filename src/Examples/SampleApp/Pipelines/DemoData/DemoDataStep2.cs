namespace SampleApp.Pipelines.DemoData
{
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;

    public class DemoDataStep2 : IStep<DemoDataArgs>
    {
        public async ValueTask ExecuteAsync(DemoDataArgs args, StepDelegate next, IInvokablePipeline<DemoDataArgs> invokablePipeline, CancellationToken cancellationToken = default)
        {
            if (args.Value > 10)
            {
                args.Value = 0;
            }

            await next();
        }
    }
}
