using System;
using System.Collections.Generic;
using System.Linq;

namespace ArraySorter.ReadModels.Models
{
    public record Job
    {
        public Guid Id { get; init; }

        public IEnumerable<int> Input { get; init; } = Enumerable.Empty<int>();

        public IEnumerable<int> Output { get; init; } = Enumerable.Empty<int>();

        public DateTimeOffset Timestamp { get; init; }

        public double? Duration { get; init; }

        public Status Status { get; init; }
    }
}
