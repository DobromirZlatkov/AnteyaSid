namespace AnteyaSidOnContainers.Services.Catalog.API.Application.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using AnteyaSidOnContainers.Services.Catalog.Services.Data.Contracts;
    using MediatR;

    public class DeleteCatalogItemCommandHandler : IRequestHandler<DeleteCatalogItemCommand, int>
    {
        private readonly ICatalogItemService _catalogItemService;

        public DeleteCatalogItemCommandHandler(ICatalogItemService catalogItemService)
        {
            _catalogItemService = catalogItemService;
        }

        public async Task<int> Handle(DeleteCatalogItemCommand request, CancellationToken cancellationToken)
        {
            return await _catalogItemService.Delete(request.Id);
        }
    }
}
