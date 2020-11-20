using ArraySorter.Domain;
using System;
using System.Collections.Generic;

namespace ArraySorter.Application
{
    public interface IJobRepository
    {
        void Save(Job job);

        Job? Get(Guid id);

        IEnumerable<Job> Get();
    }
}
