namespace AnteyaSidOnContainers.Services.Catalog.API.Application.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using AnteyaSidOnContainers.Services.Catalog.Services.Data.Contracts;

    using MediatR;

    public class CreateCatalogItemCommandHandler : IRequestHandler<CreateCatalogItemCommand, int>
    {
        private readonly ICatalogItemService _catalogItemService;
        private readonly IMediator _mediator;

        public CreateCatalogItemCommandHandler(
            IMediator mediator, 
            ICatalogItemService catalogItemService)
        {
            _catalogItemService = catalogItemService ?? throw new ArgumentNullException(nameof(catalogItemService));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<int> Handle(CreateCatalogItemCommand request, CancellationToken cancellationToken)
        {
            return await this._catalogItemService.CreateNew(request.Name, request.Price, request.Color);
        }
    }
}