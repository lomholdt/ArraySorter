using System.Threading;
using System.Threading.Tasks;

namespace ArraySorter.Api.HostedService
{
    public interface ISortingProcessingService
    {
        Task DoWork(CancellationToken stoppingToken);
    }
}
