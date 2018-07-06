namespace AnteyaSidOnContainers.Services.Catalog.Services.Data.Contracts
{
    using System;
    using System.Threading.Tasks;

    public interface IRequestManager : IService
    {
        Task<bool> ExistAsync(Guid id);

        Task CreateRequestForCommandAsync<T>(Guid id);
    }
}
