using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations;

/// <inheritdoc />
public partial class ChangingPermissions : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_MemberSettings_Users_UserId",
            table: "MemberSettings");

        migrationBuilder.DropIndex(
            name: "IX_MemberSettings_UserId",
            table: "MemberSettings");

        migrationBuilder.UpdateData(
            table: "Permissions",
            keyColumn: "Id",
            keyValue: new Guid("4d99af50-c4b9-4bc7-8c63-6e4f6f24a73a"),
            column: "Value",
            value: "CanListDevices");

        migrationBuilder.UpdateData(
            table: "Permissions",
            keyColumn: "Id",
            keyValue: new Guid("7fa6a0f4-d7d9-4c89-a85e-92b937fc0274"),
            column: "Value",
            value: "CanAsociateDevices");

        migrationBuilder.UpdateData(
            table: "Permissions",
            keyColumn: "Id",
            keyValue: new Guid("b2ff8154-fdb0-4a87-a7b5-ded13fb66f57"),
            column: "Value",
            value: "CanAddMembers");

        migrationBuilder.UpdateData(
            table: "Permissions",
            keyColumn: "Id",
            keyValue: new Guid("c0f0d7a7-3e77-4128-87d3-30113b19936d"),
            column: "Value",
            value: "CanGetNotifications");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "Permissions",
            keyColumn: "Id",
            keyValue: new Guid("4d99af50-c4b9-4bc7-8c63-6e4f6f24a73a"),
            column: "Value",
            value: "puedeListarDispositivos");

        migrationBuilder.UpdateData(
            table: "Permissions",
            keyColumn: "Id",
            keyValue: new Guid("7fa6a0f4-d7d9-4c89-a85e-92b937fc0274"),
            column: "Value",
            value: "puedeAsociarDispositivos");

        migrationBuilder.UpdateData(
            table: "Permissions",
            keyColumn: "Id",
            keyValue: new Guid("b2ff8154-fdb0-4a87-a7b5-ded13fb66f57"),
            column: "Value",
            value: "puedeAgregarMiembros");

        migrationBuilder.UpdateData(
            table: "Permissions",
            keyColumn: "Id",
            keyValue: new Guid("c0f0d7a7-3e77-4128-87d3-30113b19936d"),
            column: "Value",
            value: "puedeRecibirNotificaciones");

        migrationBuilder.CreateIndex(
            name: "IX_MemberSettings_UserId",
            table: "MemberSettings",
            column: "UserId");

        migrationBuilder.AddForeignKey(
            name: "FK_MemberSettings_Users_UserId",
            table: "MemberSettings",
            column: "UserId",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
