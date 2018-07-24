namespace AnteyaSidOnContainers.Services.Catalog.API.Application.Commands
{
    using AnteyaSidOnContainers.Services.Catalog.Data.Models;
    using MediatR;
    using System.Runtime.Serialization;

    public class CreateCatalogItemCommand : IRequest<CatalogItem>
    {
        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        public decimal Price { get; private set; }

        [DataMember]
        public string Color { get; private set; }

        public CreateCatalogItemCommand(string name, decimal price, string color)
        {
            this.Name = name;
            this.Price = price;
            this.Color = color;
        }
    }
}
