using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiasComputer.DataLayer.Migrations
{
    public partial class Mig_InitVoteTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecommendedCount",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LikeCount",
                table: "Mags");

            migrationBuilder.CreateTable(
                name: "MagVotes",
                columns: table => new
                {
                    VoteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MagId = table.Column<int>(type: "int", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vote = table.Column<bool>(type: "bit", nullable: false),
                    VoteDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MagVotes", x => x.VoteId);
                    table.ForeignKey(
                        name: "FK_MagVotes_Mags_MagId",
                        column: x => x.MagId,
                        principalTable: "Mags",
                        principalColumn: "MagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductVotes",
                columns: table => new
                {
                    VoteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Vote = table.Column<bool>(type: "bit", nullable: false),
                    VoteDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVotes", x => x.VoteId);
                    table.ForeignKey(
                        name: "FK_ProductVotes_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductVotes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MagVotes_MagId",
                table: "MagVotes",
                column: "MagId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVotes_ProductId",
                table: "ProductVotes",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVotes_UserId",
                table: "ProductVotes",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MagVotes");

            migrationBuilder.DropTable(
                name: "ProductVotes");

            migrationBuilder.AddColumn<int>(
                name: "RecommendedCount",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LikeCount",
                table: "Mags",
                type: "int",
                nullable: true);
        }
    }
}
