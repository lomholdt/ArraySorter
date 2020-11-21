using System;
using System.Linq;
using ArraySorter.Domain;
using ArraySorter.Infrastructure.Database;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace ArraySorter.Infrastructure
{
    public abstract class InMemoryJobRepositoryTest
    {
        private readonly InMemoryJobRepository _sut;

        private readonly ILogger<InMemoryJobRepository> _logger = Substitute.For<ILogger<InMemoryJobRepository>>();

        private readonly Job _job1 = new(Guid.NewGuid());

        private readonly Job _job2 = new(Guid.NewGuid());

        private readonly ApplicationContext _context = new();

        public InMemoryJobRepositoryTest()
        {
            _context.TryAdd(_job1.Id, _job1);

            _context.TryAdd(_job2.Id, _job2);

            _sut = new InMemoryJobRepository(_logger, _context);
        }

        public sealed class When_Creating_Instance : InMemoryJobRepositoryTest
        {
            [Fact]
            public void It_throws_when_logger_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => new InMemoryJobRepository(default, _context));
            }

            [Fact]
            public void It_throws_when_context_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => new InMemoryJobRepository(_logger, default));
            }
        }

        public sealed class When_Getting_By_Id : InMemoryJobRepositoryTest
        {
            [Fact]
            public void It_returns_null_if_id_does_not_exist()
            {
                var actual = _sut.Get(Guid.NewGuid());

                Assert.Null(actual);
            }

            [Fact]
            public void It_returns_the_job()
            {
                var expected = _job1;

                var actual = _sut.Get(_job1.Id);

                Assert.Equal(expected, actual);
            }
        }

        public sealed class When_Getting : InMemoryJobRepositoryTest
        {
            [Fact]
            public void It_returns_all_the_jobs()
            {
                var expected = _context.Values.Count;

                var actual = _sut.Get().Count();

                Assert.Equal(expected, actual);
            }
        }

        public sealed class When_Saving : InMemoryJobRepositoryTest
        {
            [Fact]
            public void It_saves_the_job()
            {
                var id = Guid.NewGuid();

                var job = new Job(id);

                _sut.Save(job);

                _context.TryGetValue(id, out var savedJob);

                Assert.NotNull(savedJob);
            }
        }
    }
}
