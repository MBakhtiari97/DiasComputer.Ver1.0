using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiasComputer.DataLayer.Migrations
{
    public partial class Mig_UpdateOrdersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Shipping_ShippingId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Shipping");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ShippingId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingId",
                table: "Orders");

            migrationBuilder.AddColumn<bool>(
                name: "IsOrderProcessed",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOrderProcessed",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "ShippingId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Shipping",
                columns: table => new
                {
                    ShippingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cost = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipping", x => x.ShippingId);
                });

            migrationBuilder.InsertData(
                table: "Shipping",
                columns: new[] { "ShippingId", "CityName", "Cost" },
                values: new object[] { 1, "تهران", 0 });

            migrationBuilder.InsertData(
                table: "Shipping",
                columns: new[] { "ShippingId", "CityName", "Cost" },
                values: new object[] { 2, "غیره", 20000 });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShippingId",
                table: "Orders",
                column: "ShippingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Shipping_ShippingId",
                table: "Orders",
                column: "ShippingId",
                principalTable: "Shipping",
                principalColumn: "ShippingId");
        }
    }
}
