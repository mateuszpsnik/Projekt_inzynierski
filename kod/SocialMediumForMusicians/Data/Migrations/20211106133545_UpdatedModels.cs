using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialMediumForMusicians.Data.Migrations
{
    public partial class UpdatedModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Accepted",
                table: "Meetings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Blocked",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "EmailMessage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecipentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Read = table.Column<bool>(type: "bit", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailMessage_Musicians_RecipentId",
                        column: x => x.RecipentId,
                        principalTable: "Musicians",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailMessage_RecipentId",
                table: "EmailMessage",
                column: "RecipentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailMessage");

            migrationBuilder.DropColumn(
                name: "Accepted",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "Blocked",
                table: "AspNetUsers");
        }
    }
}
