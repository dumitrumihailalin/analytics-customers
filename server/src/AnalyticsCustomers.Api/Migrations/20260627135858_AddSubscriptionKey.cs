using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnalyticsCustomers.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscriptionKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubscriptionKeys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Key = table.Column<string>(type: "text", nullable: false),
                    IssuedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionKeys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubscriptionKeys_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionKeys_Key",
                table: "SubscriptionKeys",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionKeys_UserId_IsActive",
                table: "SubscriptionKeys",
                columns: new[] { "UserId", "IsActive" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubscriptionKeys");
        }
    }
}
