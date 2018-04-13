using AnteyaSidOnContainers.HealthCHecks.Resilience.Http;

namespace AnteyaSidOnContainers.WebApps.WebMVC.Infrastructure
{
    public interface IResilientHttpClientFactory
    {
        ResilientHttpClient CreateResilientHttpClient();
    }
}
