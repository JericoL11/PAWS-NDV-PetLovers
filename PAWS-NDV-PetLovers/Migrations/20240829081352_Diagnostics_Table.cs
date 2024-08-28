using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAWS_NDV_PetLovers.Migrations
{
    /// <inheritdoc />
    public partial class Diagnostics_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Purchases",
                columns: table => new
                {
                    purchaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "60000, 1"),
                    purchaseDetId = table.Column<int>(type: "int", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    totalPayment = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => x.purchaseId);
                });

            migrationBuilder.CreateTable(
                name: "Diagnostics",
                columns: table => new
                {
                    diagnostic_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "30000, 1"),
                    petId = table.Column<int>(type: "int", nullable: false),
                    weight = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    totalPayment = table.Column<double>(type: "float", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Purchase_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diagnostics", x => x.diagnostic_Id);
                    table.ForeignKey(
                        name: "FK_Diagnostics_Pets_petId",
                        column: x => x.petId,
                        principalTable: "Pets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Diagnostics_Purchases_Purchase_Id",
                        column: x => x.Purchase_Id,
                        principalTable: "Purchases",
                        principalColumn: "purchaseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseDetails",
                columns: table => new
                {
                    purchaseDet_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "160000, 1"),
                    productId = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    sellingPrice = table.Column<double>(type: "float", nullable: false),
                    purchaseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseDetails", x => x.purchaseDet_Id);
                    table.ForeignKey(
                        name: "FK_PurchaseDetails_Products_productId",
                        column: x => x.productId,
                        principalTable: "Products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseDetails_Purchases_purchaseId",
                        column: x => x.purchaseId,
                        principalTable: "Purchases",
                        principalColumn: "purchaseId");
                });

            migrationBuilder.CreateTable(
                name: "DiagnosticDetails",
                columns: table => new
                {
                    diagnosticDet_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "300000, 1"),
                    details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    diagnosticsId = table.Column<int>(type: "int", nullable: false),
                    serviceId = table.Column<int>(type: "int", nullable: false),
                    servicePrice = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiagnosticDetails", x => x.diagnosticDet_Id);
                    table.ForeignKey(
                        name: "FK_DiagnosticDetails_Diagnostics_diagnosticsId",
                        column: x => x.diagnosticsId,
                        principalTable: "Diagnostics",
                        principalColumn: "diagnostic_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiagnosticDetails_Services_serviceId",
                        column: x => x.serviceId,
                        principalTable: "Services",
                        principalColumn: "serviceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiagnosticDetails_diagnosticsId",
                table: "DiagnosticDetails",
                column: "diagnosticsId");

            migrationBuilder.CreateIndex(
                name: "IX_DiagnosticDetails_serviceId",
                table: "DiagnosticDetails",
                column: "serviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnostics_petId",
                table: "Diagnostics",
                column: "petId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnostics_Purchase_Id",
                table: "Diagnostics",
                column: "Purchase_Id");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseDetails_productId",
                table: "PurchaseDetails",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseDetails_purchaseId",
                table: "PurchaseDetails",
                column: "purchaseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiagnosticDetails");

            migrationBuilder.DropTable(
                name: "PurchaseDetails");

            migrationBuilder.DropTable(
                name: "Diagnostics");

            migrationBuilder.DropTable(
                name: "Purchases");
        }
    }
}
