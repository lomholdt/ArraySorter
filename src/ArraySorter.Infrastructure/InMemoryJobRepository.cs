using System;
using ArraySorter.Domain;
using ArraySorter.Application;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace ArraySorter.Infrastructure
{
    public class InMemoryJobRepository : IJobRepository
    {
        private readonly ILogger<InMemoryJobRepository> _logger;

        private static readonly ConcurrentDictionary<Guid, Job> _database = new();

        public InMemoryJobRepository(ILogger<InMemoryJobRepository> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Job? Get(Guid id)
        {
            _logger.LogInformation($"Getting Job {id}");

            var job = _database.GetValueOrDefault(id);

            return job;
        }

        public IEnumerable<Job> Get()
        {
            _logger.LogInformation("Getting all jobs");

            var jobs = _database.Values.ToArray();

            return jobs;
        }

        public void Save(Job job)
        {
            _logger.LogInformation($"Saving job {job.Id}");

            _database.AddOrUpdate(job.Id, job, (key, oldValue) => job);
        }
    }
}
