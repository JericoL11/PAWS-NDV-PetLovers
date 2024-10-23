using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAWS_NDV_PetLovers.Migrations
{
    /// <inheritdoc />
    public partial class Update_StockAdjTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockAdjustments_Purchases_purchaseId",
                table: "StockAdjustments");

            migrationBuilder.RenameColumn(
                name: "purchaseId",
                table: "StockAdjustments",
                newName: "purchaseDetailsNavpurchaseDet_Id");

            migrationBuilder.RenameIndex(
                name: "IX_StockAdjustments_purchaseId",
                table: "StockAdjustments",
                newName: "IX_StockAdjustments_purchaseDetailsNavpurchaseDet_Id");

            migrationBuilder.AddColumn<int>(
                name: "purchaseDet_Id",
                table: "StockAdjustments",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StockAdjustments_PurchaseDetails_purchaseDetailsNavpurchaseDet_Id",
                table: "StockAdjustments",
                column: "purchaseDetailsNavpurchaseDet_Id",
                principalTable: "PurchaseDetails",
                principalColumn: "purchaseDet_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockAdjustments_PurchaseDetails_purchaseDetailsNavpurchaseDet_Id",
                table: "StockAdjustments");

            migrationBuilder.DropColumn(
                name: "purchaseDet_Id",
                table: "StockAdjustments");

            migrationBuilder.RenameColumn(
                name: "purchaseDetailsNavpurchaseDet_Id",
                table: "StockAdjustments",
                newName: "purchaseId");

            migrationBuilder.RenameIndex(
                name: "IX_StockAdjustments_purchaseDetailsNavpurchaseDet_Id",
                table: "StockAdjustments",
                newName: "IX_StockAdjustments_purchaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockAdjustments_Purchases_purchaseId",
                table: "StockAdjustments",
                column: "purchaseId",
                principalTable: "Purchases",
                principalColumn: "purchaseId");
        }
    }
}
