using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAWS_NDV_PetLovers.Migrations
{
    /// <inheritdoc />
    public partial class Remove_FkPurchaseDet_Purchase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseDetails_Purchases_purchaseId",
                table: "PurchaseDetails");

            migrationBuilder.DropColumn(
                name: "purchaseDetId",
                table: "Purchases");

            migrationBuilder.AlterColumn<int>(
                name: "purchaseId",
                table: "PurchaseDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseDetails_Purchases_purchaseId",
                table: "PurchaseDetails",
                column: "purchaseId",
                principalTable: "Purchases",
                principalColumn: "purchaseId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseDetails_Purchases_purchaseId",
                table: "PurchaseDetails");

            migrationBuilder.AddColumn<int>(
                name: "purchaseDetId",
                table: "Purchases",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "purchaseId",
                table: "PurchaseDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseDetails_Purchases_purchaseId",
                table: "PurchaseDetails",
                column: "purchaseId",
                principalTable: "Purchases",
                principalColumn: "purchaseId");
        }
    }
}
