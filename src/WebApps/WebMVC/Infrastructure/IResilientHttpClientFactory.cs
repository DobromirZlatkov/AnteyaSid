namespace AnteyaSidOnContainers.WebApps.WebMVC.Infrastructure
{
    using AnteyaSidOnContainers.BuildingBlocks.Resilience.Http;

    public interface IResilientHttpClientFactory
    {
        ResilientHttpClient CreateResilientHttpClient();
    }
}
