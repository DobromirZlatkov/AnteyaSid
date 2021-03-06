namespace AnteyaSidOnContainers.Services.Catalog.Services.Data.Models
{
    using System;
    using System.Collections.Generic;

    public class SyncOrdersViewModel
    {
        public List<SyncOrderViewModel> Orders { get; set; }
    }

    public class SyncOrderViewModel
    {
        public string OrderTicket { get; set; }

        public string Status { get; set; }

        public string OrderSymbol { get; set; }

        public decimal OrderOpenPrice { get; set; }

        public decimal OrderTakeProfit { get; set; }

        public decimal OrderStopLoss { get; set; }

        public DateTime? OrderOpenTime { get; set; }

        public DateTime? OrderCloseTime { get; set; }

        public DateTime? OrderExpiration { get; set; }

        public decimal OrderProfit { get; set; }

        public double OrderLots { get; set; }

        public string OrderType { get; set; }

        public decimal OrderCommission { get; set; }

        public decimal OrderSwap { get; set; }

        public string Source { get; set; }
    }
}
