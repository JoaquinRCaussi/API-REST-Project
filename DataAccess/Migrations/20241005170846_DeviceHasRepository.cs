using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations;

/// <inheritdoc />
public partial class DeviceHasRepository : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
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
            name: "Permissions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Permissions", x => x.Id);
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
            name: "Companies",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                RUT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Logo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Companies", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Devices",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                DeviceType = table.Column<int>(type: "int", nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Photo = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Devices", x => x.Id);
                table.ForeignKey(
                    name: "FK_Devices_Companies_CompanyId",
                    column: x => x.CompanyId,
                    principalTable: "Companies",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "HomeDevices",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                state = table.Column<bool>(type: "bit", nullable: false),
                HomeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_HomeDevices", x => x.Id);
                table.ForeignKey(
                    name: "FK_HomeDevices_Devices_DeviceId",
                    column: x => x.DeviceId,
                    principalTable: "Devices",
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
                HomeOwner = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Homes", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "MemberSettings",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                HomeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MemberSettings", x => x.Id);
                table.ForeignKey(
                    name: "FK_MemberSettings_Homes_HomeId",
                    column: x => x.HomeId,
                    principalTable: "Homes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
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

        migrationBuilder.CreateTable(
            name: "MemberSettingPermissions",
            columns: table => new
            {
                MemberSettingsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                PermissionsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MemberSettingPermissions", x => new { x.MemberSettingsId, x.PermissionsId });
                table.ForeignKey(
                    name: "FK_MemberSettingPermissions_MemberSettings_MemberSettingsId",
                    column: x => x.MemberSettingsId,
                    principalTable: "MemberSettings",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_MemberSettingPermissions_Permissions_PermissionsId",
                    column: x => x.PermissionsId,
                    principalTable: "Permissions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.InsertData(
            table: "PermissionKeys",
            columns: new[] { "Id", "Value" },
            values: new object[,]
            {
                { new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"), "CanCreateAdmin" },
                { new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61"), "CanCreateHomeOwner" },
                { new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a62"), "CanCreateCompany" },
                { new Guid("e43167ad-158b-4a39-8f5d-c0a69b32d7cf"), "CanCreateCompanyOwner" },
                { new Guid("e43167ad-158b-4a39-8f5d-c1a69b32d7cf"), "CanCreateADevice" }
            });

        migrationBuilder.InsertData(
            table: "Permissions",
            columns: new[] { "Id", "Value" },
            values: new object[,]
            {
                { new Guid("4d99af50-c4b9-4bc7-8c63-6e4f6f24a73a"), "CanListDevices" },
                { new Guid("7fa6a0f4-d7d9-4c89-a85e-92b937fc0274"), "CanAsociateDevices" },
                { new Guid("b2ff8154-fdb0-4a87-a7b5-ded13fb66f57"), "CanAddMembers" },
                { new Guid("c0f0d7a7-3e77-4128-87d3-30113b19936d"), "CanGetNotifications" }
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
                { new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a62"), new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61") },
                { new Guid("e43167ad-158b-4a39-8f5d-c0a69b32d7cf"), new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74") },
                { new Guid("e43167ad-158b-4a39-8f5d-c1a69b32d7cf"), new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61") }
            });

        migrationBuilder.InsertData(
            table: "Users",
            columns: new[] { "Id", "CompanyID", "Email", "HomeId", "ImagePath", "LastName", "Name", "Password", "RoleID" },
            values: new object[,]
            {
                { new Guid("205e7ec9-673c-4db2-911d-10fe2b9c159a"), new Guid("10570280-239e-4fb8-8939-4f37415fccb7"), "anothercompanyowner1@gmail.com", null, "", "anotherCompanyOwner", "anotherCompanyOwner", "companyowner@1", new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61") },
                { new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"), null, "admin@admin.com", null, "", "Admin", "Admin", "admin", new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74") },
                { new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61"), null, "companyowner1@gmail.com", null, "", "CompanyOwner", "CompanyOwner", "companyowner@1", new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61") },
                { new Guid("e43167ad-158b-4a39-8f5d-c0a69b32d7cf"), null, "homeowner1@gmail.com", null, "", "HomeOwner", "HomeOwner", "homeowner@1", new Guid("e43167ad-158b-4a39-8f5d-c0a69b32d7cf") }
            });

        migrationBuilder.InsertData(
            table: "Companies",
            columns: new[] { "Id", "Logo", "Name", "OwnerId", "RUT" },
            values: new object[] { new Guid("10570280-239e-4fb8-8939-4f37415fccb7"), "sadas/dasdasdas/asdasd", "Samsung", new Guid("205e7ec9-673c-4db2-911d-10fe2b9c159a"), "2141412" });

        migrationBuilder.InsertData(
            table: "Devices",
            columns: new[] { "Id", "CompanyId", "Description", "DeviceType", "Model", "Name", "Photo" },
            values: new object[,]
            {
                { new Guid("5d95af52-c4b9-4bc7-8c63-6e4f6f24a73a"), new Guid("10570280-239e-4fb8-8939-4f37415fccb7"), "Lampara de techo", 1, "Modelo 1", "Lampara", "https://www.google.com" },
                { new Guid("6d95af53-c4b9-4bc7-8c63-6e4f6f24a73a"), new Guid("10570280-239e-4fb8-8939-4f37415fccb7"), "Lampara de avion", 1, "Modelo 2", "Lampara de avion", "https://www.avion.com" }
            });

        migrationBuilder.CreateIndex(
            name: "IX_Companies_OwnerId",
            table: "Companies",
            column: "OwnerId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Devices_CompanyId",
            table: "Devices",
            column: "CompanyId");

        migrationBuilder.CreateIndex(
            name: "IX_HomeDevices_DeviceId",
            table: "HomeDevices",
            column: "DeviceId");

        migrationBuilder.CreateIndex(
            name: "IX_HomeDevices_HomeId",
            table: "HomeDevices",
            column: "HomeId");

        migrationBuilder.CreateIndex(
            name: "IX_Homes_OwnerId",
            table: "Homes",
            column: "OwnerId");

        migrationBuilder.CreateIndex(
            name: "IX_MemberSettingPermissions_PermissionsId",
            table: "MemberSettingPermissions",
            column: "PermissionsId");

        migrationBuilder.CreateIndex(
            name: "IX_MemberSettings_HomeId",
            table: "MemberSettings",
            column: "HomeId");

        migrationBuilder.CreateIndex(
            name: "IX_PermissionKeyRole_RolesId",
            table: "PermissionKeyRole",
            column: "RolesId");

        migrationBuilder.CreateIndex(
            name: "IX_Users_HomeId",
            table: "Users",
            column: "HomeId");

        migrationBuilder.CreateIndex(
            name: "IX_Users_RoleID",
            table: "Users",
            column: "RoleID");

        migrationBuilder.AddForeignKey(
            name: "FK_Companies_Users_OwnerId",
            table: "Companies",
            column: "OwnerId",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_HomeDevices_Homes_HomeId",
            table: "HomeDevices",
            column: "HomeId",
            principalTable: "Homes",
            principalColumn: "Id");

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
            name: "HomeDevices");

        migrationBuilder.DropTable(
            name: "MemberSettingPermissions");

        migrationBuilder.DropTable(
            name: "PermissionKeyRole");

        migrationBuilder.DropTable(
            name: "Devices");

        migrationBuilder.DropTable(
            name: "MemberSettings");

        migrationBuilder.DropTable(
            name: "Permissions");

        migrationBuilder.DropTable(
            name: "PermissionKeys");

        migrationBuilder.DropTable(
            name: "Companies");

        migrationBuilder.DropTable(
            name: "Users");

        migrationBuilder.DropTable(
            name: "Homes");

        migrationBuilder.DropTable(
            name: "Roles");
    }
}
