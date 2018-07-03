namespace AnteyaSidOnContainers.Services.Catalog.Services.Data.Contracts.Idempotency
{
    using System;

    using System.Threading.Tasks;

    public interface IClientRequestService : IService
    {
        Task<bool> DoExistAsync(Guid id);

        Task CreateRequestForCommandAsync<T>(Guid id);
    }
}
