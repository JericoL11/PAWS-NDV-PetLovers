using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAWS_NDV_PetLovers.Migrations
{
    /// <inheritdoc />
    public partial class AlterAdd_TotaLPayment_Diagnosis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "totalPayment",
                table: "Diagnostics",
                newName: "totalServicePayment");

            migrationBuilder.AddColumn<double>(
                name: "GrandTotal",
                table: "Diagnostics",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "totalProductPayment",
                table: "Diagnostics",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GrandTotal",
                table: "Diagnostics");

            migrationBuilder.DropColumn(
                name: "totalProductPayment",
                table: "Diagnostics");

            migrationBuilder.RenameColumn(
                name: "totalServicePayment",
                table: "Diagnostics",
                newName: "totalPayment");
        }
    }
}
