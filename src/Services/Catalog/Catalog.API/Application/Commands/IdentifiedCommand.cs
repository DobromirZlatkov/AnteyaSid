namespace AnteyaSidOnContainers.Services.Catalog.API.Application.Commands
{
    using MediatR;
    using System;

    public class IdentifiedCommand : IRequest<int>
    {
        public CreateCatalogItemCommand Command { get; }

        public Guid Id { get; }

        public IdentifiedCommand(CreateCatalogItemCommand command, Guid id)
        {
            Command = command;
            Id = id;
        }
    }
}
