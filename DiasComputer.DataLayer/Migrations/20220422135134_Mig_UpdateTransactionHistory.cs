using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiasComputer.DataLayer.Migrations
{
    public partial class Mig_UpdateTransactionHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionToTypes_TransactionHistories_TransactionHistoryTH_Id",
                table: "TransactionToTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionToTypes_TransactionTypes_TransactionTypeTypeId",
                table: "TransactionToTypes");

            migrationBuilder.DropIndex(
                name: "IX_TransactionToTypes_TransactionHistoryTH_Id",
                table: "TransactionToTypes");

            migrationBuilder.DropIndex(
                name: "IX_TransactionToTypes_TransactionTypeTypeId",
                table: "TransactionToTypes");

            migrationBuilder.DropColumn(
                name: "TransactionHistoryTH_Id",
                table: "TransactionToTypes");

            migrationBuilder.DropColumn(
                name: "TransactionTypeTypeId",
                table: "TransactionToTypes");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionToTypes_TH_Id",
                table: "TransactionToTypes",
                column: "TH_Id");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionToTypes_TypeId",
                table: "TransactionToTypes",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionToTypes_TransactionHistories_TH_Id",
                table: "TransactionToTypes",
                column: "TH_Id",
                principalTable: "TransactionHistories",
                principalColumn: "TH_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionToTypes_TransactionTypes_TypeId",
                table: "TransactionToTypes",
                column: "TypeId",
                principalTable: "TransactionTypes",
                principalColumn: "TypeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionToTypes_TransactionHistories_TH_Id",
                table: "TransactionToTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionToTypes_TransactionTypes_TypeId",
                table: "TransactionToTypes");

            migrationBuilder.DropIndex(
                name: "IX_TransactionToTypes_TH_Id",
                table: "TransactionToTypes");

            migrationBuilder.DropIndex(
                name: "IX_TransactionToTypes_TypeId",
                table: "TransactionToTypes");

            migrationBuilder.AddColumn<int>(
                name: "TransactionHistoryTH_Id",
                table: "TransactionToTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransactionTypeTypeId",
                table: "TransactionToTypes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionToTypes_TransactionHistoryTH_Id",
                table: "TransactionToTypes",
                column: "TransactionHistoryTH_Id");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionToTypes_TransactionTypeTypeId",
                table: "TransactionToTypes",
                column: "TransactionTypeTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionToTypes_TransactionHistories_TransactionHistoryTH_Id",
                table: "TransactionToTypes",
                column: "TransactionHistoryTH_Id",
                principalTable: "TransactionHistories",
                principalColumn: "TH_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionToTypes_TransactionTypes_TransactionTypeTypeId",
                table: "TransactionToTypes",
                column: "TransactionTypeTypeId",
                principalTable: "TransactionTypes",
                principalColumn: "TypeId");
        }
    }
}
