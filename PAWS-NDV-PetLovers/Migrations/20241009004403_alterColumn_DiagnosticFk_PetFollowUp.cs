using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAWS_NDV_PetLovers.Migrations
{
    /// <inheritdoc />
    public partial class alterColumn_DiagnosticFk_PetFollowUp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // If you had previously altered the schema, ensure you revert any unnecessary changes.
            // Adding the foreign key back if it was dropped before.

            migrationBuilder.DropForeignKey(
                name: "FK_PetFollowUps_Diagnostics_Diagnosticsdiagnostic_Id",
                table: "PetFollowUps");

            // If there was a previous column for diagnostic_Id, you can drop it
            migrationBuilder.DropColumn(
                name: "diagnostic_Id",
                table: "PetFollowUps");

            // Rename or keep columns as necessary, making sure to not change the Id identity column
            migrationBuilder.RenameColumn(
                name: "Diagnosticsdiagnostic_Id",
                table: "PetFollowUps",
                newName: "Diagnostics");

            // Recreate foreign key relationships as necessary
            migrationBuilder.AddForeignKey(
                name: "FK_PetFollowUps_Diagnostics_Diagnostics",
                table: "PetFollowUps",
                column: "Diagnostics",
                principalTable: "Diagnostics",
                principalColumn: "diagnostic_Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Define how to revert the migration, including foreign keys and columns
            migrationBuilder.DropForeignKey(
                name: "FK_PetFollowUps_Diagnostics_Diagnostics",
                table: "PetFollowUps");

            migrationBuilder.RenameColumn(
                name: "Diagnostics",
                table: "PetFollowUps",
                newName: "Diagnosticsdiagnostic_Id");

            migrationBuilder.AddColumn<int>(
                name: "diagnostic_Id",
                table: "PetFollowUps",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_PetFollowUps_Diagnostics_Diagnosticsdiagnostic_Id",
                table: "PetFollowUps",
                column: "Diagnosticsdiagnostic_Id",
                principalTable: "Diagnostics",
                principalColumn: "diagnostic_Id");
        }
    }
}
