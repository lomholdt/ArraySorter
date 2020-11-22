using System;
using System.Collections.Generic;
using ArraySorter.ReadModels.Models;

namespace ArraySorter.ReadModels
{
    public interface IJobQueries
    {
        IEnumerable<Job> Get();

        Job Get(Guid id);
    }
}
