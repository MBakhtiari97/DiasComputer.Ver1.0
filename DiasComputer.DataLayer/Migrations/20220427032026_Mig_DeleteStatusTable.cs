using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiasComputer.DataLayer.Migrations
{
    public partial class Mig_DeleteStatusTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_AvailabilityStatus_StatusId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "AvailabilityStatus");

            migrationBuilder.DropIndex(
                name: "IX_Products_StatusId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Products");

            migrationBuilder.AddColumn<bool>(
                name: "AvailableStatus",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableStatus",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AvailabilityStatus",
                columns: table => new
                {
                    StatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvailabilityStatus", x => x.StatusId);
                });

            migrationBuilder.InsertData(
                table: "AvailabilityStatus",
                columns: new[] { "StatusId", "Status" },
                values: new object[] { 1, "موجود" });

            migrationBuilder.InsertData(
                table: "AvailabilityStatus",
                columns: new[] { "StatusId", "Status" },
                values: new object[] { 2, "ناموجود" });

            migrationBuilder.InsertData(
                table: "AvailabilityStatus",
                columns: new[] { "StatusId", "Status" },
                values: new object[] { 3, "به زودی" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_StatusId",
                table: "Products",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AvailabilityStatus_StatusId",
                table: "Products",
                column: "StatusId",
                principalTable: "AvailabilityStatus",
                principalColumn: "StatusId");
        }
    }
}
