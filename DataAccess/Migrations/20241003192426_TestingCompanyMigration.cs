using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations;

/// <inheritdoc />
public partial class TestingCompanyMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "PermissionKeys",
            columns: new[] { "Id", "Value" },
            values: new object[] { new Guid("7fa6a0f4-d7d9-4c89-a85e-92b937fc0274"), "CanCreateCompany" });

        migrationBuilder.InsertData(
            table: "PermissionKeyRole",
            columns: new[] { "PermissionKeysId", "RolesId" },
            values: new object[] { new Guid("7fa6a0f4-d7d9-4c89-a85e-92b937fc0274"), new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61") });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "PermissionKeyRole",
            keyColumns: new[] { "PermissionKeysId", "RolesId" },
            keyValues: new object[] { new Guid("7fa6a0f4-d7d9-4c89-a85e-92b937fc0274"), new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61") });

        migrationBuilder.DeleteData(
            table: "PermissionKeys",
            keyColumn: "Id",
            keyValue: new Guid("7fa6a0f4-d7d9-4c89-a85e-92b937fc0274"));
    }
}
