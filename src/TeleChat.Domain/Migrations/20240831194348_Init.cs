using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TeleChat.Domain.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroupChat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupChat", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MessageType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    DefaultStyle = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Login = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Text = table.Column<string>(type: "text", nullable: false),
                    MessageTypeId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    GroupChatId = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Message_GroupChat_GroupChatId",
                        column: x => x.GroupChatId,
                        principalTable: "GroupChat",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Message_MessageType_MessageTypeId",
                        column: x => x.MessageTypeId,
                        principalTable: "MessageType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Message_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserGroupChat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    GroupChatId = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroupChat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserGroupChat_GroupChat_GroupChatId",
                        column: x => x.GroupChatId,
                        principalTable: "GroupChat",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserGroupChat_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "GroupChat",
                columns: new[] { "Id", "Created", "Guid", "Name" },
                values: new object[] { 1, new DateTime(2024, 8, 31, 19, 43, 47, 67, DateTimeKind.Utc).AddTicks(9182), new Guid("dd1cdaef-ee33-4cdc-8fa1-5285627aeccf"), "Domyślna grupa" });

            migrationBuilder.InsertData(
                table: "MessageType",
                columns: new[] { "Id", "DefaultStyle", "Name" },
                values: new object[,]
                {
                    { 1, null, "PlainText" },
                    { 2, "max-width: 200px; max-height: 200px;", "GIF" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Created", "Login", "Name", "Password" },
                values: new object[] { 1, new DateTime(2024, 8, 31, 19, 43, 47, 67, DateTimeKind.Utc).AddTicks(3212), "demo", "Demo", "739136E95F37FEE4B526F9C20C3E9DA6-11FB2578105BEA2A05F32D9CA5DFD27C" });

            migrationBuilder.InsertData(
                table: "UserGroupChat",
                columns: new[] { "Id", "Created", "GroupChatId", "UserId" },
                values: new object[] { 1, new DateTime(2024, 8, 31, 19, 43, 47, 68, DateTimeKind.Utc).AddTicks(2915), 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Message_GroupChatId",
                table: "Message",
                column: "GroupChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_MessageTypeId",
                table: "Message",
                column: "MessageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_UserId",
                table: "Message",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupChat_GroupChatId",
                table: "UserGroupChat",
                column: "GroupChatId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupChat_UserId",
                table: "UserGroupChat",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "UserGroupChat");

            migrationBuilder.DropTable(
                name: "MessageType");

            migrationBuilder.DropTable(
                name: "GroupChat");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
