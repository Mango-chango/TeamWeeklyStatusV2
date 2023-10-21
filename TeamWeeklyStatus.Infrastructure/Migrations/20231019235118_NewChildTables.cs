using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamWeeklyStatus.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewChildTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoneThisWeek",
                table: "WeeklyStatuses");

            migrationBuilder.DropColumn(
                name: "PlanForNextWeek",
                table: "WeeklyStatuses");

            migrationBuilder.CreateTable(
                name: "DoneThisWeekTask",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WeeklyStatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoneThisWeekTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoneThisWeekTask_WeeklyStatuses_WeeklyStatusId",
                        column: x => x.WeeklyStatusId,
                        principalTable: "WeeklyStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanForNextWeekTask",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WeeklyStatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanForNextWeekTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanForNextWeekTask_WeeklyStatuses_WeeklyStatusId",
                        column: x => x.WeeklyStatusId,
                        principalTable: "WeeklyStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoneThisWeekTask_WeeklyStatusId",
                table: "DoneThisWeekTask",
                column: "WeeklyStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanForNextWeekTask_WeeklyStatusId",
                table: "PlanForNextWeekTask",
                column: "WeeklyStatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoneThisWeekTask");

            migrationBuilder.DropTable(
                name: "PlanForNextWeekTask");

            migrationBuilder.AddColumn<string>(
                name: "DoneThisWeek",
                table: "WeeklyStatuses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PlanForNextWeek",
                table: "WeeklyStatuses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
