using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations;

/// <inheritdoc />
public partial class AdminCanCreateCompany : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "PermissionKeyRole",
            columns: new[] { "PermissionKeysId", "RolesId" },
            values: new object[] { new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a62"), new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74") });

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("205e7ec9-673c-4db2-911d-10fe2b9c159a"),
            column: "CreatedAt",
            value: new DateTime(2024, 10, 6, 22, 28, 22, 561, DateTimeKind.Local).AddTicks(7689));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"),
            column: "CreatedAt",
            value: new DateTime(2024, 10, 6, 22, 28, 22, 574, DateTimeKind.Local).AddTicks(415));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61"),
            column: "CreatedAt",
            value: new DateTime(2024, 10, 6, 22, 28, 22, 574, DateTimeKind.Local).AddTicks(1052));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("e43167ad-158b-4a39-8f5d-c0a69b32d7cf"),
            column: "CreatedAt",
            value: new DateTime(2024, 10, 6, 22, 28, 22, 574, DateTimeKind.Local).AddTicks(1028));
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "PermissionKeyRole",
            keyColumns: new[] { "PermissionKeysId", "RolesId" },
            keyValues: new object[] { new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a62"), new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74") });

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("205e7ec9-673c-4db2-911d-10fe2b9c159a"),
            column: "CreatedAt",
            value: new DateTime(2024, 10, 6, 21, 48, 22, 26, DateTimeKind.Local).AddTicks(2322));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"),
            column: "CreatedAt",
            value: new DateTime(2024, 10, 6, 21, 48, 22, 39, DateTimeKind.Local).AddTicks(7806));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61"),
            column: "CreatedAt",
            value: new DateTime(2024, 10, 6, 21, 48, 22, 39, DateTimeKind.Local).AddTicks(8093));

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("e43167ad-158b-4a39-8f5d-c0a69b32d7cf"),
            column: "CreatedAt",
            value: new DateTime(2024, 10, 6, 21, 48, 22, 39, DateTimeKind.Local).AddTicks(8064));
    }
}
