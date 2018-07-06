namespace AnteyaSidOnContainers.Services.Catalog.Services.Data.Idempotency
{
    using System;
    using System.Threading.Tasks;
    using AnteyaSidOnContainers.Services.Catalog.Data.Contracts;
    using AnteyaSidOnContainers.Services.Catalog.Data.Models.Idempotency;
    using AnteyaSidOnContainers.Services.Catalog.Services.Data.Contracts.Idempotency;
    using Microsoft.EntityFrameworkCore;

    public class ClientRequestService : IClientRequestService
    {
        private readonly IDeletableEntityRepository<ClientRequest> _clientRequestRepository;

        public ClientRequestService(
            IDeletableEntityRepository<ClientRequest>  clientRequestRepository
            )
        {
            _clientRequestRepository = clientRequestRepository ?? throw new ArgumentNullException(nameof(clientRequestRepository));
        }

        public async Task CreateRequestForCommandAsync<T>(Guid id)
        {
            var exists = await DoExistAsync(id);

            var request = exists ? throw new Exception($"Request with {id} already exists.") :
                new ClientRequest()
                {
                    Id = id,
                    Name = typeof(T).Name,
                };

            this._clientRequestRepository.Add(request);

            await this._clientRequestRepository.SaveChangesAsync();
        }

        public async Task<bool> DoExistAsync(Guid id)
        {
            return await this._clientRequestRepository.All().AnyAsync(x => x.Id == id);
        }
    }
}
