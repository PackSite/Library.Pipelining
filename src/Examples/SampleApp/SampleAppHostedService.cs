namespace SampleApp
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using PackSite.Library.Pipelining;
    using SampleApp.Pipelines.DemoData;
    using SampleApp.Pipelines.OtherDemoData;

    public sealed class SampleAppHostedService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public SampleAppHostedService(ILogger<SampleAppHostedService> logger, IHostApplicationLifetime appLifetime, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _appLifetime = appLifetime;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Starting SampleApp");

            _appLifetime.ApplicationStarted.Register(() =>
            {
                Task.Run(async () => await ExecuteAsync(_appLifetime.ApplicationStopping));
            });

            return Task.CompletedTask;
        }

        private async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            DemoDataContext context = new();
            OtherDemoDataContext otherContext = new();

            do
            {
                Console.WriteLine();

                using (IServiceScope scope = _serviceScopeFactory.CreateScope())
                {
                    IInvokablePipelineFactory invokablePipelineFactory = scope.ServiceProvider.GetRequiredService<IInvokablePipelineFactory>();
                    IInvokablePipeline<DemoDataContext> invokablePipeline = invokablePipelineFactory.GetRequiredPipeline<DemoDataContext>();

                    await invokablePipeline.InvokeAsync(context, cancellationToken);
                    _logger.LogInformation("\n    R1: {@Result}\n    IPC: {@IPCounters}\n    PC: {@PCounters}", context, invokablePipeline.Counters, invokablePipeline.Pipeline.Counters);

                    IInvokablePipeline<OtherDemoDataContext>? otherInvokablePipeline = invokablePipelineFactory.GetPipeline<OtherDemoDataContext>("demo-other");

                    if (otherInvokablePipeline is not null)
                    {
                        try
                        {
                            await otherInvokablePipeline.InvokeAsync(otherContext, cancellationToken);
                        }
                        catch (Exception)
                        {

                        }
                        finally
                        {
                            _logger.LogInformation("\n    R2: {@Result}\n    IPC: {@IPC}\n    PC: {@PCounters}", otherContext, otherInvokablePipeline.Counters, otherInvokablePipeline.Pipeline.Counters);
                        }
                    }
                }

                Console.WriteLine("Press any key to proceed or press [Ctrl+C] to exit...");

            } while (!cancellationToken.IsCancellationRequested); //(Console.ReadKey(true).KeyChar > 0 && !cancellationToken.IsCancellationRequested);

            // Stop the application once the work is done
            _appLifetime.StopApplication();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping SampleApp");

            return Task.CompletedTask;
        }
    }
}
