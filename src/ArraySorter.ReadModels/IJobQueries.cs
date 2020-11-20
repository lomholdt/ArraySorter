using System;
using System.Collections.Generic;

namespace ArraySorter.ReadModels
{
    public interface IJobQueries
    {
        IEnumerable<Job> Get();

        Job Get(Guid id);
    }
}
