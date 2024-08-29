namespace PipelineInitializerExample.Steps
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;

    public sealed class SurroundWithSquareBracketsTransformStep : IStep<TextProcessingArgs>
    {
        public async Task ExecuteAsync(TextProcessingArgs args, StepDelegate next, IInvokablePipeline<TextProcessingArgs> invokablePipeline, CancellationToken cancellationToken = default)
        {
            args.Text = '[' + args.Text + ']';

            if (args.Text.Length > 15)
            {
                throw new ApplicationException("Text too long.");
            }

            await next();
        }
    }
}
