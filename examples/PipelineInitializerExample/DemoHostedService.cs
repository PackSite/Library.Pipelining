namespace PipelineInitializerExample
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using PackSite.Library.Pipelining;

    public sealed class DemoHostedService : BackgroundService
    {
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public DemoHostedService(IHostApplicationLifetime appLifetime, ILogger<DemoHostedService> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _appLifetime = appLifetime;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(async () =>
            {
                try
                {
                    await using (AsyncServiceScope scope = _serviceScopeFactory.CreateAsyncScope())
                    {
                        Console.WriteLine("Type some text (for a string with length > 15, a demo excpetion is thrown):");
                        string text = Console.ReadLine() ?? string.Empty;

                        TextProcessingArgs pipelineArgs0 = new(text);
                        TextProcessingArgs pipelineArgs1 = new(text);
                        Console.WriteLine("INPUT: {0}", pipelineArgs0.Text);

                        IInvokablePipelineFactory invokablePipelineFactory = scope.ServiceProvider.GetRequiredService<IInvokablePipelineFactory>();
                        IInvokablePipeline<TextProcessingArgs> invokablePipeline0 = invokablePipelineFactory.GetRequiredPipeline<TextProcessingArgs>();
                        IInvokablePipeline<TextProcessingArgs> invokablePipeline1 = invokablePipelineFactory.GetRequiredPipeline<TextProcessingArgs>("text-processsing-pipeline");

                        await invokablePipeline0.InvokeAsync(pipelineArgs0, stoppingToken);
                        await invokablePipeline1.InvokeAsync(pipelineArgs1, stoppingToken);
                        Console.WriteLine("OUTPUT[auto-name]: {0}", pipelineArgs0.Text);
                        Console.WriteLine("OUTPUT[user-name]: {0}", pipelineArgs1.Text);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "Fatal demo error");
                }
                finally
                {
                    _appLifetime.StopApplication();
                }
            }, stoppingToken);
        }
    }
}
