using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnalyticsCustomers.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderIngestFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Orders",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "QuantitySold",
                table: "Orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "QuantitySold",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Orders");
        }
    }
}
