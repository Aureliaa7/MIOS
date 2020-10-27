using Microsoft.EntityFrameworkCore.Migrations;

namespace MusicalInstrumentsShop.DataAccess.Migrations
{
    public partial class ModifiedPhotosTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "Photos");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Photos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Photos");

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Photos",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
