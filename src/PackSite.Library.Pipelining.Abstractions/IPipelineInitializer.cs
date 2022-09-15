namespace PackSite.Library.Pipelining
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Pipeline initializer.
    /// </summary>
    public interface IPipelineInitializer
    {
        /// <summary>
        /// Invoked on host start to register pipelines.
        /// </summary>
        /// <param name="pipelines">Target pipelines collection.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns></returns>
        ValueTask RegisterAsync(IPipelineCollection pipelines, CancellationToken cancellationToken);
    }
}
