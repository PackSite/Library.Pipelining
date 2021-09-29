namespace SimpleExample.Steps
{
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;

    public sealed class ToUpperTransformStep : IStep<TextProcessingArgs>
    {
        public async ValueTask ExecuteAsync(TextProcessingArgs args, StepDelegate next, IInvokablePipeline<TextProcessingArgs> invokablePipeline, CancellationToken cancellationToken = default)
        {
            args.Text = args.Text.ToUpperInvariant();

            await next();
        }
    }
}
