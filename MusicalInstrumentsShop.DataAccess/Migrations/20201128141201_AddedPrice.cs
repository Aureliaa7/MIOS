using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MusicalInstrumentsShop.DataAccess.Migrations
{
    public partial class AddedPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpectedDeliveryDate",
                table: "OrderDetails",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "DeliveryMethods",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpectedDeliveryDate",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "DeliveryMethods");
        }
    }
}
