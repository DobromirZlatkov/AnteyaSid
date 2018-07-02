namespace AnteyaSidOnContainers.Services.Catalog.API.Application.Commands
{
    using MediatR;
    using System.Runtime.Serialization;

    // DDD and CQRS patterns comment: Note that it is recommended to implement immutable Commands
    // In this case, its immutability is achieved by having all the setters as private
    // plus only being able to update the data just once, when creating the object through its constructor.
    // References on Immutable Commands:  
    // http://cqrs.nu/Faq
    // https://docs.spine3.org/motivation/immutability.html 
    // http://blog.gauffin.org/2012/06/griffin-container-introducing-command-support/
    // https://msdn.microsoft.com/en-us/library/bb383979.aspx

    [DataContract]
    public class CreateCatalogItemCommand : IRequest<int>
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
