using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "Indoors",
            table: "Devices",
            type: "bit",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "Outdoors",
            table: "Devices",
            type: "bit",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "SupportMovementDetection",
            table: "Devices",
            type: "bit",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "SupportPersonDetection",
            table: "Devices",
            type: "bit",
            nullable: true);

        migrationBuilder.InsertData(
            table: "PermissionKeys",
            columns: new[] { "Id", "Value" },
            values: new object[] { new Guid("77777777-7777-7777-7777-777777777777"), "CanGetCompanies" });

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"),
            column: "CreatedAt",
            value: new DateTime(2024, 10, 8, 12, 5, 49, 27, DateTimeKind.Local).AddTicks(6084));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61"),
            column: "CreatedAt",
            value: new DateTime(2024, 10, 8, 12, 5, 49, 29, DateTimeKind.Local).AddTicks(4947));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("e43167ad-158b-4a39-8f5d-c0a69b32d7cf"),
            column: "CreatedAt",
            value: new DateTime(2024, 10, 8, 12, 5, 49, 29, DateTimeKind.Local).AddTicks(4886));

        migrationBuilder.InsertData(
            table: "PermissionKeyRole",
            columns: new[] { "PermissionKeysId", "RolesId" },
            values: new object[] { new Guid("77777777-7777-7777-7777-777777777777"), new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61") });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "PermissionKeyRole",
            keyColumns: new[] { "PermissionKeysId", "RolesId" },
            keyValues: new object[] { new Guid("77777777-7777-7777-7777-777777777777"), new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61") });

        migrationBuilder.DeleteData(
            table: "PermissionKeys",
            keyColumn: "Id",
            keyValue: new Guid("77777777-7777-7777-7777-777777777777"));

        migrationBuilder.DropColumn(
            name: "Indoors",
            table: "Devices");

        migrationBuilder.DropColumn(
            name: "Outdoors",
            table: "Devices");

        migrationBuilder.DropColumn(
            name: "SupportMovementDetection",
            table: "Devices");

        migrationBuilder.DropColumn(
            name: "SupportPersonDetection",
            table: "Devices");

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"),
            column: "CreatedAt",
            value: new DateTime(2024, 10, 7, 18, 23, 17, 188, DateTimeKind.Local).AddTicks(5290));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61"),
            column: "CreatedAt",
            value: new DateTime(2024, 10, 7, 18, 23, 17, 221, DateTimeKind.Local).AddTicks(3730));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("e43167ad-158b-4a39-8f5d-c0a69b32d7cf"),
            column: "CreatedAt",
            value: new DateTime(2024, 10, 7, 18, 23, 17, 221, DateTimeKind.Local).AddTicks(3700));
    }
}
