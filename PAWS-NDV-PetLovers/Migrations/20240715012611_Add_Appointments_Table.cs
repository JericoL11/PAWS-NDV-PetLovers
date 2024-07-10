using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAWS_NDV_PetLovers.Migrations
{
    /// <inheritdoc />
    public partial class Add_Appointments_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    AppointId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "100, 1"),
                    OwnerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.AppointId);
                    table.ForeignKey(
                        name: "FK_Appointments_Owners_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Owners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    serviceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "500, 1"),
                    serviceName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    serviceCharge = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.serviceId);
                });

            migrationBuilder.CreateTable(
                name: "AppointmentDetails",
                columns: table => new
                {
                    AppointId_details = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "12000, 1"),
                    AppointId = table.Column<int>(type: "int", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    serviceID = table.Column<int>(type: "int", nullable: false),
                    petID = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentDetails", x => x.AppointId_details);
                    table.ForeignKey(
                        name: "FK_AppointmentDetails_Appointments_AppointId",
                        column: x => x.AppointId,
                        principalTable: "Appointments",
                        principalColumn: "AppointId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppointmentDetails_Pets_petID",
                        column: x => x.petID,
                        principalTable: "Pets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppointmentDetails_Services_serviceID",
                        column: x => x.serviceID,
                        principalTable: "Services",
                        principalColumn: "serviceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetails_AppointId",
                table: "AppointmentDetails",
                column: "AppointId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetails_petID",
                table: "AppointmentDetails",
                column: "petID");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetails_serviceID",
                table: "AppointmentDetails",
                column: "serviceID");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_OwnerId",
                table: "Appointments",
                column: "OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppointmentDetails");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Services");
        }
    }
}
