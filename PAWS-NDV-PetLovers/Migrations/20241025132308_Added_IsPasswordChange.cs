using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAWS_NDV_PetLovers.Migrations
{
    /// <inheritdoc />
    public partial class Added_IsPasswordChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPasswordChanged",
                table: "Staff",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPasswordChanged",
                table: "Admins",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPasswordChanged",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "IsPasswordChanged",
                table: "Admins");
        }
    }
}
