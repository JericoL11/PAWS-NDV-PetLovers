using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAWS_NDV_PetLovers.Migrations
{
    /// <inheritdoc />
    public partial class Remove_ProductPrice_Diagnostics2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Diagnostics_Diagnosticsdiagnostic_Id",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_Diagnosticsdiagnostic_Id",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "Diagnosticsdiagnostic_Id",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "totalProductPayment",
                table: "Diagnostics");

            migrationBuilder.RenameColumn(
                name: "totalPayment",
                table: "Purchases",
                newName: "totalProductPayment");

            migrationBuilder.AddColumn<int>(
                name: "purchaseId",
                table: "Diagnostics",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Diagnostics_purchaseId",
                table: "Diagnostics",
                column: "purchaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Diagnostics_Purchases_purchaseId",
                table: "Diagnostics",
                column: "purchaseId",
                principalTable: "Purchases",
                principalColumn: "purchaseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diagnostics_Purchases_purchaseId",
                table: "Diagnostics");

            migrationBuilder.DropIndex(
                name: "IX_Diagnostics_purchaseId",
                table: "Diagnostics");

            migrationBuilder.DropColumn(
                name: "purchaseId",
                table: "Diagnostics");

            migrationBuilder.RenameColumn(
                name: "totalProductPayment",
                table: "Purchases",
                newName: "totalPayment");

            migrationBuilder.AddColumn<int>(
                name: "Diagnosticsdiagnostic_Id",
                table: "Purchases",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "totalProductPayment",
                table: "Diagnostics",
                type: "float",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_Diagnosticsdiagnostic_Id",
                table: "Purchases",
                column: "Diagnosticsdiagnostic_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Diagnostics_Diagnosticsdiagnostic_Id",
                table: "Purchases",
                column: "Diagnosticsdiagnostic_Id",
                principalTable: "Diagnostics",
                principalColumn: "diagnostic_Id");
        }
    }
}
