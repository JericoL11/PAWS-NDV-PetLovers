using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAWS_NDV_PetLovers.Migrations
{
    /// <inheritdoc />
    public partial class Remoce_StockAdj_PurchaseDet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "billingId",
                table: "StockAdjustments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustments_billingId",
                table: "StockAdjustments",
                column: "billingId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockAdjustments_Billings_billingId",
                table: "StockAdjustments",
                column: "billingId",
                principalTable: "Billings",
                principalColumn: "billingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockAdjustments_Billings_billingId",
                table: "StockAdjustments");

            migrationBuilder.DropIndex(
                name: "IX_StockAdjustments_billingId",
                table: "StockAdjustments");

            migrationBuilder.DropColumn(
                name: "billingId",
                table: "StockAdjustments");
        }
    }
}
