namespace WeirdExample.Pipelines.DemoData
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;

    public class DemoDataStep1 : IStep<DemoDataArgs>
    {
        public async Task ExecuteAsync(DemoDataArgs args, StepDelegate next, IInvokablePipeline<DemoDataArgs> invokablePipeline, CancellationToken cancellationToken = default)
        {
            args.Value++;

            await next();

            if (args.Value > 5)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
