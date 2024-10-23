using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAWS_NDV_PetLovers.Migrations
{
    /// <inheritdoc />
    public partial class AlterStockAdj_BillingId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockAdjustments_Billings_billingId",
                table: "StockAdjustments");

            migrationBuilder.DropForeignKey(
                name: "FK_StockAdjustments_PurchaseDetails_purchaseDet_Id",
                table: "StockAdjustments");

            migrationBuilder.DropIndex(
                name: "IX_StockAdjustments_purchaseDet_Id",
                table: "StockAdjustments");

            migrationBuilder.RenameColumn(
                name: "purchaseDet_Id",
                table: "StockAdjustments",
                newName: "billing_Id");

            migrationBuilder.RenameColumn(
                name: "billingId",
                table: "StockAdjustments",
                newName: "billingNavbillingId");

            migrationBuilder.RenameIndex(
                name: "IX_StockAdjustments_billingId",
                table: "StockAdjustments",
                newName: "IX_StockAdjustments_billingNavbillingId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockAdjustments_Billings_billingNavbillingId",
                table: "StockAdjustments",
                column: "billingNavbillingId",
                principalTable: "Billings",
                principalColumn: "billingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockAdjustments_Billings_billingNavbillingId",
                table: "StockAdjustments");

            migrationBuilder.RenameColumn(
                name: "billing_Id",
                table: "StockAdjustments",
                newName: "purchaseDet_Id");

            migrationBuilder.RenameColumn(
                name: "billingNavbillingId",
                table: "StockAdjustments",
                newName: "billingId");

            migrationBuilder.RenameIndex(
                name: "IX_StockAdjustments_billingNavbillingId",
                table: "StockAdjustments",
                newName: "IX_StockAdjustments_billingId");

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustments_purchaseDet_Id",
                table: "StockAdjustments",
                column: "purchaseDet_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StockAdjustments_Billings_billingId",
                table: "StockAdjustments",
                column: "billingId",
                principalTable: "Billings",
                principalColumn: "billingId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockAdjustments_PurchaseDetails_purchaseDet_Id",
                table: "StockAdjustments",
                column: "purchaseDet_Id",
                principalTable: "PurchaseDetails",
                principalColumn: "purchaseDet_Id");
        }
    }
}
