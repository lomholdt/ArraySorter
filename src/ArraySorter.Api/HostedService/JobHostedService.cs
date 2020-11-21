using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ArraySorter.Api.HostedService
{
    public class JobHostedService : BackgroundService
    {
        private readonly ILogger<JobHostedService> _logger;

        private readonly IServiceProvider _services;

        public JobHostedService(ILogger<JobHostedService> logger, IServiceProvider services)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _services = services ?? throw new ArgumentNullException(nameof(services));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) =>
            await DoWork(stoppingToken);

        private async Task DoWork(CancellationToken token)
        {
            _logger.LogInformation($"{nameof(JobHostedService)} is running.");

            using var scope = _services.CreateScope();

            var service = scope.ServiceProvider.GetRequiredService<IJobProcessingService>();

            await service.DoWork(token);
        }
    }
}
