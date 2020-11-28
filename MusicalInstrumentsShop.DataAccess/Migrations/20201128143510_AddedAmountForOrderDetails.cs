using Microsoft.EntityFrameworkCore.Migrations;

namespace MusicalInstrumentsShop.DataAccess.Migrations
{
    public partial class AddedAmountForOrderDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "OrderDetails",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "OrderDetails");
        }
    }
}
