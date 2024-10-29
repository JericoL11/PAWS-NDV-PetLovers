using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAWS_NDV_PetLovers.Migrations
{
    /// <inheritdoc />
    public partial class bdate_userAccountRm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bdate",
                table: "UserAccounts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "bdate",
                table: "UserAccounts",
                type: "datetime2",
                nullable: true);
        }
    }
}
