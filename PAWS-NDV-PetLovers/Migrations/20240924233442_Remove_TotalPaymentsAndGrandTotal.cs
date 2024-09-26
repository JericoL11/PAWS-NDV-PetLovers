using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAWS_NDV_PetLovers.Migrations
{
    /// <inheritdoc />
    public partial class Remove_TotalPaymentsAndGrandTotal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "totalProductPayment",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "grandTotal",
                table: "Diagnostics");

            migrationBuilder.DropColumn(
                name: "totalServicePayment",
                table: "Diagnostics");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "totalProductPayment",
                table: "Purchases",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "grandTotal",
                table: "Diagnostics",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "totalServicePayment",
                table: "Diagnostics",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
