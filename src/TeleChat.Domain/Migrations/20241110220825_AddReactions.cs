using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TeleChat.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddReactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reaction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MessageReaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageId = table.Column<int>(type: "int", nullable: false),
                    ReactionId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageReaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageReaction_Message_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Message",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MessageReaction_Reaction_ReactionId",
                        column: x => x.ReactionId,
                        principalTable: "Reaction",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MessageReaction_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Reaction",
                columns: new[] { "Id", "Value" },
                values: new object[,]
                {
                    { 1, "👍" },
                    { 2, "👎" },
                    { 3, "🧡" },
                    { 4, "🔥" },
                    { 5, "🥰" },
                    { 6, "👏" },
                    { 7, "😁" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MessageReaction_MessageId",
                table: "MessageReaction",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReaction_ReactionId",
                table: "MessageReaction",
                column: "ReactionId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReaction_UserId",
                table: "MessageReaction",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageReaction");

            migrationBuilder.DropTable(
                name: "Reaction");

            migrationBuilder.UpdateData(
                table: "GroupChat",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Guid" },
                values: new object[] { new DateTime(2024, 10, 8, 21, 29, 1, 462, DateTimeKind.Local).AddTicks(458), new Guid("2a0f031d-0185-494a-b2eb-caa8349096ff") });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2024, 10, 8, 21, 29, 1, 461, DateTimeKind.Local).AddTicks(4718));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2024, 10, 8, 21, 29, 1, 461, DateTimeKind.Local).AddTicks(4789));

            migrationBuilder.UpdateData(
                table: "UserGroupChat",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2024, 10, 8, 21, 29, 1, 462, DateTimeKind.Local).AddTicks(3168));

            migrationBuilder.UpdateData(
                table: "UserGroupChat",
                keyColumn: "Id",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2024, 10, 8, 21, 29, 1, 462, DateTimeKind.Local).AddTicks(3199));
        }
    }
}
