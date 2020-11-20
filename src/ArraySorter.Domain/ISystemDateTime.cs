using System;

namespace ArraySorter.Domain
{
    public interface ISystemDateTime
    {
        DateTimeOffset UtcNow { get; }
    }
}
