using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskItemManager.Database.Migrations
{
    /// <inheritdoc />
    public partial class UsersEmailUniqueness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartedAt",
                schema: "taskItems",
                table: "TaskItems",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 11, 26, 12, 45, 23, 26, DateTimeKind.Utc).AddTicks(7486),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 11, 25, 22, 34, 44, 613, DateTimeKind.Utc).AddTicks(9190));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DoneAt",
                schema: "taskItems",
                table: "TaskItems",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 11, 26, 12, 45, 23, 26, DateTimeKind.Utc).AddTicks(7833),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 11, 25, 22, 34, 44, 613, DateTimeKind.Utc).AddTicks(9524));

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "taskItems",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                schema: "taskItems",
                table: "Users");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartedAt",
                schema: "taskItems",
                table: "TaskItems",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 11, 25, 22, 34, 44, 613, DateTimeKind.Utc).AddTicks(9190),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 11, 26, 12, 45, 23, 26, DateTimeKind.Utc).AddTicks(7486));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DoneAt",
                schema: "taskItems",
                table: "TaskItems",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 11, 25, 22, 34, 44, 613, DateTimeKind.Utc).AddTicks(9524),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 11, 26, 12, 45, 23, 26, DateTimeKind.Utc).AddTicks(7833));
        }
    }
}
