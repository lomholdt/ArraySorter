using System;
using System.Collections.Generic;
using System.Linq;

namespace ArraySorter.Application
{
    public class EnqueueCommand : Command
    {
        public Guid Id { get; }

        public IEnumerable<int> Input { get; } = Enumerable.Empty<int>();

        public EnqueueCommand(Guid id, IEnumerable<int> input)
        {
            Id = id;
            Input = input;
        }
    }
}
