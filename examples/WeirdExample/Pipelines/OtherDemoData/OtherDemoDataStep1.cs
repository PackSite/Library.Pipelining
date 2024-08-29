namespace WeirdExample.Pipelines.OtherDemoData
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;

    public class OtherDemoDataStep1 : IStep<OtherDemoDataArgs>
    {
        public async Task ExecuteAsync(OtherDemoDataArgs args, StepDelegate next, IInvokablePipeline<OtherDemoDataArgs> invokablePipeline, CancellationToken cancellationToken = default)
        {
            args.Value++;

            await next();

            if (args.Value > 2)
            {
                throw new InvalidOperationException("Demo");
            }
        }
    }
}
