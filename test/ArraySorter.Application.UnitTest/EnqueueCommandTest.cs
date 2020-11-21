using System;
using System.Collections.Generic;
using Xunit;

namespace ArraySorter.Application.UnitTest
{
    public abstract class EnqueueCommandTest
    {
        public sealed class When_Creating_Instance
        {
            private readonly EnqueueCommand _sut;
            private readonly Guid _id = Guid.NewGuid();
            private readonly IEnumerable<int> _input = new[] { 42, 7, 9, 13 };

            public When_Creating_Instance()
            {
                _sut = new EnqueueCommand(_id, _input);
            }

            [Fact]
            public void It_sets_the_id()
            {
                var expected = _id;

                var actual = _sut.Id;

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void It_sets_the_input()
            {
                var expected = _input;

                var actual = _sut.Input;

                Assert.Equal(expected, actual);
            }
        }
    }
}
