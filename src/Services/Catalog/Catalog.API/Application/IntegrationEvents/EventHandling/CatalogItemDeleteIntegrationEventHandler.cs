namespace AnteyaSidOnContainers.Services.Catalog.API.Application.IntegrationEvents.EventHandling
{
    using AnteyaSidOnContainers.BuildingBlocks.EventBus.EventBus.AnteyaSid.Abstractions;
    using AnteyaSidOnContainers.Services.Catalog.API.Application.Commands;
    using AnteyaSidOnContainers.Services.Catalog.API.Application.IntegrationEvents.Events;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;

    public class CatalogItemDeleteIntegrationEventHandler : IIntegrationEventHandler<CatalogItemDeleteIntegrationEvent>
    {
        private readonly IMediator _mediator;
        private readonly ILoggerFactory _logger;

        public CatalogItemDeleteIntegrationEventHandler(
            IMediator mediator,
            ILoggerFactory logger
            )
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Integration event handler which starts the Catalog item delete event
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public async Task Handle(CatalogItemDeleteIntegrationEvent @event)
        {
            if (@event.RequestId != Guid.Empty)
            {
                var deleteCatalogItemCommand = new DeleteCatalogItemCommand(@event.Id);

                var requestDeleteCatalogItem = new IdentifiedCommand<DeleteCatalogItemCommand, int>(deleteCatalogItemCommand, @event.RequestId);

                var result = await _mediator.Send(requestDeleteCatalogItem);
            }

            _logger.CreateLogger(nameof(CatalogItemUpdateIntegrationEventHandler))
                .LogTrace(false ? $"CatalogItemDelete integration event has been received and a create new order process is started with requestId: {@event.RequestId}" :
                    $"CatalogItemDelete integration event has been received but a new order process has failed with requestId: {@event.RequestId}");
        }
    }
}
