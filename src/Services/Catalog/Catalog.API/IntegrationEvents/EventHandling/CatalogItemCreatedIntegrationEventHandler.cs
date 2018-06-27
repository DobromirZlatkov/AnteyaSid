namespace AnteyaSidOnContainers.Services.Catalog.API.IntegrationEvents.EventHandling
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using MediatR;

    using AnteyaSidOnContainers.BuildingBlocks.EventBus.EventBus.AnteyaSid.Abstractions;
    using AnteyaSidOnContainers.Services.Catalog.API.IntegrationEvents.Events;

    public class CatalogItemCreatedIntegrationEventHandler : IIntegrationEventHandler<CatalogItemCreatedIntegrationEvent>
    {
       // private readonly IMediator _mediator;
        private readonly ILoggerFactory _logger;

        public CatalogItemCreatedIntegrationEventHandler(
           // IMediator mediator,
            ILoggerFactory logger)
        {
           // _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
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
                //var createOrderCommand = new CreateOrderCommand(eventMsg.Basket.Items, eventMsg.UserId, eventMsg.City, eventMsg.Street,
                //    eventMsg.State, eventMsg.Country, eventMsg.ZipCode,
                //    eventMsg.CardNumber, eventMsg.CardHolderName, eventMsg.CardExpiration,
                //    eventMsg.CardSecurityNumber, eventMsg.CardTypeId);

                //var requestCreateOrder = new IdentifiedCommand<CreateOrderCommand, bool>(createOrderCommand, eventMsg.RequestId);
                //result = await _mediator.Send(requestCreateOrder);
            }

            _logger.CreateLogger(nameof(CatalogItemCreatedIntegrationEventHandler))
                .LogTrace(false ? $"UserCheckoutAccepted integration event has been received and a create new order process is started with requestId: {eventMsg.RequestId}" :
                    $"UserCheckoutAccepted integration event has been received but a new order process has failed with requestId: {eventMsg.RequestId}");
        }
    }
}
