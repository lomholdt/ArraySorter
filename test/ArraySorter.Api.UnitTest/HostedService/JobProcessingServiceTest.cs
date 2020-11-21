using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ArraySorter.Api.Configuration;
using ArraySorter.Api.HostedService;
using ArraySorter.Application;
using ArraySorter.Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace ArraySorter.Api.UnitTest.HostedService
{
    public abstract class JobProcessingServiceTest
    {
        private readonly ILogger<JobProcessingService> _logger = Substitute.For<ILogger<JobProcessingService>>();

        private readonly IJobRepository _repository = Substitute.For<IJobRepository>();

        private readonly ISystemTime _systemTime = Substitute.For<ISystemTime>();

        private readonly IOptions<ApplicationOptions> _options = Substitute.For<IOptions<ApplicationOptions>>();

        private readonly ApplicationOptions _applicationOptions = new ApplicationOptions
        {
            HostedServices = new HostedServices
            {
                Sorting = new Configuration.HostedService
                {
                    Delay = 7
                }
            }
        };

        private readonly JobProcessingService _sut;

        public JobProcessingServiceTest()
        {
            _systemTime.UtcNow.Returns(new DateTime(1988, 09, 21));

            _options.Value.Returns(_applicationOptions);

            _sut = new JobProcessingService(_logger, _repository, _systemTime, _options);
        }

        public sealed class When_Creating_Instance : JobProcessingServiceTest
        {
            [Fact]
            public void It_throws_when_logger_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => new JobProcessingService(default, _repository, _systemTime, _options));
            }

            [Fact]
            public void It_throws_when_repository_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => new JobProcessingService(_logger, default, _systemTime, _options));
            }

            [Fact]
            public void It_throws_when_systemTime_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => new JobProcessingService(_logger, _repository, default, _options));
            }

            [Fact]
            public void It_throws_when_options_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => new JobProcessingService(_logger, _repository, _systemTime, default));
            }
        }

        public sealed class When_Doing_Work : JobProcessingServiceTest
        {
            private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();

            private readonly CancellationToken _token;

            private readonly Job _job;

            private readonly IEnumerable<int> _input = new[] { 42, 7, 9, 13 };

            public When_Doing_Work()
            {
                _token = _tokenSource.Token;

                _job = new Job(Guid.NewGuid());

                _job.Enqueue(_input, _systemTime);
            }

            [Fact]
            public async Task It_invokes_get_on_the_repository()
            {
                _repository.Get()
                    .Returns(new[] { _job })
                    .AndDoes(_ => _tokenSource.Cancel());

                await Assert.ThrowsAsync<TaskCanceledException>(() => _sut.DoWork(_token));

                _repository.Received().Get();
            }

            [Fact]
            public async Task It_invokes_save_on_the_repository()
            {
                _repository.Get()
                    .Returns(new[] { _job })
                    .AndDoes(_ => _tokenSource.Cancel());

                await Assert.ThrowsAsync<TaskCanceledException>(() => _sut.DoWork(_token));

                _repository.Received().Save(_job);
            }
        }
    }
}
