using Microsoft.EntityFrameworkCore.Migrations;

namespace AnteyaSidOnContainers.Services.Catalog.API.Migrations
{
    public partial class AddTradingOrderSource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "TradingOrders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Source",
                table: "TradingOrders");
        }
    }
}
