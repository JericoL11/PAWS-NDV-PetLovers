using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAWS_NDV_PetLovers.Migrations
{
    /// <inheritdoc />
    public partial class Stock_Adjustments_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockAdjustments",
                columns: table => new
                {
                    stockAdj_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "6010, 1"),
                    source = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    stock = table.Column<int>(type: "int", nullable: false),
                    productId = table.Column<int>(type: "int", nullable: true),
                    purchaseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockAdjustments", x => x.stockAdj_Id);
                    table.ForeignKey(
                        name: "FK_StockAdjustments_Products_productId",
                        column: x => x.productId,
                        principalTable: "Products",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_StockAdjustments_Purchases_purchaseId",
                        column: x => x.purchaseId,
                        principalTable: "Purchases",
                        principalColumn: "purchaseId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustments_productId",
                table: "StockAdjustments",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustments_purchaseId",
                table: "StockAdjustments",
                column: "purchaseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockAdjustments");
        }
    }
}
