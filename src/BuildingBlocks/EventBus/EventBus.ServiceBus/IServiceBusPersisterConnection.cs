namespace AnteyaSidOnContainers.BuildingBlocks.EventBus.EventBus.ServiceBus
{
    using System;

    using Microsoft.Azure.ServiceBus;

    public interface IServiceBusPersisterConnection : IDisposable
    {
        ServiceBusConnectionStringBuilder ServiceBusConnectionStringBuilder { get; }

        ITopicClient CreateModel();
    }
}
