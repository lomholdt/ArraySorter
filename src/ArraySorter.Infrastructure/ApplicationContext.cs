using System;
using System.Collections.Concurrent;
using ArraySorter.Domain;

namespace ArraySorter.Infrastructure.Database
{
    public class ApplicationContext : ConcurrentDictionary<Guid, Job>
    {
    }
}
