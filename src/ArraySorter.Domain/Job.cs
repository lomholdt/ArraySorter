using System;
using System.Collections.Generic;
using System.Linq;
using ArraySorter.Domain.Exceptions;

namespace ArraySorter.Domain
{
    public class Job
    {
        public Guid Id { get; init; }

        public IEnumerable<int> Input { get; private set; } = Enumerable.Empty<int>();

        public IEnumerable<int> Output { get; private set; } = Enumerable.Empty<int>();

        public DateTimeOffset Timestamp { get; private set; }

        public TimeSpan? Duration { get; private set; }

        public Status Status { get; private set; }

        public Job(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(id)} cannot be empty.");
            }

            Id = id;
        }

        public void Enqueue(IEnumerable<int> input, ISystemTime systemTime)
        {
            if (!input.Any())
            {
                throw new EmptyInputException("Cannot enqueue empty array");
            }

            Input = input;

            Timestamp = systemTime.UtcNow;

            Status = Status.Pending;
        }

        public void Process(ISystemTime systemTime)
        {
            if (Status != Status.Pending)
            {
                throw new InvalidStatusException($"Status '{Status}' is invalid. Expected: '{Status.Pending}'");
            }

            var start = systemTime.UtcNow;

            Output = Input.OrderBy(_ => _);

            Duration = systemTime.UtcNow - start;

            Status = Status.Completed;
        }
    }
}
