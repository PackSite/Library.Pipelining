namespace SampleApp.Pipelines.Simple
{
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;

    public class SimpleStep2 : IStep<SimpleArgs>
    {
        private readonly IPipelineCollection _pipelines; //TODO: use IInvokablePipelineFactory to preserve lifetime??? or maybe not??
        private readonly IStepActivator _stepActivator;

        public SimpleStep2(IPipelineCollection pipelines, IStepActivator stepActivator)
        {
            _pipelines = pipelines;
            _stepActivator = stepActivator;
        }

        public async ValueTask ExecuteAsync(SimpleArgs args, StepDelegate next, IInvokablePipeline<SimpleArgs> invokablePipeline, CancellationToken cancellationToken = default)
        {
            args.Value += GetType().Name + " > ";

            IPipeline<SimpleArgs> subpipeline = _pipelines.Get<SimpleArgs>("dynamic-subpipeline-demo");
            IInvokablePipeline<SimpleArgs> invokableSubpipeline = subpipeline.CreateInvokable(_stepActivator, next);
            await invokableSubpipeline.InvokeAsync(args, cancellationToken);

            args.Value += GetType().Name + " < ";
        }
    }
}
