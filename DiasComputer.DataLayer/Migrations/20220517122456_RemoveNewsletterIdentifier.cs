using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiasComputer.DataLayer.Migrations
{
    public partial class RemoveNewsletterIdentifier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NWIdentifier",
                table: "NewsLetters");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NWIdentifier",
                table: "NewsLetters",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
