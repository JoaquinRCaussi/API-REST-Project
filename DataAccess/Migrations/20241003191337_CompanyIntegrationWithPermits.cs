using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class CompanyIntegrationWithPermits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PermissionKeys",
                columns: new[] { "Id", "Value" },
                values: new object[] { new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a62"), "CanCreateCompany" });

            migrationBuilder.InsertData(
                table: "PermissionKeyRole",
                columns: new[] { "PermissionKeysId", "RolesId" },
                values: new object[]
                {
                    new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a62"),
                    new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61")
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PermissionKeyRole",
                keyColumns: new[] { "PermissionKeysId", "RolesId" },
                keyValues: new object[]
                {
                    new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a62"),
                    new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61")
                });

            migrationBuilder.DeleteData(
                table: "PermissionKeys",
                keyColumn: "Id",
                keyValue: new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a62"));
        }
    }
}
