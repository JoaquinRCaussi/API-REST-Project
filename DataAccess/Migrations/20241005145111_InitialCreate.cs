using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Companies_CompanyID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CompanyID",
                table: "Users");

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "Devices",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Companies",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Devices",
                keyColumn: "Id",
                keyValue: new Guid("5d95af52-c4b9-4bc7-8c63-6e4f6f24a73a"),
                column: "CompanyId",
                value: new Guid("10570280-239e-4fb8-8939-4f37415fccb7"));

            migrationBuilder.UpdateData(
                table: "Devices",
                keyColumn: "Id",
                keyValue: new Guid("6d95af53-c4b9-4bc7-8c63-6e4f6f24a73a"),
                column: "CompanyId",
                value: new Guid("10570280-239e-4fb8-8939-4f37415fccb7"));

            migrationBuilder.InsertData(
                table: "PermissionKeys",
                columns: new[] { "Id", "Value" },
                values: new object[] { new Guid("e43167ad-158b-4a39-8f5d-c1a69b32d7cf"), "CanCreateADevice" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CompanyID", "Email", "HomeId", "ImagePath", "LastName", "Name", "Password", "RoleID" },
                values: new object[] { new Guid("205e7ec9-673c-4db2-911d-10fe2b9c159a"), new Guid("10570280-239e-4fb8-8939-4f37415fccb7"), "anothercompanyowner1@gmail.com", null, "", "anotherCompanyOwner", "anotherCompanyOwner", "companyowner@1", new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61") });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Logo", "Name", "OwnerId", "RUT" },
                values: new object[] { new Guid("10570280-239e-4fb8-8939-4f37415fccb7"), "sadas/dasdasdas/asdasd", "Samsung", new Guid("205e7ec9-673c-4db2-911d-10fe2b9c159a"), "2141412" });

            migrationBuilder.InsertData(
                table: "PermissionKeyRole",
                columns: new[] { "PermissionKeysId", "RolesId" },
                values: new object[] { new Guid("e43167ad-158b-4a39-8f5d-c1a69b32d7cf"), new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61") });

            migrationBuilder.CreateIndex(
                name: "IX_Devices_CompanyId",
                table: "Devices",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_OwnerId",
                table: "Companies",
                column: "OwnerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Users_OwnerId",
                table: "Companies",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Companies_CompanyId",
                table: "Devices",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Users_OwnerId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Companies_CompanyId",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_CompanyId",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Companies_OwnerId",
                table: "Companies");

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("10570280-239e-4fb8-8939-4f37415fccb7"));

            migrationBuilder.DeleteData(
                table: "PermissionKeyRole",
                keyColumns: new[] { "PermissionKeysId", "RolesId" },
                keyValues: new object[] { new Guid("e43167ad-158b-4a39-8f5d-c1a69b32d7cf"), new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61") });

            migrationBuilder.DeleteData(
                table: "PermissionKeys",
                keyColumn: "Id",
                keyValue: new Guid("e43167ad-158b-4a39-8f5d-c1a69b32d7cf"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("205e7ec9-673c-4db2-911d-10fe2b9c159a"));

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Companies");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CompanyID",
                table: "Users",
                column: "CompanyID",
                unique: true,
                filter: "[CompanyID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Companies_CompanyID",
                table: "Users",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "Id");
        }
    }
}
