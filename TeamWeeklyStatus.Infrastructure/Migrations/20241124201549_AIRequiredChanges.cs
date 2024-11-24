using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamWeeklyStatus.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AIRequiredChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AIEngines",
                columns: table => new
                {
                    AIEngineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AIEngineName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AIEngines", x => x.AIEngineId);
                });

            migrationBuilder.CreateTable(
                name: "WeeklyStatusRichTexts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: true),
                    WeekStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DoneThisWeekContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlanForNextWeekContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Blockers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpcomingPTO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklyStatusRichTexts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeeklyStatusRichTexts_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WeeklyStatusRichTexts_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TeamAIConfigurations",
                columns: table => new
                {
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    AIEngineId = table.Column<int>(type: "int", nullable: false),
                    ApiKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApiUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamAIConfigurations", x => new { x.TeamId, x.AIEngineId });
                    table.ForeignKey(
                        name: "FK_TeamAIConfigurations_AIEngines_AIEngineId",
                        column: x => x.AIEngineId,
                        principalTable: "AIEngines",
                        principalColumn: "AIEngineId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamAIConfigurations_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeamAIConfigurations_AIEngineId",
                table: "TeamAIConfigurations",
                column: "AIEngineId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamAIConfigurations_TeamId",
                table: "TeamAIConfigurations",
                column: "TeamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyStatusRichTexts_MemberId",
                table: "WeeklyStatusRichTexts",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyStatusRichTexts_TeamId",
                table: "WeeklyStatusRichTexts",
                column: "TeamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamAIConfigurations");

            migrationBuilder.DropTable(
                name: "WeeklyStatusRichTexts");

            migrationBuilder.DropTable(
                name: "AIEngines");
        }
    }
}
