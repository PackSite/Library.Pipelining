namespace PackSite.Library.Pipelining.Tests.Data.Steps
{
    using System.Threading;
    using System.Threading.Tasks;

    public class GenericStep : IStep
    {
        /// <inheritdoc/>
        public async ValueTask ExecuteAsync(object context, StepDelegate next, CancellationToken cancellationToken = default)
        {
            await next();
        }
    }
}
