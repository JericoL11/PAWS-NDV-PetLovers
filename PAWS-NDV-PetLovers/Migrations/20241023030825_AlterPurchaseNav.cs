using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAWS_NDV_PetLovers.Migrations
{
    /// <inheritdoc />
    public partial class AlterPurchaseNav : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockAdjustments_PurchaseDetails_purchaseDetailsNavpurchaseDet_Id",
                table: "StockAdjustments");

            migrationBuilder.DropIndex(
                name: "IX_StockAdjustments_purchaseDetailsNavpurchaseDet_Id",
                table: "StockAdjustments");

            migrationBuilder.DropColumn(
                name: "purchaseDetailsNavpurchaseDet_Id",
                table: "StockAdjustments");

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustments_purchaseDet_Id",
                table: "StockAdjustments",
                column: "purchaseDet_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StockAdjustments_PurchaseDetails_purchaseDet_Id",
                table: "StockAdjustments",
                column: "purchaseDet_Id",
                principalTable: "PurchaseDetails",
                principalColumn: "purchaseDet_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockAdjustments_PurchaseDetails_purchaseDet_Id",
                table: "StockAdjustments");

            migrationBuilder.DropIndex(
                name: "IX_StockAdjustments_purchaseDet_Id",
                table: "StockAdjustments");

            migrationBuilder.AddColumn<int>(
                name: "purchaseDetailsNavpurchaseDet_Id",
                table: "StockAdjustments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustments_purchaseDetailsNavpurchaseDet_Id",
                table: "StockAdjustments",
                column: "purchaseDetailsNavpurchaseDet_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StockAdjustments_PurchaseDetails_purchaseDetailsNavpurchaseDet_Id",
                table: "StockAdjustments",
                column: "purchaseDetailsNavpurchaseDet_Id",
                principalTable: "PurchaseDetails",
                principalColumn: "purchaseDet_Id");
        }
    }
}
