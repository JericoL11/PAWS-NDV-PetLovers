using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAWS_NDV_PetLovers.Migrations
{
    /// <inheritdoc />
    public partial class Appointment_Remarks_Transfer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "remarks",
                table: "AppointmentDetails");

            migrationBuilder.AddColumn<string>(
                name: "remarks",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "remarks",
                table: "Appointments");

            migrationBuilder.AddColumn<string>(
                name: "remarks",
                table: "AppointmentDetails",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
