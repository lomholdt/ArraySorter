using ArraySorter.Api.Controllers.Job;
using ArraySorter.Application;
using ArraySorter.ReadModels;
using ArraySorter.ReadModels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ArraySorter.Api.UnitTest
{
    public abstract class JobControllerTest
    {
        private readonly ILogger<JobController> _logger = Substitute.For<ILogger<JobController>>();

        private readonly ICommandHandler<EnqueueCommand> _handler = Substitute.For<ICommandHandler<EnqueueCommand>>();

        private readonly IJobQueries _readModel = Substitute.For<IJobQueries>();

        private readonly IEnumerable<Job> _jobs;

        private readonly Job _job1 = new Job
        {
            Id = Guid.NewGuid(),
        };

        private readonly Job _job2 = new Job
        {
            Id = Guid.NewGuid(),
        };

        private readonly JobController _sut;

        public JobControllerTest()
        {
            _jobs = new[] { _job1, _job2 };

            _sut = new JobController(_logger, _handler, _readModel);
        }

        public sealed class When_Creating_Instance : JobControllerTest
        {

            [Fact]
            public void It_throws_when_logger_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => new JobController(default, _handler, _readModel));
            }

            [Fact]
            public void It_throws_when_handler_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => new JobController(_logger, default, _readModel));
            }

            [Fact]
            public void It_throws_when_queries_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => new JobController(_logger, _handler, default));
            }
        }

        public sealed class When_Enqueueing : JobControllerTest
        {
            private readonly Request _request;

            public When_Enqueueing()
            {
                _request = new Request { Input = new[] { 42, 13, 7, 9 } };
            }

            [Fact]
            public async Task It_invokes_the_handler()
            {
                await _sut.Enqueue(_request);

                await _handler.Received().Handle(Arg.Any<EnqueueCommand>());
            }

            [Fact]
            public async Task It_returns_the_correct_type()
            {
                var response = await _sut.Enqueue(_request);

                Assert.IsType<OkObjectResult>(response);
            }
        }

        public sealed class When_Getting_Overview : JobControllerTest
        {
            public When_Getting_Overview()
            {
                _readModel.Get().Returns(_jobs);
            }

            [Fact]
            public void It_invokes_Get()
            {
                _sut.Overview();

                _readModel.Received().Get();
            }

            [Fact]
            public void It_returns_a_list_of_jobs()
            {
                var response = _sut.Overview();

                Assert.IsAssignableFrom<IEnumerable<Job>>(response);
            }

            [Fact]
            public void It_returns_all_the_jobs()
            {
                var expected = _jobs.Count();

                var response = _sut.Overview();

                var actual = response.Count();

                Assert.Equal(expected, actual);
            }
        }

        public sealed class When_Getting : JobControllerTest
        {
            private readonly Guid _id = Guid.NewGuid();

            public When_Getting()
            {
                _readModel.Get(_id).Returns(_job1);
            }

            [Fact]
            public void It_invokes_Get()
            {
                _sut.Get(_id);

                _readModel.Received().Get(_id);
            }

            [Fact]
            public void It_returns_a_single_job()
            {
                var response = _sut.Get(_id);

                Assert.IsType<Job>(response);
            }

            [Fact]
            public void It_returns_the_correct_job()
            {
                var expected = _id;

                _readModel.Get(_id).Returns(new Job { Id = _id });

                var response = _sut.Get(expected);

                var actual = response.Id;

                Assert.Equal(expected, actual);
            }
        }
    }
}
