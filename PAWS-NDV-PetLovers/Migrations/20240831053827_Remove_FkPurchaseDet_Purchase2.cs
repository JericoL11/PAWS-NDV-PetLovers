using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAWS_NDV_PetLovers.Migrations
{
    /// <inheritdoc />
    public partial class Remove_FkPurchaseDet_Purchase2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diagnostics_Purchases_purchaseId",
                table: "Diagnostics");

            migrationBuilder.RenameColumn(
                name: "purchaseId",
                table: "Diagnostics",
                newName: "PurchaseNavpurchaseId");

            migrationBuilder.RenameIndex(
                name: "IX_Diagnostics_purchaseId",
                table: "Diagnostics",
                newName: "IX_Diagnostics_PurchaseNavpurchaseId");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "Purchases",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Diagnostics_Purchases_PurchaseNavpurchaseId",
                table: "Diagnostics",
                column: "PurchaseNavpurchaseId",
                principalTable: "Purchases",
                principalColumn: "purchaseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diagnostics_Purchases_PurchaseNavpurchaseId",
                table: "Diagnostics");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Purchases");

            migrationBuilder.RenameColumn(
                name: "PurchaseNavpurchaseId",
                table: "Diagnostics",
                newName: "purchaseId");

            migrationBuilder.RenameIndex(
                name: "IX_Diagnostics_PurchaseNavpurchaseId",
                table: "Diagnostics",
                newName: "IX_Diagnostics_purchaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Diagnostics_Purchases_purchaseId",
                table: "Diagnostics",
                column: "purchaseId",
                principalTable: "Purchases",
                principalColumn: "purchaseId");
        }
    }
}
