using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class HomeAndFilterMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Homes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Location = table.Column<string>(type: "TEXT", nullable: false),
                    MemberCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Devices = table.Column<string>(type: "TEXT", nullable: false),
                    HomeOwner = table.Column<Guid>(type: "TEXT", nullable: false),
                    Members = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Homes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Company", "Email", "LastName", "Name", "Password", "Role" },
                values: new object[] { new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"), null, "admin@admin.com", "Admin", "Admin", "admin", new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Homes");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"));
        }
    }
}
