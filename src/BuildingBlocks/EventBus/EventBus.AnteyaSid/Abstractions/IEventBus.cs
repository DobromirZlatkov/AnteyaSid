using AnteyaSidOnContainers.BuildingBlocks.EventBus.EventBus.AnteyaSid.Events;

namespace AnteyaSidOnContainers.BuildingBlocks.EventBus.EventBus.AnteyaSid.Abstractions
{
    public interface IEventBus
    {
        void Publish(IntegrationEvent @event);

        void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        void SubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler;

        void UnsubscribeDynamic<TH>(string eventname)
            where TH : IDynamicIntegrationEventHandler;
    }
}
