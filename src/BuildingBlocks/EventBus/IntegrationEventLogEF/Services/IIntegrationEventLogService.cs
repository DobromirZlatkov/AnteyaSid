namespace AnteyaSidOnContainers.BuildingBlocks.EventBus.IntegrationEventLogEF.Services
{
    using System.Data.Common;
    using System.Threading.Tasks;

    using AnteyaSidOnContainers.BuildingBlocks.EventBus.EventBus.AnteyaSid.Events;

    public interface IIntegrationEventLogService
    {
        Task SaveEventAsync(IntegrationEvent @event, DbTransaction transaction);

        Task MarkEventAsPublishedAsync(IntegrationEvent @event);
    }
}
