using System;
using ArraySorter.Domain;
using ArraySorter.Application;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.Extensions.Logging;
using ArraySorter.Infrastructure.Database;

namespace ArraySorter.Infrastructure
{
    public class InMemoryJobRepository : IJobRepository
    {
        private readonly ILogger<InMemoryJobRepository> _logger;

        private readonly ApplicationContext _context;

        public InMemoryJobRepository(ILogger<InMemoryJobRepository> logger, ApplicationContext context)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Job? Get(Guid id)
        {
            _logger.LogInformation($"Getting Job {id}");

            var job = _context.GetValueOrDefault(id);

            return job;
        }

        public IEnumerable<Job> Get()
        {
            _logger.LogInformation("Getting all jobs");

            var jobs = _context.Values.ToArray();

            return jobs;
        }

        public void Save(Job job)
        {
            _logger.LogInformation($"Saving job {job.Id}");

            _context.AddOrUpdate(job.Id, job, (key, oldValue) => job);
        }
    }
}
