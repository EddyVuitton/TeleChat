using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TeleChat.Domain.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "GroupChat",
                columns: new[] { "Id", "Created", "Guid", "Name" },
                values: new object[] { 1, new DateTime(2024, 8, 28, 10, 53, 48, 72, DateTimeKind.Utc).AddTicks(7316), new Guid("97831b26-33a2-4967-a124-8c61d9e3e3cb"), "Domyślna grupa" });

            migrationBuilder.InsertData(
                table: "MessageType",
                columns: new[] { "Id", "DefaultStyle", "Name" },
                values: new object[,]
                {
                    { 1, null, "PlainText" },
                    { 2, "max-width: 200px; max-height: 200px;", "GIF" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GroupChat",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MessageType",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MessageType",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
