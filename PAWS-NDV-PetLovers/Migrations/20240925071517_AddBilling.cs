using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAWS_NDV_PetLovers.Migrations
{
    /// <inheritdoc />
    public partial class AddBilling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Billings",
                columns: table => new
                {
                    billingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "80000, 1"),
                    Diagnostics = table.Column<int>(type: "int", nullable: true),
                    Purchases = table.Column<int>(type: "int", nullable: true),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    grandTotal = table.Column<double>(type: "float", nullable: true),
                    cashReceive = table.Column<double>(type: "float", nullable: true),
                    changeAmount = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Billings", x => x.billingId);
                    table.ForeignKey(
                        name: "FK_Billings_Diagnostics_Diagnostics",
                        column: x => x.Diagnostics,
                        principalTable: "Diagnostics",
                        principalColumn: "diagnostic_Id");
                    table.ForeignKey(
                        name: "FK_Billings_Purchases_Purchases",
                        column: x => x.Purchases,
                        principalTable: "Purchases",
                        principalColumn: "purchaseId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Billings_Diagnostics",
                table: "Billings",
                column: "Diagnostics");

            migrationBuilder.CreateIndex(
                name: "IX_Billings_Purchases",
                table: "Billings",
                column: "Purchases");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Billings");
        }
    }
}
