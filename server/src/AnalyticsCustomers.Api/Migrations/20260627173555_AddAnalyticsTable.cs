using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnalyticsCustomers.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddAnalyticsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Analytics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StoreId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<string>(type: "text", nullable: false),
                    CategoryId = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    QuantitySold = table.Column<int>(type: "integer", nullable: false),
                    Stock = table.Column<int>(type: "integer", nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Analytics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Analytics_Users_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Analytics_StoreId_ProductId",
                table: "Analytics",
                columns: new[] { "StoreId", "ProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_Analytics_StoreId_RecordedAt",
                table: "Analytics",
                columns: new[] { "StoreId", "RecordedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Analytics");
        }
    }
}
