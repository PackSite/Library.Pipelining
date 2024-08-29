namespace PackSite.Library.Pipelining.Tests.Data.Steps
{
    using System.Threading;
    using System.Threading.Tasks;

    public class GenericStep : IStep
    {
        /// <inheritdoc/>
        public async Task ExecuteAsync(object args, StepDelegate next, IInvokablePipeline invokablePipeline, CancellationToken cancellationToken = default)
        {
            await next();
        }
    }
}
