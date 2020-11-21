using System;

namespace ArraySorter.Domain
{
    public interface ISystemTime
    {
        DateTimeOffset UtcNow { get; }
    }
}
