using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ArraySorter.Api.HostedService
{
    public class SortingHostedService : BackgroundService
    {
        private readonly ILogger<SortingHostedService> _logger;

        public IServiceProvider Services { get; }

        public SortingHostedService(ILogger<SortingHostedService> logger, IServiceProvider services)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) =>
            await DoWork(stoppingToken);

        private async Task DoWork(CancellationToken token)
        {
            _logger.LogInformation($"{nameof(SortingHostedService)} is running.");

            using var scope = Services.CreateScope();

            var service = scope.ServiceProvider.GetRequiredService<ISortingProcessingService>();

            await service.DoWork(token);
        }
    }
}
