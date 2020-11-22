using System;
using System.Collections.Generic;
using System.Linq;
using ArraySorter.Application;
using ArraySorter.ReadModels.Models;

namespace ArraySorter.ReadModels
{
    public class JobQueries : IJobQueries
    {
        private readonly IJobRepository _repository;

        public JobQueries(IJobRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public IEnumerable<Job> Get()
        {
            var jobs = _repository.Get();

            return jobs.Select(job => job.ToReadModel());
        }

        public Job Get(Guid id)
        {
            var job = _repository.Get(id);

            if (job is null)
            {
                return new Job { };
            }

            return job.ToReadModel();
        }
    }
}
