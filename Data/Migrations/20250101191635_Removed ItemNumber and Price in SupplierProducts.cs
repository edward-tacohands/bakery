using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bageri.api.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedItemNumberandPriceinSupplierProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemNumber",
                table: "SupplierProducts");

            migrationBuilder.DropColumn(
                name: "PricePerKg",
                table: "SupplierProducts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ItemNumber",
                table: "SupplierProducts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PricePerKg",
                table: "SupplierProducts",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
