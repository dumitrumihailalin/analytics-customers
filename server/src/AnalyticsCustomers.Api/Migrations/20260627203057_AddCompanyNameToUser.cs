using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnalyticsCustomers.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyNameToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Users_OwnerId",
                table: "Stores");

            migrationBuilder.DropForeignKey(
                name: "FK_SubscriptionKeys_Stores_StoreId",
                table: "SubscriptionKeys");

            migrationBuilder.DropForeignKey(
                name: "FK_SubscriptionKeys_Users_UserId",
                table: "SubscriptionKeys");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Users_UserId",
                table: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_UserId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_SubscriptionKeys_StoreId",
                table: "SubscriptionKeys");

            migrationBuilder.DropIndex(
                name: "IX_SubscriptionKeys_UserId_IsActive",
                table: "SubscriptionKeys");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Plan",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "key",
                table: "Subscriptions");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "Users",
                newName: "Level");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Subscriptions",
                newName: "PlanId");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Stores",
                newName: "OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_Stores_OwnerId",
                table: "Stores",
                newName: "IX_Stores_OrganizationId");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                table: "Subscriptions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Stores",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "ApiKeys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StoreId = table.Column<Guid>(type: "uuid", nullable: false),
                    KeyHash = table.Column<string>(type: "text", nullable: false),
                    Prefix = table.Column<string>(type: "text", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUsedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiKeys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApiKeys_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_OrganizationId",
                table: "Users",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_OrganizationId",
                table: "Subscriptions",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_KeyHash",
                table: "ApiKeys",
                column: "KeyHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_StoreId",
                table: "ApiKeys",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Organizations_OrganizationId",
                table: "Stores",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Organizations_OrganizationId",
                table: "Subscriptions",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Organizations_OrganizationId",
                table: "Users",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Organizations_OrganizationId",
                table: "Stores");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Organizations_OrganizationId",
                table: "Subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Organizations_OrganizationId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "ApiKeys");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Users_OrganizationId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_OrganizationId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Stores");

            migrationBuilder.RenameColumn(
                name: "Level",
                table: "Users",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "PlanId",
                table: "Subscriptions",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "Stores",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Stores_OrganizationId",
                table: "Stores",
                newName: "IX_Stores_OwnerId");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Plan",
                table: "Subscriptions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "key",
                table: "Subscriptions",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OrderReference = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    ProductCategory = table.Column<string>(type: "text", nullable: true),
                    QuantitySold = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Stock = table.Column<int>(type: "integer", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    WeekNumber = table.Column<int>(type: "integer", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_UserId",
                table: "Subscriptions",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionKeys_StoreId",
                table: "SubscriptionKeys",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionKeys_UserId_IsActive",
                table: "SubscriptionKeys",
                columns: new[] { "UserId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId_WeekNumber_Year",
                table: "Orders",
                columns: new[] { "UserId", "WeekNumber", "Year" });

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Users_OwnerId",
                table: "Stores",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubscriptionKeys_Stores_StoreId",
                table: "SubscriptionKeys",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubscriptionKeys_Users_UserId",
                table: "SubscriptionKeys",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Users_UserId",
                table: "Subscriptions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
