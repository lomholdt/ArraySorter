using System;
using System.Runtime.Serialization;

namespace ArraySorter.Domain.Exceptions
{
    [Serializable]
    public class InvalidStatusException : Exception
    {
        public InvalidStatusException() { }

        public InvalidStatusException(string message) : base(message) { }

        public InvalidStatusException(string message, Exception inner) : base(message, inner) { }

        protected InvalidStatusException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
