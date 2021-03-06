using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiasComputer.DataLayer.Migrations
{
    public partial class Mig_UpdateReferTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ReferStatus",
                table: "ReferOrders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReferStatus",
                table: "ReferOrders");
        }
    }
}
