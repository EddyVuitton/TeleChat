using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeleChat.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddMessageType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MessageType",
                columns: new[] { "Id", "DefaultStyle", "Name" },
                values: new object[] { 3, "max-width: 450px; max-height: 350px;", "Image" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MessageType",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
