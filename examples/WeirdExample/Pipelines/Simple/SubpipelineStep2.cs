namespace WeirdExample.Pipelines.Simple
{
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;

    public class SubpipelineStep2 : IStep<SimpleArgs>
    {
        public async ValueTask ExecuteAsync(SimpleArgs args, StepDelegate next, IInvokablePipeline<SimpleArgs> invokablePipeline, CancellationToken cancellationToken = default)
        {
            args.Value += GetType().Name + " > ";

            await next();

            args.Value += GetType().Name + " < ";
        }
    }
}
