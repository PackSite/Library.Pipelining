namespace SimpleExample.Steps
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;

    public class ExceptionHandlingStep : IStep
    {
        public ExceptionHandlingStep()
        {

        }

        public async ValueTask ExecuteAsync(object args, StepDelegate next, IInvokablePipeline invokablePipeline, CancellationToken cancellationToken = default)
        {
            try
            {
                await next();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("An exception occurred while executing '{0}' pipeline:", invokablePipeline.Pipeline.Name);
                Console.Error.WriteLine(ex.Message);

                // Retry
                //await invokablePipeline.InvokeAsync(args, cancellationToken);
            }
        }
    }
}
