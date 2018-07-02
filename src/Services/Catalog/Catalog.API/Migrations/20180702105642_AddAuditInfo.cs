using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AnteyaSidOnContainers.Services.Catalog.API.Migrations
{
    public partial class AddAuditInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "CatalogItems",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "CatalogItems",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CatalogItems",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "CatalogItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "CatalogItems");
        }
    }
}
