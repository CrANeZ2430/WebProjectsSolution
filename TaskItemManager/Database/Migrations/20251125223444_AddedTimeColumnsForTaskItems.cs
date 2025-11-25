using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskItemManager.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddedTimeColumnsForTaskItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DoneAt",
                schema: "taskItems",
                table: "TaskItems",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 11, 25, 22, 34, 44, 613, DateTimeKind.Utc).AddTicks(9524));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartedAt",
                schema: "taskItems",
                table: "TaskItems",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 11, 25, 22, 34, 44, 613, DateTimeKind.Utc).AddTicks(9190));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoneAt",
                schema: "taskItems",
                table: "TaskItems");

            migrationBuilder.DropColumn(
                name: "StartedAt",
                schema: "taskItems",
                table: "TaskItems");
        }
    }
}
