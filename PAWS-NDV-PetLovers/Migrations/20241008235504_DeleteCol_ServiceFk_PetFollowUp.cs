using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAWS_NDV_PetLovers.Migrations
{
    /// <inheritdoc />
    public partial class DeleteCol_ServiceFk_PetFollowUp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PetFollowUps_Services_serviceId",
                table: "PetFollowUps");

            migrationBuilder.DropIndex(
                name: "IX_PetFollowUps_serviceId",
                table: "PetFollowUps");

            migrationBuilder.DropColumn(
                name: "serviceId",
                table: "PetFollowUps");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "serviceId",
                table: "PetFollowUps",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PetFollowUps_serviceId",
                table: "PetFollowUps",
                column: "serviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_PetFollowUps_Services_serviceId",
                table: "PetFollowUps",
                column: "serviceId",
                principalTable: "Services",
                principalColumn: "serviceId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
