using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations;

/// <inheritdoc />
public partial class InitialCreate4 : Migration
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
                Photo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Outdoors = table.Column<bool>(type: "bit", nullable: true),
                Indoors = table.Column<bool>(type: "bit", nullable: true),
                SupportMovementDetection = table.Column<bool>(type: "bit", nullable: true),
                SupportPersonDetection = table.Column<bool>(type: "bit", nullable: true)
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
                HardwareId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                State = table.Column<bool>(type: "bit", nullable: false),
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
                Latitude = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Longitude = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
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
            name: "MemberSettingPermission",
            columns: table => new
            {
                MemberSettingsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                PermissionsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MemberSettingPermission", x => new { x.MemberSettingsId, x.PermissionsId });
                table.ForeignKey(
                    name: "FK_MemberSettingPermission_MemberSettings_MemberSettingsId",
                    column: x => x.MemberSettingsId,
                    principalTable: "MemberSettings",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_MemberSettingPermission_Permissions_PermissionsId",
                    column: x => x.PermissionsId,
                    principalTable: "Permissions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Notifications",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Event = table.Column<string>(type: "nvarchar(max)", nullable: true),
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                IsRead = table.Column<bool>(type: "bit", nullable: false),
                HardwareId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                HomeDeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Notifications", x => x.Id);
                table.ForeignKey(
                    name: "FK_Notifications_HomeDevices_HomeDeviceId",
                    column: x => x.HomeDeviceId,
                    principalTable: "HomeDevices",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Notifications_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.InsertData(
            table: "PermissionKeys",
            columns: new[] { "Id", "Value" },
            values: new object[,]
            {
                { new Guid("11111111-1111-1111-1111-111111111111"), "CanCreateAdmin" },
                { new Guid("22222222-2222-2222-2222-222222222222"), "CanCreateCompanyOwner" },
                { new Guid("33333333-3333-3333-3333-333333333333"), "CanCreateHomeOwner" },
                { new Guid("44444444-4444-4444-4444-444444444444"), "CanCreateCompany" },
                { new Guid("55555555-5555-5555-5555-555555555555"), "CanCreateADevice" },
                { new Guid("66666666-6666-6666-6666-666666666666"), "CanManageUsers" },
                { new Guid("77777777-7777-7777-7777-777777777777"), "CanGetCompanies" }
            });

        migrationBuilder.InsertData(
            table: "Permissions",
            columns: new[] { "Id", "Value" },
            values: new object[,]
            {
                { new Guid("1a6b8ddf-3f92-4fda-87a4-777777777777"), "CanAsociateDevices" },
                { new Guid("2ebd4f21-3cd4-431f-97e7-999999999999"), "CanGetNotifications" },
                { new Guid("3bcde1b8-5ad2-4f6c-92c7-101010101010"), "CanAddMembers" },
                { new Guid("4c6b8b8d-3f92-4fda-87a4-202020202020"), "CanCreateRoom" },
                { new Guid("5d7fa8f6-ace7-42e5-8bdf-303030303030"), "CanAsociateDeviceToRoom" },
                { new Guid("6ebd4f21-3cd4-431f-97e7-404040404040"), "CanChangeHomeName" },
                { new Guid("7bcde1b8-5ad2-4f6c-92c7-505050505050"), "CanChangeHomeDevicesNames" },
                { new Guid("d47fa8f6-ace7-42e5-8bdf-888888888888"), "CanListDevices" }
            });

        migrationBuilder.InsertData(
            table: "Roles",
            columns: new[] { "Id", "Name" },
            values: new object[,]
            {
                { new Guid("6d72b33a-582b-411e-a9b1-333333333333"), "HomeOwner" },
                { new Guid("78947c68-f0aa-49d3-8f47-444444444444"), "CompanyOwner" },
                { new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61"), "Admin" }
            });

        migrationBuilder.InsertData(
            table: "PermissionKeyRole",
            columns: new[] { "PermissionKeysId", "RolesId" },
            values: new object[,]
            {
                { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61") },
                { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61") },
                { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("6d72b33a-582b-411e-a9b1-333333333333") },
                { new Guid("44444444-4444-4444-4444-444444444444"), new Guid("78947c68-f0aa-49d3-8f47-444444444444") },
                { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("78947c68-f0aa-49d3-8f47-444444444444") },
                { new Guid("66666666-6666-6666-6666-666666666666"), new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61") },
                { new Guid("77777777-7777-7777-7777-777777777777"), new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61") }
            });

        migrationBuilder.InsertData(
            table: "Users",
            columns: new[] { "Id", "CompanyID", "CreatedAt", "Email", "HomeId", "ImagePath", "LastName", "Name", "Password", "RoleID" },
            values: new object[] { new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"), null, new DateTime(2024, 11, 7, 20, 9, 8, 176, DateTimeKind.Local).AddTicks(652), "admin@admin.com", null, "", "Admin", "Admin", "admin", new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61") });

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
            name: "IX_MemberSettingPermission_PermissionsId",
            table: "MemberSettingPermission",
            column: "PermissionsId");

        migrationBuilder.CreateIndex(
            name: "IX_MemberSettings_HomeId",
            table: "MemberSettings",
            column: "HomeId");

        migrationBuilder.CreateIndex(
            name: "IX_Notifications_HomeDeviceId",
            table: "Notifications",
            column: "HomeDeviceId");

        migrationBuilder.CreateIndex(
            name: "IX_Notifications_UserId",
            table: "Notifications",
            column: "UserId");

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
            name: "MemberSettingPermission");

        migrationBuilder.DropTable(
            name: "Notifications");

        migrationBuilder.DropTable(
            name: "PermissionKeyRole");

        migrationBuilder.DropTable(
            name: "MemberSettings");

        migrationBuilder.DropTable(
            name: "Permissions");

        migrationBuilder.DropTable(
            name: "HomeDevices");

        migrationBuilder.DropTable(
            name: "PermissionKeys");

        migrationBuilder.DropTable(
            name: "Devices");

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
