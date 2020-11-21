using System;
using Xunit;
using NSubstitute;
using ArraySorter.Domain;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ArraySorter.Application.UnitTest
{
    public abstract class EnqueueCommandHandlerTest
    {
        private readonly IJobRepository _repository = Substitute.For<IJobRepository>();

        private readonly ISystemTime _systemTime = Substitute.For<ISystemTime>();

        public sealed class When_Creating_Instance : EnqueueCommandHandlerTest
        {
            [Fact]
            public void It_throws_if_system_time_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => new EnqueueCommandHandler(default, _repository));
            }

            [Fact]
            public void It_throws_if_repository_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => new EnqueueCommandHandler(_systemTime, default));
            }
        }

        public sealed class When_Handling : EnqueueCommandHandlerTest
        {
            private readonly Guid _id = Guid.NewGuid();

            private readonly IEnumerable<int> _input = new [] { 42, 7, 9, 13 };

            private readonly EnqueueCommandHandler _sut;
            private readonly EnqueueCommand _command;

            public When_Handling()
            {
                _command = new EnqueueCommand(_id, _input);

                _sut = new EnqueueCommandHandler(_systemTime, _repository);
            }

            [Fact]
            public async Task It_throws_if_id_is_empty()
            {
                var id = Guid.Empty;

                var command = new EnqueueCommand(id, _input);

                await Assert.ThrowsAsync<ArgumentException>(() => _sut.Handle(command));
            }

            [Fact]
            public async Task It_invokes_save_on_the_repository()
            {
                await _sut.Handle(_command);

                _repository.Received().Save(Arg.Any<Job>());
            }
        }
    }
}
