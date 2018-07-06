using System;

namespace AnteyaSidOnContainers.BuildingBlocks.EventBus.EventBus.AnteyaSid.Events
{
    public class IntegrationEvent
    {
        public IntegrationEvent()
        {
            IntegrationEventId = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        public Guid IntegrationEventId { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
