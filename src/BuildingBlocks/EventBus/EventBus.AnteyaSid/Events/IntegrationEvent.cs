using System;

namespace AnteyaSidOnContainers.BuildingBlocks.EventBus.EventBus.AnteyaSid.Events
{
    public class IntegrationEvent
    {
        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        public Guid Id { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
