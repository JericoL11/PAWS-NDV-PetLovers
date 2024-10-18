using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAWS_NDV_PetLovers.Migrations
{
    /// <inheritdoc />
    public partial class AllTables : Migration
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
                    ownerId = table.Column<int>(type: "int", nullable: true),
                    fname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    contact = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.AppointId);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "5000, 1"),
                    categoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    registeredDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    lastUpdate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Owners",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1000, 1"),
                    fname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    contact = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    registeredDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    lastUpdate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owners", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Purchases",
                columns: table => new
                {
                    purchaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "30000, 1"),
                    date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    diagnosisId_holder = table.Column<int>(type: "int", nullable: true),
                    customerName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => x.purchaseId);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    serviceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "500, 1"),
                    serviceName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    serviceCharge = table.Column<double>(type: "float", nullable: false),
                    serviceType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.serviceId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "50000, 1"),
                    productName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sellingPrice = table.Column<double>(type: "float", nullable: false),
                    supplierPrice = table.Column<double>(type: "float", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    registeredDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    lastUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    expiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pets",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "100000, 1"),
                    petName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    species = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    breed = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    age = table.Column<int>(type: "int", nullable: false),
                    color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    gender = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    lastUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    registeredDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ownerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pets", x => x.id);
                    table.ForeignKey(
                        name: "FK_Pets_Owners_ownerId",
                        column: x => x.ownerId,
                        principalTable: "Owners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppointmentDetails",
                columns: table => new
                {
                    AppDetailsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "11000, 1"),
                    AppointId = table.Column<int>(type: "int", nullable: false),
                    serviceID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentDetails", x => x.AppDetailsId);
                    table.ForeignKey(
                        name: "FK_AppointmentDetails_Appointments_AppointId",
                        column: x => x.AppointId,
                        principalTable: "Appointments",
                        principalColumn: "AppointId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppointmentDetails_Services_serviceID",
                        column: x => x.serviceID,
                        principalTable: "Services",
                        principalColumn: "serviceId");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseDetails",
                columns: table => new
                {
                    purchaseDet_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "300000, 1"),
                    purchaseId = table.Column<int>(type: "int", nullable: false),
                    productId = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    sellingPrice = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseDetails", x => x.purchaseDet_Id);
                    table.ForeignKey(
                        name: "FK_PurchaseDetails_Products_productId",
                        column: x => x.productId,
                        principalTable: "Products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseDetails_Purchases_purchaseId",
                        column: x => x.purchaseId,
                        principalTable: "Purchases",
                        principalColumn: "purchaseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Diagnostics",
                columns: table => new
                {
                    diagnostic_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "60000, 1"),
                    petId = table.Column<int>(type: "int", nullable: false),
                    weight = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseNavpurchaseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diagnostics", x => x.diagnostic_Id);
                    table.ForeignKey(
                        name: "FK_Diagnostics_Pets_petId",
                        column: x => x.petId,
                        principalTable: "Pets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Diagnostics_Purchases_PurchaseNavpurchaseId",
                        column: x => x.PurchaseNavpurchaseId,
                        principalTable: "Purchases",
                        principalColumn: "purchaseId");
                });

            migrationBuilder.CreateTable(
                name: "Billings",
                columns: table => new
                {
                    billingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "80000, 1"),
                    DiagnosticsId = table.Column<int>(type: "int", nullable: true),
                    PurchaseId = table.Column<int>(type: "int", nullable: true),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    grandTotal = table.Column<double>(type: "float", nullable: true),
                    cashReceive = table.Column<double>(type: "float", nullable: true),
                    changeAmount = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Billings", x => x.billingId);
                    table.ForeignKey(
                        name: "FK_Billings_Diagnostics_DiagnosticsId",
                        column: x => x.DiagnosticsId,
                        principalTable: "Diagnostics",
                        principalColumn: "diagnostic_Id");
                    table.ForeignKey(
                        name: "FK_Billings_Purchases_PurchaseId",
                        column: x => x.PurchaseId,
                        principalTable: "Purchases",
                        principalColumn: "purchaseId");
                });

            migrationBuilder.CreateTable(
                name: "DiagnosticDetails",
                columns: table => new
                {
                    diagnosticDet_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "160000, 1"),
                    details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    diagnosticsId = table.Column<int>(type: "int", nullable: false),
                    serviceId = table.Column<int>(type: "int", nullable: false),
                    totalServicePayment = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiagnosticDetails", x => x.diagnosticDet_Id);
                    table.ForeignKey(
                        name: "FK_DiagnosticDetails_Diagnostics_diagnosticsId",
                        column: x => x.diagnosticsId,
                        principalTable: "Diagnostics",
                        principalColumn: "diagnostic_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiagnosticDetails_Services_serviceId",
                        column: x => x.serviceId,
                        principalTable: "Services",
                        principalColumn: "serviceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PetFollowUps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "21000, 1"),
                    diagnosticsId = table.Column<int>(type: "int", nullable: false),
                    serviceId = table.Column<int>(type: "int", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetFollowUps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PetFollowUps_Diagnostics_diagnosticsId",
                        column: x => x.diagnosticsId,
                        principalTable: "Diagnostics",
                        principalColumn: "diagnostic_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PetFollowUps_Services_serviceId",
                        column: x => x.serviceId,
                        principalTable: "Services",
                        principalColumn: "serviceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetails_AppointId",
                table: "AppointmentDetails",
                column: "AppointId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetails_serviceID",
                table: "AppointmentDetails",
                column: "serviceID");

            migrationBuilder.CreateIndex(
                name: "IX_Billings_DiagnosticsId",
                table: "Billings",
                column: "DiagnosticsId");

            migrationBuilder.CreateIndex(
                name: "IX_Billings_PurchaseId",
                table: "Billings",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_DiagnosticDetails_diagnosticsId",
                table: "DiagnosticDetails",
                column: "diagnosticsId");

            migrationBuilder.CreateIndex(
                name: "IX_DiagnosticDetails_serviceId",
                table: "DiagnosticDetails",
                column: "serviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnostics_petId",
                table: "Diagnostics",
                column: "petId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnostics_PurchaseNavpurchaseId",
                table: "Diagnostics",
                column: "PurchaseNavpurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_PetFollowUps_diagnosticsId",
                table: "PetFollowUps",
                column: "diagnosticsId");

            migrationBuilder.CreateIndex(
                name: "IX_PetFollowUps_serviceId",
                table: "PetFollowUps",
                column: "serviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_ownerId",
                table: "Pets",
                column: "ownerId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseDetails_productId",
                table: "PurchaseDetails",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseDetails_purchaseId",
                table: "PurchaseDetails",
                column: "purchaseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppointmentDetails");

            migrationBuilder.DropTable(
                name: "Billings");

            migrationBuilder.DropTable(
                name: "DiagnosticDetails");

            migrationBuilder.DropTable(
                name: "PetFollowUps");

            migrationBuilder.DropTable(
                name: "PurchaseDetails");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Diagnostics");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Pets");

            migrationBuilder.DropTable(
                name: "Purchases");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Owners");
        }
    }
}
