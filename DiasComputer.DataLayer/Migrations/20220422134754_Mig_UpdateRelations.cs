using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiasComputer.DataLayer.Migrations
{
    public partial class Mig_UpdateRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionHistories_TransactionTypes_TransactionTypeTypeId",
                table: "TransactionHistories");

            migrationBuilder.DropTable(
                name: "CouponProduct");

            migrationBuilder.DropIndex(
                name: "IX_TransactionHistories_TransactionTypeTypeId",
                table: "TransactionHistories");

            migrationBuilder.DropColumn(
                name: "TransactionTypeTypeId",
                table: "TransactionHistories");

            migrationBuilder.CreateTable(
                name: "ProductToCoupons",
                columns: table => new
                {
                    PC_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CouponId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductToCoupons", x => x.PC_Id);
                    table.ForeignKey(
                        name: "FK_ProductToCoupons_Coupons_CouponId",
                        column: x => x.CouponId,
                        principalTable: "Coupons",
                        principalColumn: "CouponId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductToCoupons_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionToTypes",
                columns: table => new
                {
                    TT_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TH_Id = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    TransactionHistoryTH_Id = table.Column<int>(type: "int", nullable: true),
                    TransactionTypeTypeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionToTypes", x => x.TT_Id);
                    table.ForeignKey(
                        name: "FK_TransactionToTypes_TransactionHistories_TransactionHistoryTH_Id",
                        column: x => x.TransactionHistoryTH_Id,
                        principalTable: "TransactionHistories",
                        principalColumn: "TH_Id");
                    table.ForeignKey(
                        name: "FK_TransactionToTypes_TransactionTypes_TransactionTypeTypeId",
                        column: x => x.TransactionTypeTypeId,
                        principalTable: "TransactionTypes",
                        principalColumn: "TypeId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductToCoupons_CouponId",
                table: "ProductToCoupons",
                column: "CouponId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductToCoupons_ProductId",
                table: "ProductToCoupons",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionToTypes_TransactionHistoryTH_Id",
                table: "TransactionToTypes",
                column: "TransactionHistoryTH_Id");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionToTypes_TransactionTypeTypeId",
                table: "TransactionToTypes",
                column: "TransactionTypeTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductToCoupons");

            migrationBuilder.DropTable(
                name: "TransactionToTypes");

            migrationBuilder.AddColumn<int>(
                name: "TransactionTypeTypeId",
                table: "TransactionHistories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CouponProduct",
                columns: table => new
                {
                    CouponsCouponId = table.Column<int>(type: "int", nullable: false),
                    ProductsProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CouponProduct", x => new { x.CouponsCouponId, x.ProductsProductId });
                    table.ForeignKey(
                        name: "FK_CouponProduct_Coupons_CouponsCouponId",
                        column: x => x.CouponsCouponId,
                        principalTable: "Coupons",
                        principalColumn: "CouponId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CouponProduct_Products_ProductsProductId",
                        column: x => x.ProductsProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionHistories_TransactionTypeTypeId",
                table: "TransactionHistories",
                column: "TransactionTypeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CouponProduct_ProductsProductId",
                table: "CouponProduct",
                column: "ProductsProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionHistories_TransactionTypes_TransactionTypeTypeId",
                table: "TransactionHistories",
                column: "TransactionTypeTypeId",
                principalTable: "TransactionTypes",
                principalColumn: "TypeId");
        }
    }
}
