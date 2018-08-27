namespace AnteyaSidOnContainers.Services.Catalog.API.Application.IntegrationEvents.EventHandling
{
    using AnteyaSidOnContainers.BuildingBlocks.EventBus.EventBus.AnteyaSid.Abstractions;
    using AnteyaSidOnContainers.Services.Catalog.API.Application.Commands;
    using AnteyaSidOnContainers.Services.Catalog.API.Application.IntegrationEvents.Events;
    using AnteyaSidOnContainers.Services.Catalog.Data.Models;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;

    public class CatalogItemUpdateIntegrationEventHandler : IIntegrationEventHandler<CatalogItemUpdateIntegrationEvent>
    {
        private readonly IMediator _mediator;
        private readonly ILoggerFactory _logger;

        public CatalogItemUpdateIntegrationEventHandler(
            IMediator mediator,
            ILoggerFactory logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        ///  Integration event handler which starts the Catalog Item create proccess
        /// </summary>
        /// <param name="eventMsg">
        ///  Integration event message which is sent by the WebMvc once Catalog Item is successfully created from the ui and validated from the WebMvc.
        /// </param>
        /// <returns></returns>
        public async Task Handle(CatalogItemUpdateIntegrationEvent eventMsg)
        {
            if (eventMsg.RequestId != Guid.Empty)
            {
                var updateCatalogItemCommand = new UpdateCatalogItemCommand(eventMsg.Id, eventMsg.Name, eventMsg.Price, eventMsg.Color);

                var requestUpdateCatalogItem = new IdentifiedCommand<UpdateCatalogItemCommand, CatalogItem>(updateCatalogItemCommand, eventMsg.RequestId);

                var result = await _mediator.Send(requestUpdateCatalogItem);
            }

            _logger.CreateLogger(nameof(CatalogItemUpdateIntegrationEventHandler))
                .LogTrace(false ? $"CatalogItemUpdate integration event has been received and a create new order process is started with requestId: {eventMsg.RequestId}" :
                    $"CatalogItemUpdate integration event has been received but a new order process has failed with requestId: {eventMsg.RequestId}");
        }
    }
}
