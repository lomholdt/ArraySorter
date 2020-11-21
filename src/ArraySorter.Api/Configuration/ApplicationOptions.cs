namespace ArraySorter.Api.Configuration
{
    public class ApplicationOptions
    {
        public HostedServices HostedServices { get; set; } = new HostedServices();

    }

    public sealed class HostedServices
    {
        public HostedService Sorting { get; set; } = new HostedService();
    }

    public sealed class HostedService
    {
        public int Delay { get; set; } = 1000;
    }
}
