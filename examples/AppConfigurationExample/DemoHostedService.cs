﻿namespace AppConfigurationExample
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
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        using (IServiceScope scope = _serviceScopeFactory.CreateScope())
                        {
                            string text = "  Lorem ipsum ";

                            TextProcessingArgs pipelineArgs = new(text);
                            Console.WriteLine("INPUT: {0}", pipelineArgs.Text);

                            var invokablePipelineFactory = scope.ServiceProvider.GetRequiredService<IInvokablePipelineFactory>();
                            IInvokablePipeline<TextProcessingArgs> invokablePipeline = invokablePipelineFactory.GetRequiredPipeline<TextProcessingArgs>();

                            await invokablePipeline.InvokeAsync(pipelineArgs, stoppingToken);
                            Console.WriteLine("OUTPUT: {0}", pipelineArgs.Text);
                        }

                        await Task.Delay(2500);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "Demo error");
                }
            }, stoppingToken);
        }
    }
}