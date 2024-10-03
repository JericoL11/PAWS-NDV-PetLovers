using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAWS_NDV_PetLovers.Migrations
{
    /// <inheritdoc />
    public partial class AddNAvAppointments_Owners : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appointments_ownerId",
                table: "Appointments");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ownerId",
                table: "Appointments",
                column: "ownerId",
                unique: true,
                filter: "[ownerId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appointments_ownerId",
                table: "Appointments");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ownerId",
                table: "Appointments",
                column: "ownerId");
        }
    }
}
