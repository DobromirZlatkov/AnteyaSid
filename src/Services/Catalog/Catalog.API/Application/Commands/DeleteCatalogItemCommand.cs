namespace AnteyaSidOnContainers.Services.Catalog.API.Application.Commands
{
    using MediatR;
    using System.Runtime.Serialization;

    public class DeleteCatalogItemCommand : IRequest<int>
    {
        [DataMember]
        public int Id { get; private set; }

        public DeleteCatalogItemCommand(int id)
        {
            this.Id = id;
        }
    }
}
