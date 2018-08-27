namespace AnteyaSidOnContainers.Services.Catalog.API.Application.Commands
{
    using AnteyaSidOnContainers.Services.Catalog.Data.Models;
    using AnteyaSidOnContainers.Services.Catalog.Services.Data.Contracts;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateCatalogItemCommandHandler : IRequestHandler<CreateCatalogItemCommand, CatalogItem>
    {
        private readonly ICatalogItemService _catalogItemService;

        public CreateCatalogItemCommandHandler(
            ICatalogItemService catalogItemService)
        {
            _catalogItemService = catalogItemService ?? throw new ArgumentNullException(nameof(catalogItemService));
        }

        public async Task<CatalogItem> Handle(CreateCatalogItemCommand request, CancellationToken cancellationToken)
        {
            return await _catalogItemService.CreateNew(request.Name, request.Price, request.Color);
        }
    }
}
