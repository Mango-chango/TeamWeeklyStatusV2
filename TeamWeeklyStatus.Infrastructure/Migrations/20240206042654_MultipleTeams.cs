using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamWeeklyStatus.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MultipleTeams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "WeeklyStatuses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndActiveDate",
                table: "TeamMembers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAdminMember",
                table: "TeamMembers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartActiveDate",
                table: "TeamMembers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyStatuses_TeamId",
                table: "WeeklyStatuses",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeeklyStatuses_Teams_TeamId",
                table: "WeeklyStatuses",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeeklyStatuses_Teams_TeamId",
                table: "WeeklyStatuses");

            migrationBuilder.DropIndex(
                name: "IX_WeeklyStatuses_TeamId",
                table: "WeeklyStatuses");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "WeeklyStatuses");

            migrationBuilder.DropColumn(
                name: "EndActiveDate",
                table: "TeamMembers");

            migrationBuilder.DropColumn(
                name: "IsAdminMember",
                table: "TeamMembers");

            migrationBuilder.DropColumn(
                name: "StartActiveDate",
                table: "TeamMembers");
        }
    }
}
