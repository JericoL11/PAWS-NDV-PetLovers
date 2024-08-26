using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAWS_NDV_PetLovers.Migrations
{
    /// <inheritdoc />
    public partial class AddCol_Vaccine_DiagnosticDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "vaccineName",
                table: "DiagnosticDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "vaccineName",
                table: "DiagnosticDetails");
        }
    }
}
