namespace AnteyaSidOnContainers.Services.Catalog.API.Application.IntegrationEvents.EventHandling
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using MediatR;

    using AnteyaSidOnContainers.BuildingBlocks.EventBus.EventBus.AnteyaSid.Abstractions;
    using AnteyaSidOnContainers.Services.Catalog.API.Application.IntegrationEvents.Events;
    using AnteyaSidOnContainers.Services.Catalog.API.Application.Commands;

    public class CatalogItemCreatedIntegrationEventHandler : IIntegrationEventHandler<CatalogItemCreatedIntegrationEvent>
    {
        private readonly IMediator _mediator;
        private readonly ILoggerFactory _logger;

        public CatalogItemCreatedIntegrationEventHandler(
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
        /// Integration event message which is sent by the WebMvc once Catalog Item is successfully created from the ui and validated from the WebMvc.
        /// </param>
        /// <returns></returns>
        public async Task Handle(CatalogItemCreatedIntegrationEvent eventMsg)
        {
            if (eventMsg.RequestId != Guid.Empty)
            {
                var createCatalogItemCommand = new CreateCatalogItemCommand(eventMsg.Name, eventMsg.Price, eventMsg.Color);

                var requestCreateCatalogItem = new IdentifiedCommand<CreateCatalogItemCommand, int>(createCatalogItemCommand, eventMsg.RequestId);

                var result = await _mediator.Send(requestCreateCatalogItem);
            }

            _logger.CreateLogger(nameof(CatalogItemCreatedIntegrationEventHandler))
                .LogTrace(false ? $"UserCheckoutAccepted integration event has been received and a create new order process is started with requestId: {eventMsg.RequestId}" :
                    $"UserCheckoutAccepted integration event has been received but a new order process has failed with requestId: {eventMsg.RequestId}");
        }
    }
}
