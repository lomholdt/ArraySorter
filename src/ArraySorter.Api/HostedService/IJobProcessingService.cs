using System.Threading;
using System.Threading.Tasks;

namespace ArraySorter.Api.HostedService
{
    public interface IJobProcessingService
    {
        Task DoWork(CancellationToken stoppingToken);
    }
}
