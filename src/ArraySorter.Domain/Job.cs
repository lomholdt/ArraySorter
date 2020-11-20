using System;
using System.Collections.Generic;
using System.Linq;

namespace ArraySorter.Domain
{
    public class Job
    {
        public Guid Id { get; init; }

        public IEnumerable<int> Input { get; private set; } = Enumerable.Empty<int>();

        public IEnumerable<int> Output { get; private set; } = Enumerable.Empty<int>();

        public DateTimeOffset Timestamp { get; private set; }

        public DateTimeOffset? Duration { get; private set; }

        public Status Status { get; private set; }

        public Job(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(id)} cannot be empty.");
            }

            Id = id;
        }

        public void Enqueue(IEnumerable<int> input, ISystemDateTime systemDateTime)
        {
            if (!input.Any())
            {
                throw new ArgumentException("Cannot enqueue empty array");
            }

            Input = input;

            Timestamp = systemDateTime.UtcNow;

            Status = Status.Pending;
        }

        public void SetOutput(IEnumerable<int> output)
        {
            if (Status != Status.Pending)
            {
                throw new Exception("Invalid status"); // TODO use custom exception
            }

            Output = output;

            Status = Status.Completed;

            // Set duration
        }
    }
}
