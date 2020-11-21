using System;
using System.Threading;
using System.Threading.Tasks;
using ArraySorter.Api.HostedService;
using ArraySorter.Infrastructure;
using ArraySorter.Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;
using ArraySorter.Domain;

namespace ArraySorter.Api.UnitTest.HostedService
{
    public abstract class JobHostedServiceTest
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IServiceCollection _services = new ServiceCollection();

        private readonly ILogger<JobHostedService> _logger = Substitute.For<ILogger<JobHostedService>>();

        private readonly IJobProcessingService _service = Substitute.For<IJobProcessingService>();

        private readonly JobHostedServiceWrapper _sut;

        public JobHostedServiceTest()
        {
            _services.AddLogging();

            _services.AddScoped(_ => _service);

            _services.AddScoped<IJobRepository, InMemoryJobRepository>();

            _services.AddScoped<ISystemTime, SystemTimeProvider>();

            _serviceProvider = _services.BuildServiceProvider();

            _sut = new JobHostedServiceWrapper(_logger, _serviceProvider);
        }

        public sealed class When_Creating_Instance : JobHostedServiceTest
        {
            [Fact]
            public void It_throws_when_logger_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => new JobHostedService(default, _serviceProvider));
            }

            [Fact]
            public void It_throws_when_services_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => new JobHostedService(_logger, default));
            }
        }

        public sealed class When_Executing : JobHostedServiceTest
        {
            private readonly CancellationToken token = CancellationToken.None;

            [Fact]
            public async Task It_invokes_DoWork_on_the_processing_service()
            {
                await _sut.Execute(token);

                await _service.Received().DoWork(token);
            }
        }

        private class JobHostedServiceWrapper : JobHostedService
        {
            public JobHostedServiceWrapper(ILogger<JobHostedService> logger, IServiceProvider services) : base(logger, services)
            {
            }

            public Task Execute(CancellationToken stoppingToken)
            {
                return base.ExecuteAsync(stoppingToken);
            }
        }
    }
}
