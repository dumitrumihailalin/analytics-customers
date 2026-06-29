using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnalyticsCustomers.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddStoreAndRefactorRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Analytics_Users_StoreId",
                table: "Analytics");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "Users");

            migrationBuilder.AddColumn<Guid>(
                name: "StoreId",
                table: "SubscriptionKeys",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StoreName = table.Column<string>(type: "text", nullable: false),
                    Street = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Website = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stores_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionKeys_StoreId",
                table: "SubscriptionKeys",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_OwnerId",
                table: "Stores",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Analytics_Stores_StoreId",
                table: "Analytics",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubscriptionKeys_Stores_StoreId",
                table: "SubscriptionKeys",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Analytics_Stores_StoreId",
                table: "Analytics");

            migrationBuilder.DropForeignKey(
                name: "FK_SubscriptionKeys_Stores_StoreId",
                table: "SubscriptionKeys");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropIndex(
                name: "IX_SubscriptionKeys_StoreId",
                table: "SubscriptionKeys");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "SubscriptionKeys");

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Analytics_Users_StoreId",
                table: "Analytics",
                column: "StoreId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
