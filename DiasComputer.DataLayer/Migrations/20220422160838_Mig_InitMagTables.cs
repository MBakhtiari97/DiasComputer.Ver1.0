using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiasComputer.DataLayer.Migrations
{
    public partial class Mig_InitMagTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAnswered",
                table: "Reviews",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    AuthorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AuthorEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AuthorAvatar = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AuthorPhone = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UniqueCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NationalCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.AuthorId);
                });

            migrationBuilder.CreateTable(
                name: "MagBanners",
                columns: table => new
                {
                    BannerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BannerImg = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BannerSize = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BannerAlt = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MagBanners", x => x.BannerId);
                });

            migrationBuilder.CreateTable(
                name: "MagGroups",
                columns: table => new
                {
                    GroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MagGroups", x => x.GroupId);
                    table.ForeignKey(
                        name: "FK_MagGroups_MagGroups_ParentId",
                        column: x => x.ParentId,
                        principalTable: "MagGroups",
                        principalColumn: "GroupId");
                });

            migrationBuilder.CreateTable(
                name: "MagNewsletters",
                columns: table => new
                {
                    MN_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MN_Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MN_Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MagNewsletters", x => x.MN_Id);
                });

            migrationBuilder.CreateTable(
                name: "MagTickets",
                columns: table => new
                {
                    TicketId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TicketDescription = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    TicketSenderName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TicketSenderEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TicketSenderIP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MagTickets", x => x.TicketId);
                });

            migrationBuilder.CreateTable(
                name: "Mags",
                columns: table => new
                {
                    MagId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MagTitle = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    WrittenAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MagImg = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MagShortDescription = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    MagDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    LikeCount = table.Column<int>(type: "int", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mags", x => x.MagId);
                    table.ForeignKey(
                        name: "FK_Mags_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "AuthorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MagReviews",
                columns: table => new
                {
                    ReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReviewName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ReviewEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ReviewWebsite = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReviewText = table.Column<string>(type: "nvarchar(1200)", maxLength: 1200, nullable: false),
                    ReviewDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    ReportedCount = table.Column<int>(type: "int", nullable: true),
                    MagId = table.Column<int>(type: "int", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    IsAnswered = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MagReviews", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_MagReviews_Mags_MagId",
                        column: x => x.MagId,
                        principalTable: "Mags",
                        principalColumn: "MagId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MagReviews_Reviews_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Reviews",
                        principalColumn: "ReviewId");
                });

            migrationBuilder.CreateTable(
                name: "MagTags",
                columns: table => new
                {
                    TagId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagTitle = table.Column<string>(type: "nvarchar(1200)", maxLength: 1200, nullable: false),
                    MagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MagTags", x => x.TagId);
                    table.ForeignKey(
                        name: "FK_MagTags_Mags_MagId",
                        column: x => x.MagId,
                        principalTable: "Mags",
                        principalColumn: "MagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MagToGroups",
                columns: table => new
                {
                    MG_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    MagId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MagToGroups", x => x.MG_Id);
                    table.ForeignKey(
                        name: "FK_MagToGroups_MagGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "MagGroups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MagToGroups_Mags_MagId",
                        column: x => x.MagId,
                        principalTable: "Mags",
                        principalColumn: "MagId");
                });

            migrationBuilder.CreateTable(
                name: "MagVisitors",
                columns: table => new
                {
                    MV_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MagId = table.Column<int>(type: "int", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MagVisitors", x => x.MV_Id);
                    table.ForeignKey(
                        name: "FK_MagVisitors_Mags_MagId",
                        column: x => x.MagId,
                        principalTable: "Mags",
                        principalColumn: "MagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MagGroups_ParentId",
                table: "MagGroups",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_MagReviews_MagId",
                table: "MagReviews",
                column: "MagId");

            migrationBuilder.CreateIndex(
                name: "IX_MagReviews_ParentId",
                table: "MagReviews",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Mags_AuthorId",
                table: "Mags",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_MagTags_MagId",
                table: "MagTags",
                column: "MagId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MagToGroups_GroupId",
                table: "MagToGroups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_MagToGroups_MagId",
                table: "MagToGroups",
                column: "MagId");

            migrationBuilder.CreateIndex(
                name: "IX_MagVisitors_MagId",
                table: "MagVisitors",
                column: "MagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MagBanners");

            migrationBuilder.DropTable(
                name: "MagNewsletters");

            migrationBuilder.DropTable(
                name: "MagReviews");

            migrationBuilder.DropTable(
                name: "MagTags");

            migrationBuilder.DropTable(
                name: "MagTickets");

            migrationBuilder.DropTable(
                name: "MagToGroups");

            migrationBuilder.DropTable(
                name: "MagVisitors");

            migrationBuilder.DropTable(
                name: "MagGroups");

            migrationBuilder.DropTable(
                name: "Mags");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropColumn(
                name: "IsAnswered",
                table: "Reviews");
        }
    }
}
