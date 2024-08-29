namespace WeirdExample.Pipelines.OtherDemoData
{
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;

    public class OtherDemoDataStep2 : IStep<OtherDemoDataArgs>
    {
        public async Task ExecuteAsync(OtherDemoDataArgs args, StepDelegate next, IInvokablePipeline<OtherDemoDataArgs> invokablePipeline, CancellationToken cancellationToken = default)
        {
            if (args.Value > 5)
            {
                args.Value = 0;
            }

            await next();
        }
    }
}
