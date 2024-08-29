namespace SubPipelineExample.Pipelines
{
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;

    public class Step3 : IStep<DemoArgs>
    {
        public async Task ExecuteAsync(DemoArgs args, StepDelegate next, IInvokablePipeline<DemoArgs> invokablePipeline, CancellationToken cancellationToken = default)
        {
            args.Value += GetType().Name + " || ";

            await next();

            args.Value += GetType().Name + " < ";
        }
    }
}
