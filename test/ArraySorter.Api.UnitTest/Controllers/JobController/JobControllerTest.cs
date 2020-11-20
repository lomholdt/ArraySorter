using ArraySorter.Api.Controllers.Job;
using ArraySorter.Application;
using ArraySorter.ReadModels;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using Xunit;

namespace ArraySorter.Api.UnitTest
{
    public abstract class JobControllerTest
    {
        public sealed class When_Creating_Instance : JobControllerTest
        {
            private readonly ILogger<JobController> _logger = Substitute.For<ILogger<JobController>>();

            private readonly ICommandHandler<EnqueueCommand> _handler = Substitute.For<ICommandHandler<EnqueueCommand>>();

            private readonly IJobQueries _queries = Substitute.For<IJobQueries>();

            [Fact]
            public void It_throws_when_logger_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => new JobController(default, _handler, _queries));
            }

            [Fact]
            public void It_throws_when_handler_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => new JobController(_logger, default, _queries));
            }

            [Fact]
            public void It_throws_when_queries_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => new JobController(_logger, _handler, default));
            }
        }
    }
}
