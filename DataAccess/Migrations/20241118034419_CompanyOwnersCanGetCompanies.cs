using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations;

/// <inheritdoc />
public partial class CompanyOwnersCanGetCompanies : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "PermissionKeyRole",
            columns: new[] { "PermissionKeysId", "RolesId" },
            values: new object[] { new Guid("77777777-7777-7777-7777-777777777777"), new Guid("78947c68-f0aa-49d3-8f47-444444444444") });

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"),
            column: "CreatedAt",
            value: new DateTime(2024, 11, 18, 0, 44, 19, 197, DateTimeKind.Local).AddTicks(5349));
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "PermissionKeyRole",
            keyColumns: new[] { "PermissionKeysId", "RolesId" },
            keyValues: new object[] { new Guid("77777777-7777-7777-7777-777777777777"), new Guid("78947c68-f0aa-49d3-8f47-444444444444") });

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"),
            column: "CreatedAt",
            value: new DateTime(2024, 11, 16, 0, 45, 39, 111, DateTimeKind.Local).AddTicks(1316));
    }
}
