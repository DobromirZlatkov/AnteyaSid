namespace AnteyaSidOnContainers.Services.Catalog.API.Application.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using AnteyaSidOnContainers.Services.Catalog.Data.Models;
    using AnteyaSidOnContainers.Services.Catalog.Services.Data.Contracts;

    using MediatR;

    public class UpdateCatalogItemCommandHandler : IRequestHandler<UpdateCatalogItemCommand, CatalogItem>
    {
        private readonly ICatalogItemService _catalogItemService;
        private readonly IMediator _mediator;

        public UpdateCatalogItemCommandHandler(
            IMediator mediator, 
            ICatalogItemService catalogItemService)
        {
            _catalogItemService = catalogItemService ?? throw new ArgumentNullException(nameof(catalogItemService));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<CatalogItem> Handle(UpdateCatalogItemCommand request, CancellationToken cancellationToken)
        {
            return await this._catalogItemService.Update(request.Id, request.Name, request.Price, request.Color);
        }
    }
}