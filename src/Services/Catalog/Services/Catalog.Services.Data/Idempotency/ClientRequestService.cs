namespace AnteyaSidOnContainers.Services.Catalog.Services.Data.Idempotency
{
    using System;
    using System.Threading.Tasks;

    using AnteyaSidOnContainers.Services.Catalog.Services.Data.Contracts.Idempotency;

    public class ClientRequestService : IClientRequestService
    {
        public Task CreateRequestForCommandAsync<T>(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DoExistAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
