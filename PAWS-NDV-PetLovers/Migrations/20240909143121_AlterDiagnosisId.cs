using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAWS_NDV_PetLovers.Migrations
{
    /// <inheritdoc />
    public partial class AlterDiagnosisId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "diagnosisId",
                table: "Purchases");

            migrationBuilder.AddColumn<int>(
                name: "diagnosisId_holder",
                table: "Purchases",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "diagnosisId_holder",
                table: "Purchases");

            migrationBuilder.AddColumn<int>(
                name: "diagnosisId",
                table: "Purchases",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
