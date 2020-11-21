using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArraySorter.Api.Configuration;
using ArraySorter.Application;
using ArraySorter.Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ArraySorter.Api.HostedService
{
    internal class SortingProcessingService : ISortingProcessingService
    {
        private readonly ILogger _logger;

        private readonly IJobRepository _repository;

        private readonly ISystemTime _systemDateTime;

        private readonly IOptionsMonitor<ApplicationOptions> _options;

        public SortingProcessingService(ILogger<SortingProcessingService> logger, IJobRepository repository, ISystemTime systemDateTime, IOptionsMonitor<ApplicationOptions> options)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _repository = repository ?? throw new ArgumentNullException(nameof(repository));

            _systemDateTime = systemDateTime ?? throw new ArgumentNullException(nameof(systemDateTime));

            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var job = _repository
                    .Get()
                    .FirstOrDefault(job => job.Status == Status.Pending);

                if (job is {})
                {
                    _logger.LogInformation($"Processing Job {job.Id}");

                    job.Process(_systemDateTime);

                    _repository.Save(job);
                }

                var delay = _options.CurrentValue.HostedServices.Sorting.Delay;

                _logger.LogInformation($"Delaying for {delay}ms");

                await Task.Delay(delay, stoppingToken);
            }
        }
    }
}
