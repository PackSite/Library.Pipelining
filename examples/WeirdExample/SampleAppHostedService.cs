namespace WeirdExample
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using PackSite.Library.Pipelining;
    using WeirdExample.Pipelines.DemoData;
    using WeirdExample.Pipelines.OtherDemoData;
    using WeirdExample.Pipelines.Simple;

    public sealed class SampleAppHostedService : BackgroundService
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

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(async () =>
            {
                try
                {
                    await InternalExecuteAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "Execution error");
                }
                finally
                {
                    _appLifetime.StopApplication();
                }
            }, stoppingToken);
        }

        private async Task InternalExecuteAsync(CancellationToken cancellationToken)
        {
            DemoDataArgs args = new();
            OtherDemoDataArgs otherArgs = new();

            await using AsyncServiceScope scope = _serviceScopeFactory.CreateAsyncScope();

            do
            {
                SimpleArgs simpleArgs = new();

                Console.WriteLine();
                IInvokablePipelineFactory invokablePipelineFactory = scope.ServiceProvider.GetRequiredService<IInvokablePipelineFactory>();
                IInvokablePipeline<DemoDataArgs> invokablePipeline = invokablePipelineFactory.GetRequiredPipeline<DemoDataArgs>();
                IInvokablePipeline<SimpleArgs> invokableSimplePipeline = invokablePipelineFactory.GetRequiredPipeline<SimpleArgs>();

                await invokablePipeline.InvokeAsync(args, cancellationToken);
                _logger.LogInformation("\n    R1: {@Result}\n    IPC: {@IPCounters}\n    PC: {@PCounters}", args, invokablePipeline.Counters, invokablePipeline.Pipeline.Counters);

                await invokableSimplePipeline.InvokeAsync(simpleArgs, cancellationToken);
                _logger.LogInformation("\n    R2: {@Result}\n    IPC: {@IPCounters}\n    PC: {@PCounters}", simpleArgs, invokableSimplePipeline.Counters, invokableSimplePipeline.Pipeline.Counters);

                IInvokablePipeline<OtherDemoDataArgs>? otherInvokablePipeline = invokablePipelineFactory.GetPipeline<OtherDemoDataArgs>("demo-other");

                if (otherInvokablePipeline is not null)
                {
                    try
                    {
                        await otherInvokablePipeline.InvokeAsync(otherArgs, cancellationToken);
                    }
                    catch (Exception)
                    {

                    }
                    finally
                    {
                        _logger.LogInformation("\n    R3: {@Result}\n    IPC: {@IPC}\n    PC: {@PCounters}", otherArgs, otherInvokablePipeline.Counters, otherInvokablePipeline.Pipeline.Counters);
                    }
                }
                Console.WriteLine("Press any key to proceed or press [Ctrl+C] to exit...");

            } while (Console.ReadKey(true).KeyChar > 0 && !cancellationToken.IsCancellationRequested);
        }
    }
}
