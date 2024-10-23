using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAWS_NDV_PetLovers.Migrations
{
    /// <inheritdoc />
    public partial class addNav_StockAdj_Products : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StockAdjustments_productId",
                table: "StockAdjustments");

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustments_productId",
                table: "StockAdjustments",
                column: "productId",
                unique: true,
                filter: "[productId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StockAdjustments_productId",
                table: "StockAdjustments");

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustments_productId",
                table: "StockAdjustments",
                column: "productId");
        }
    }
}
