using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations;

/// <inheritdoc />
public partial class MemberMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Company",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                RUT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Logo = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Company", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Homes",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                MemberCount = table.Column<int>(type: "int", nullable: false),
                Devices = table.Column<string>(type: "nvarchar(max)", nullable: false),
                HomeOwner = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Members = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Homes", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PermissionKeys",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Role = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PermissionKeys", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Roles",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Roles", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                RoleID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CompanyID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
                table.ForeignKey(
                    name: "FK_Users_Company_CompanyID",
                    column: x => x.CompanyID,
                    principalTable: "Company",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Users_Roles_RoleID",
                    column: x => x.RoleID,
                    principalTable: "Roles",
                    principalColumn: "Id");
            });

        migrationBuilder.InsertData(
            table: "Roles",
            columns: new[] { "Id", "Name" },
            values: new object[,]
            {
                { new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"), "Admin" },
                { new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61"), "CompanyOwner" },
                { new Guid("e43167ad-158b-4a39-8f5d-c0a69b32d7cf"), "HomeOwner" }
            });

        migrationBuilder.InsertData(
            table: "Users",
            columns: new[] { "Id", "CompanyID", "Email", "LastName", "Name", "Password", "RoleID" },
            values: new object[,]
            {
                { new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"), null, "admin@admin.com", "Admin", "Admin", "admin", new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74") },
                { new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61"), null, "companyowner1@gmail.com", "CompanyOwner", "CompanyOwner", "companyowner@1", new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61") },
                { new Guid("e43167ad-158b-4a39-8f5d-c0a69b32d7cf"), null, "homeowner1@gmail.com", "HomeOwner", "HomeOwner", "homeowner@1", new Guid("e43167ad-158b-4a39-8f5d-c0a69b32d7cf") }
            });

        migrationBuilder.CreateIndex(
            name: "IX_Users_CompanyID",
            table: "Users",
            column: "CompanyID");

        migrationBuilder.CreateIndex(
            name: "IX_Users_RoleID",
            table: "Users",
            column: "RoleID");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Homes");

        migrationBuilder.DropTable(
            name: "PermissionKeys");

        migrationBuilder.DropTable(
            name: "Users");

        migrationBuilder.DropTable(
            name: "Company");

        migrationBuilder.DropTable(
            name: "Roles");
    }
}
