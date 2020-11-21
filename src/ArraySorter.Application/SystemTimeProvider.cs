using System;
using ArraySorter.Domain;

namespace ArraySorter.Application
{
    public class SystemTimeProvider : ISystemTime
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}
