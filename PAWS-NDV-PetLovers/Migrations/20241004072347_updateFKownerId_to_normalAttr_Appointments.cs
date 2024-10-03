using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAWS_NDV_PetLovers.Migrations
{
    /// <inheritdoc />
    public partial class updateFKownerId_to_normalAttr_Appointments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Owners_ownerId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_ownerId",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "ownerId",
                table: "Appointments",
                newName: "ownerId_holder");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ownerId_holder",
                table: "Appointments",
                newName: "ownerId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ownerId",
                table: "Appointments",
                column: "ownerId",
                unique: true,
                filter: "[ownerId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Owners_ownerId",
                table: "Appointments",
                column: "ownerId",
                principalTable: "Owners",
                principalColumn: "id");
        }
    }
}
