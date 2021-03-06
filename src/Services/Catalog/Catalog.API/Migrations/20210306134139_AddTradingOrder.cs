using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AnteyaSidOnContainers.Services.Catalog.API.Migrations
{
    public partial class AddTradingOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TradingOrders",
                columns: table => new
                {
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Id = table.Column<Guid>(nullable: false),
                    OrderTicket = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    OrderSymbol = table.Column<string>(nullable: true),
                    OrderOpenPrice = table.Column<decimal>(nullable: false),
                    OrderTakeProfit = table.Column<decimal>(nullable: false),
                    OrderStopLoss = table.Column<decimal>(nullable: false),
                    OrderOpenTime = table.Column<DateTime>(nullable: true),
                    OrderCloseTime = table.Column<DateTime>(nullable: true),
                    OrderExpiration = table.Column<DateTime>(nullable: true),
                    OrderProfit = table.Column<decimal>(nullable: false),
                    OrderLots = table.Column<double>(nullable: false),
                    OrderType = table.Column<string>(nullable: true),
                    OrderCommission = table.Column<decimal>(nullable: false),
                    OrderSwap = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradingOrders", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TradingOrders");
        }
    }
}
