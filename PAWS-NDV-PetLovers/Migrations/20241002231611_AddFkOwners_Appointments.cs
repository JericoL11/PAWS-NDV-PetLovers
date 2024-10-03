using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAWS_NDV_PetLovers.Migrations
{
    /// <inheritdoc />
    public partial class AddFkOwners_Appointments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ownerId",
                table: "Appointments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ownerId",
                table: "Appointments",
                column: "ownerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Owners_ownerId",
                table: "Appointments",
                column: "ownerId",
                principalTable: "Owners",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Owners_ownerId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_ownerId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "ownerId",
                table: "Appointments");
        }
    }
}
