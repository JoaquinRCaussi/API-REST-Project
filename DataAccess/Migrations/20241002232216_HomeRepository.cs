using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class HomeRepository : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RUT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionKeys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "PermissionKeyRole",
                columns: table => new
                {
                    PermissionKeysId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RolesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionKeyRole", x => new { x.PermissionKeysId, x.RolesId });
                    table.ForeignKey(
                        name: "FK_PermissionKeyRole_PermissionKeys_PermissionKeysId",
                        column: x => x.PermissionKeysId,
                        principalTable: "PermissionKeys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionKeyRole_Roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Homes", x => x.Id);
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
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CompanyID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HomeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_Homes_HomeId",
                        column: x => x.HomeId,
                        principalTable: "Homes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "PermissionKeys",
                columns: new[] { "Id", "Value" },
                values: new object[,]
                {
                    { new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"), "CanCreateAdmin" },
                    { new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61"), "CanCreateHomeOwner" },
                    { new Guid("e43167ad-158b-4a39-8f5d-c0a69b32d7cf"), "CanCreateCompanyOwner" }
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
                table: "PermissionKeyRole",
                columns: new[] { "PermissionKeysId", "RolesId" },
                values: new object[,]
                {
                    { new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"), new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74") },
                    { new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61"), new Guid("e43167ad-158b-4a39-8f5d-c0a69b32d7cf") },
                    { new Guid("e43167ad-158b-4a39-8f5d-c0a69b32d7cf"), new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74") }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CompanyID", "Email", "HomeId", "ImagePath", "LastName", "Name", "Password", "RoleID" },
                values: new object[,]
                {
                    { new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"), null, "admin@admin.com", null, "", "Admin", "Admin", "admin", new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74") },
                    { new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61"), null, "companyowner1@gmail.com", null, "", "CompanyOwner", "CompanyOwner", "companyowner@1", new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61") },
                    { new Guid("e43167ad-158b-4a39-8f5d-c0a69b32d7cf"), null, "homeowner1@gmail.com", null, "", "HomeOwner", "HomeOwner", "homeowner@1", new Guid("e43167ad-158b-4a39-8f5d-c0a69b32d7cf") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Homes_OwnerId",
                table: "Homes",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionKeyRole_RolesId",
                table: "PermissionKeyRole",
                column: "RolesId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CompanyID",
                table: "Users",
                column: "CompanyID",
                unique: true,
                filter: "[CompanyID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_HomeId",
                table: "Users",
                column: "HomeId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleID",
                table: "Users",
                column: "RoleID");

            migrationBuilder.AddForeignKey(
                name: "FK_Homes_Users_OwnerId",
                table: "Homes",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homes_Users_OwnerId",
                table: "Homes");

            migrationBuilder.DropTable(
                name: "PermissionKeyRole");

            migrationBuilder.DropTable(
                name: "PermissionKeys");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Homes");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
