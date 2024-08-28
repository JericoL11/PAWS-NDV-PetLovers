using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAWS_NDV_PetLovers.Migrations
{
    /// <inheritdoc />
    public partial class AlterNavi_Diagnostics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diagnostics_Purchases_Purchase_Id",
                table: "Diagnostics");

            migrationBuilder.DropIndex(
                name: "IX_Diagnostics_Purchase_Id",
                table: "Diagnostics");

            migrationBuilder.DropColumn(
                name: "Purchase_Id",
                table: "Diagnostics");

            migrationBuilder.AddColumn<int>(
                name: "Diagnosticsdiagnostic_Id",
                table: "Purchases",
                type: "int",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "Purchase_Id",
                table: "Diagnostics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Diagnostics_Purchase_Id",
                table: "Diagnostics",
                column: "Purchase_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Diagnostics_Purchases_Purchase_Id",
                table: "Diagnostics",
                column: "Purchase_Id",
                principalTable: "Purchases",
                principalColumn: "purchaseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
