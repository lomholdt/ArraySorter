using ArraySorter.Application;
using ArraySorter.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

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

    public static class JobExtensions
    {
        public static Job ToReadModel(this Domain.Job self) => new Job
        {
            Id = self.Id,
            Input = self.Input,
            Output = self.Output,
            Timestamp = self.Timestamp,
            Duration = self.Duration?.Milliseconds,
            Status = (Status)self.Status,
        };
    }

    public record Job
    {
        public Guid Id { get; init; }

        public IEnumerable<int> Input { get; init; } = Enumerable.Empty<int>();

        public IEnumerable<int> Output { get; init; } = Enumerable.Empty<int>();

        public DateTimeOffset Timestamp { get; init; }

        public int? Duration { get; init; }

        public Status Status { get; init; }
    }

    public enum Status
    {
        None,
        Pending,
        Confirmed,
    }
}
