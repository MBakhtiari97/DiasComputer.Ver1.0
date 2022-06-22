using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiasComputer.DataLayer.Migrations
{
    public partial class Mig_updateProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SaleCount",
                table: "Products",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SaleCount",
                table: "Products");
        }
    }
}
