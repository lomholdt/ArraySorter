using System;
using System.Collections.Generic;
using System.Linq;
using ArraySorter.Domain.Exceptions;
using NSubstitute;
using Xunit;

namespace ArraySorter.Domain.UnitTest
{
    public abstract class JobTest
    {
        private readonly Guid _id = Guid.NewGuid();

        private readonly ISystemTime _systemTime = Substitute.For<ISystemTime>();

        private readonly DateTimeOffset _timestamp = new DateTime(1988, 09, 21);

        private readonly IEnumerable<int> _input = new[] { 42, 7, 9, 13 };

        private readonly Job _sut;

        public JobTest()
        {
            _systemTime.UtcNow.Returns(_timestamp);

            _sut = new Job(_id);
        }

        public sealed class When_Creating_Instance : JobTest
        {
            [Fact]
            public void It_throws_if_id_is_empty()
            {
                Assert.Throws<ArgumentException>(() => new Job(Guid.Empty));
            }

            [Fact]
            public void It_sets_the_id()
            {
                var expected = _id;

                var actual = _sut.Id;

                Assert.Equal(expected, actual);
            }
        }

        public sealed class When_Enqueueing : JobTest
        {
            [Fact]
            public void It_throws_if_input_is_empty()
            {
                Assert.Throws<EmptyInputException>(() => _sut.Enqueue(Enumerable.Empty<int>(), _systemTime));
            }

            [Fact]
            public void It_sets_the_input()
            {
                var expected = _input;

                _sut.Enqueue(_input, _systemTime);

                var actual = _sut.Input;

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void It_sets_the_timestamp()
            {
                var expected = _timestamp;

                _sut.Enqueue(_input, _systemTime);

                var actual = _sut.Timestamp;

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void It_sets_the_status()
            {
                var expected = Status.Pending;

                var sut = new Job(_id);

                sut.Enqueue(_input, _systemTime);

                var actual = sut.Status;

                Assert.Equal(expected, actual);
            }
        }

        public sealed class When_Processing : JobTest
        {
            public When_Processing()
            {
                _sut.Enqueue(_input, _systemTime);

                _sut.Process(_systemTime);
            }

            [Fact]
            public void It_throws_if_status_is_invalid()
            {
                Assert.Throws<InvalidStatusException>(() => _sut.Process(_systemTime));
            }

            [Fact]
            public void It_sets_the_output()
            {
                var expected = _sut.Input.OrderBy(_ => _);

                var actual = _sut.Output;

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void It_sets_the_duration()
            {
                var expected = _systemTime.UtcNow - _timestamp;

                var actual = _sut.Duration;

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void It_calculates_the_correct_duration()
            {
                _systemTime.UtcNow.Returns(new DateTime(1988, 9, 21, 0 ,0, 0));

                _sut.Enqueue(_input, _systemTime);

                _systemTime.UtcNow.Returns(new DateTime(1988, 9, 21, 0 ,0, 10));

                _sut.Process(_systemTime);

                var expected = 10000;

                var actual = _sut.Duration.Value.TotalMilliseconds;

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void It_sets_the_status()
            {
                var expected = Status.Completed;

                var actual = _sut.Status;

                Assert.Equal(expected, actual);
            }
        }
    }
}
