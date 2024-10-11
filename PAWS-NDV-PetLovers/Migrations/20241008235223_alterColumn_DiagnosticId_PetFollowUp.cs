using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAWS_NDV_PetLovers.Migrations
{
    /// <inheritdoc />
    public partial class alterColumn_DiagnosticId_PetFollowUp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "diagnosticId",
                table: "PetFollowUps",
                newName: "diagnostic_Id");

            migrationBuilder.AddColumn<int>(
                name: "Diagnosticsdiagnostic_Id",
                table: "PetFollowUps",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PetFollowUps_Diagnosticsdiagnostic_Id",
                table: "PetFollowUps",
                column: "Diagnosticsdiagnostic_Id");

            migrationBuilder.CreateIndex(
                name: "IX_PetFollowUps_serviceId",
                table: "PetFollowUps",
                column: "serviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_PetFollowUps_Diagnostics_Diagnosticsdiagnostic_Id",
                table: "PetFollowUps",
                column: "Diagnosticsdiagnostic_Id",
                principalTable: "Diagnostics",
                principalColumn: "diagnostic_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PetFollowUps_Services_serviceId",
                table: "PetFollowUps",
                column: "serviceId",
                principalTable: "Services",
                principalColumn: "serviceId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PetFollowUps_Diagnostics_Diagnosticsdiagnostic_Id",
                table: "PetFollowUps");

            migrationBuilder.DropForeignKey(
                name: "FK_PetFollowUps_Services_serviceId",
                table: "PetFollowUps");

            migrationBuilder.DropIndex(
                name: "IX_PetFollowUps_Diagnosticsdiagnostic_Id",
                table: "PetFollowUps");

            migrationBuilder.DropIndex(
                name: "IX_PetFollowUps_serviceId",
                table: "PetFollowUps");

            migrationBuilder.DropColumn(
                name: "Diagnosticsdiagnostic_Id",
                table: "PetFollowUps");

            migrationBuilder.RenameColumn(
                name: "diagnostic_Id",
                table: "PetFollowUps",
                newName: "diagnosticId");
        }
    }
}
