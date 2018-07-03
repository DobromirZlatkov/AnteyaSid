namespace AnteyaSidOnContainers.Services.Catalog.Data.Models.Idempotency
{
    using System;

    using AnteyaSidOnContainers.Services.Catalog.Data.Common.Models;

    public class ClientRequest : DeletableEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
