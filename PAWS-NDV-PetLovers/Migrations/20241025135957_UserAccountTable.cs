using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAWS_NDV_PetLovers.Migrations
{
    /// <inheritdoc />
    public partial class UserAccountTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    acc_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    mname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    userName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    passWord = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    contact = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    userType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPasswordChanged = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.acc_Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAccounts");

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    admin_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsPasswordChanged = table.Column<bool>(type: "bit", nullable: false),
                    bdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    contact = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    mname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    passWord = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    userName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    userType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.admin_Id);
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    staff_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsPasswordChanged = table.Column<bool>(type: "bit", nullable: false),
                    admin_Id = table.Column<int>(type: "int", nullable: true),
                    bdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    contact = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    createdDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    mname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    passWord = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    userName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    userType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.staff_Id);
                });
        }
    }
}
