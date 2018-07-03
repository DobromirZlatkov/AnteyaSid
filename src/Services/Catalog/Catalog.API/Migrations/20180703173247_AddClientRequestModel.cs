using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AnteyaSidOnContainers.Services.Catalog.API.Migrations
{
    public partial class AddClientRequestModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "CatalogItems",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP(6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "CatalogItems",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP(6)");

            migrationBuilder.CreateTable(
                name: "ClientRequests",
                columns: table => new
                {
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientRequests", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientRequests");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "CatalogItems",
                type: "TIMESTAMP(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "CatalogItems",
                type: "TIMESTAMP(6)",
                nullable: false,
                oldClrType: typeof(DateTime));
        }
    }
}
