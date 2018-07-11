namespace AnteyaSidOnContainers.Services.Catalog.API.Application.Commands
{
    using AnteyaSidOnContainers.Services.Catalog.Data.Models;
    using AnteyaSidOnContainers.Services.Catalog.Services.Data.Contracts;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class UpdateCatalogItemCommandHandler : IRequestHandler<UpdateCatalogItemCommand, CatalogItem>
    {
        private readonly ICatalogItemService _catalogItemService;

        public UpdateCatalogItemCommandHandler(
            ICatalogItemService catalogItemService)
        {
            _catalogItemService = catalogItemService ?? throw new ArgumentNullException(nameof(catalogItemService));
        }

        public async Task<CatalogItem> Handle(UpdateCatalogItemCommand request, CancellationToken cancellationToken)
        {
            return await this._catalogItemService.Update(request.Id, request.Name, request.Price, request.Color);
        }
    }
}