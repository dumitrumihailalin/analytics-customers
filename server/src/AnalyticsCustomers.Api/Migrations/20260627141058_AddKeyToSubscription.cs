using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnalyticsCustomers.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddKeyToSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Column was added manually in pgAdmin — use IF NOT EXISTS to be idempotent
            migrationBuilder.Sql(
                "ALTER TABLE \"Subscriptions\" ADD COLUMN IF NOT EXISTS key text;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "key",
                table: "Subscriptions");
        }
    }
}
