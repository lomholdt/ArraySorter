using System;

namespace ArraySorter.Domain.Exceptions
{
    [Serializable]
    public class EmptyInputException : Exception
    {
        public EmptyInputException() { }
        public EmptyInputException(string message) : base(message) { }
        public EmptyInputException(string message, Exception inner) : base(message, inner) { }
        protected EmptyInputException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
