using System;
using ArraySorter.Domain;

namespace ArraySorter.Application
{
    public class SystemTimeProvider : ISystemDateTime
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}
