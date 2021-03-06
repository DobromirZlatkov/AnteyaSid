namespace AnteyaSidOnContainers.Services.Catalog.Services.Data.Contracts
{
    using AnteyaSidOnContainers.Services.Catalog.Data.Models;
    using AnteyaSidOnContainers.Services.Catalog.Services.Data.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITradingOrderService : IService
    {
        Task SyncOrders(SyncOrdersViewModel orders);

        Task<List<TradingOrder>> GetOrders();
    }
}
