using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AnteyaSidOnContainers.Services.Catalog.API.Migrations
{
    public partial class AddAudotInfoToCatalogItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "CreatedOn",
                table: "CatalogItems",
                nullable: false,
                defaultValue: new byte[] {  });

            migrationBuilder.AddColumn<byte[]>(
                name: "DeletedOn",
                table: "CatalogItems",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CatalogItems",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<byte[]>(
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
