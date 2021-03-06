namespace AnteyaSidOnContainers.Services.Catalog.Services.Data
{
    using AnteyaSidOnContainers.Services.Catalog.Data.Contracts;
    using AnteyaSidOnContainers.Services.Catalog.Data.Models;
    using AnteyaSidOnContainers.Services.Catalog.Services.Data.Contracts;
    using AnteyaSidOnContainers.Services.Catalog.Services.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class TradingOrderService : ITradingOrderService
    {
        private readonly IDeletableEntityRepository<TradingOrder> _repo;

        public TradingOrderService(IDeletableEntityRepository<TradingOrder> repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<List<TradingOrder>> GetOrders()
        {
            var orders = await _repo.All().Where(x => x.Status == "active").ToListAsync();

            return orders;
        }

        public async Task SyncOrders(SyncOrdersViewModel model)
        {
            var orderTickets = model.Orders.Select(x => x.OrderTicket);
            var orderSource = model.Orders.Select(x => x.Source);

            var orders = await _repo
                .All()
                .Where(x => orderTickets.Contains(x.OrderTicket) && orderSource.Contains(x.Source))
                .ToListAsync();

            foreach (var reqeustOrder in model.Orders)
            {
                var currOrder = orders.FirstOrDefault(x => x.OrderTicket == reqeustOrder.OrderTicket);

                if (currOrder != null)
                {
                    currOrder.OrderTicket = reqeustOrder.OrderTicket;
                    //currOrder.Status = reqeustOrder.Status;
                    currOrder.OrderSymbol = reqeustOrder.OrderSymbol;
                    currOrder.OrderOpenPrice = reqeustOrder.OrderOpenPrice;
                    currOrder.OrderTakeProfit = reqeustOrder.OrderTakeProfit;
                    currOrder.OrderStopLoss = reqeustOrder.OrderStopLoss;
                    currOrder.OrderOpenTime = reqeustOrder.OrderOpenTime;
                    currOrder.OrderCloseTime = reqeustOrder.OrderCloseTime;
                    currOrder.OrderExpiration = reqeustOrder.OrderExpiration;
                    currOrder.OrderProfit = reqeustOrder.OrderProfit;
                    currOrder.OrderLots = reqeustOrder.OrderLots;
                    currOrder.OrderType = reqeustOrder.OrderType;
                    currOrder.OrderCommission = reqeustOrder.OrderCommission;
                    currOrder.OrderSwap = reqeustOrder.OrderSwap;
                    currOrder.Source = reqeustOrder.Source;
                }
                else
                {
                    var newOrder = new TradingOrder()
                    {
                        OrderTicket = reqeustOrder.OrderTicket,
                        Status = "active",//reqeustOrder.Status,
                        OrderSymbol = reqeustOrder.OrderSymbol,
                        OrderOpenPrice = reqeustOrder.OrderOpenPrice,
                        OrderTakeProfit = reqeustOrder.OrderTakeProfit,
                        OrderStopLoss = reqeustOrder.OrderStopLoss,
                        OrderOpenTime = reqeustOrder.OrderOpenTime,
                        OrderCloseTime = reqeustOrder.OrderCloseTime,
                        OrderExpiration = reqeustOrder.OrderExpiration,
                        OrderProfit = reqeustOrder.OrderProfit,
                        OrderLots = reqeustOrder.OrderLots,
                        OrderType = reqeustOrder.OrderType,
                        OrderCommission = reqeustOrder.OrderCommission,
                        OrderSwap = reqeustOrder.OrderSwap,
                        Source = reqeustOrder.Source,
                    };

                    _repo.Add(newOrder);
                }
            }

            // update missing as deleted
            var missingOrders = await _repo
                .All()
                .Where(x => orderSource.Contains(x.Source) && !orderTickets.Contains(x.OrderTicket) && x.Status == "active")
                .ToListAsync();

            missingOrders.ForEach(x => x.Status = "closed");

            await _repo.SaveChangesAsync();
        }
    }
}
