using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiasComputer.DataLayer.Migrations
{
    public partial class Mig_UpdateOrderDtails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductPrice",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductPrice",
                table: "OrderDetails");
        }
    }
}
