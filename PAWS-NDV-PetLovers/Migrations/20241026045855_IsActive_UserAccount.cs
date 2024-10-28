using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAWS_NDV_PetLovers.Migrations
{
    /// <inheritdoc />
    public partial class IsActive_UserAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "UserAccounts");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "UserAccounts",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "UserAccounts");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "UserAccounts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
