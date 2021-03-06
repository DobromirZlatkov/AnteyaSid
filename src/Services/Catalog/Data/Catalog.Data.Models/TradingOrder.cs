namespace AnteyaSidOnContainers.Services.Catalog.Data.Models
{
    using AnteyaSidOnContainers.Services.Catalog.Data.Common.Models;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class TradingOrder : DeletableEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

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