using System;
using Xunit;
using ArraySorter.Application;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace ArraySorter.ReadModels.UnitTest
{
    public abstract class JobQueriesTest
    {
        public readonly IJobRepository _repository = Substitute.For<IJobRepository>();

        private readonly Guid _id = Guid.NewGuid();

        public readonly JobQueries _sut;

        public JobQueriesTest()
        {
            _sut = new JobQueries(_repository);
        }

        public sealed class When_Creating_Instance
        {
            [Fact]
            public void It_throws_if_repository_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => new JobQueries(default));
            }
        }

        public sealed class When_Getting : JobQueriesTest
        {
            [Fact]
            public void It_invokes_get()
            {
                _sut.Get();

                _repository.Received().Get();
            }
        }

        public sealed class When_Getting_By_Id : JobQueriesTest
        {
            [Fact]
            public void It_invokes_Get()
            {
                _sut.Get(_id);

                _repository.Received().Get(_id);
            }

            [Fact]
            public void It_returns_an_empty_job_if_job_is_not_found()
            {
                _repository.Get(_id).ReturnsNull();

                var result = _sut.Get(_id);

                Assert.NotNull(result);
            }
        }
    }
}
