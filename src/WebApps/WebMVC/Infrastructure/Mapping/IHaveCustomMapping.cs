namespace AnteyaSidOnContainers.WebApps.WebMVC.Infrastructure.Mapping
{
    using AutoMapper;
    using AutoMapper.Configuration;

    public interface IHaveCustomMappings
    {
        void CreateMappings(IConfigurationProvider configuration);
    }
}
