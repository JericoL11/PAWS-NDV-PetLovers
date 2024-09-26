using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAWS_NDV_PetLovers.Migrations
{
    /// <inheritdoc />
    public partial class alterFk_Billings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Billings_Diagnostics_Diagnostics",
                table: "Billings");

            migrationBuilder.DropForeignKey(
                name: "FK_Billings_Purchases_Purchases",
                table: "Billings");

            migrationBuilder.DropIndex(
                name: "IX_Billings_Diagnostics",
                table: "Billings");

            migrationBuilder.DropIndex(
                name: "IX_Billings_Purchases",
                table: "Billings");

            migrationBuilder.RenameColumn(
                name: "Purchases",
                table: "Billings",
                newName: "PurchaseId");

            migrationBuilder.RenameColumn(
                name: "Diagnostics",
                table: "Billings",
                newName: "DiagnosticsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PurchaseId",
                table: "Billings",
                newName: "Purchases");

            migrationBuilder.RenameColumn(
                name: "DiagnosticsId",
                table: "Billings",
                newName: "Diagnostics");

            migrationBuilder.CreateIndex(
                name: "IX_Billings_Diagnostics",
                table: "Billings",
                column: "Diagnostics");

            migrationBuilder.CreateIndex(
                name: "IX_Billings_Purchases",
                table: "Billings",
                column: "Purchases");

            migrationBuilder.AddForeignKey(
                name: "FK_Billings_Diagnostics_Diagnostics",
                table: "Billings",
                column: "Diagnostics",
                principalTable: "Diagnostics",
                principalColumn: "diagnostic_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Billings_Purchases_Purchases",
                table: "Billings",
                column: "Purchases",
                principalTable: "Purchases",
                principalColumn: "purchaseId");
        }
    }
}
