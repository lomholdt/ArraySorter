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
    public  class JobProcessingService : IJobProcessingService
    {
        private readonly ILogger _logger;

        private readonly IJobRepository _repository;

        private readonly ISystemTime _systemTime;

        private readonly ApplicationOptions _options;

        public JobProcessingService(ILogger<JobProcessingService> logger, IJobRepository repository, ISystemTime systemTime, IOptions<ApplicationOptions> options)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _repository = repository ?? throw new ArgumentNullException(nameof(repository));

            _systemTime = systemTime ?? throw new ArgumentNullException(nameof(systemTime));

            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
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

                    job.Process(_systemTime);

                    _repository.Save(job);
                }

                var delay = _options.HostedServices.Sorting.Delay;

                _logger.LogInformation($"Delaying for {delay}ms");

                await Task.Delay(delay, stoppingToken);
            }
        }
    }
}
