using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WakeMeUpOnLan.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    ApiKey = table.Column<string>(type: "TEXT", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WolTargets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TargetMacAddress = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WolTargets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApiUserWolTarget",
                columns: table => new
                {
                    AllowedTargetsId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ApiUsersId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiUserWolTarget", x => new { x.AllowedTargetsId, x.ApiUsersId });
                    table.ForeignKey(
                        name: "FK_ApiUserWolTarget_ApiUsers_ApiUsersId",
                        column: x => x.ApiUsersId,
                        principalTable: "ApiUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApiUserWolTarget_WolTargets_AllowedTargetsId",
                        column: x => x.AllowedTargetsId,
                        principalTable: "WolTargets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiUserWolTarget_ApiUsersId",
                table: "ApiUserWolTarget",
                column: "ApiUsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiUserWolTarget");

            migrationBuilder.DropTable(
                name: "ApiUsers");

            migrationBuilder.DropTable(
                name: "WolTargets");
        }
    }
}
