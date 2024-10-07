using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class DeviceExists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PermissionKeyRole",
                keyColumns: new[] { "PermissionKeysId", "RolesId" },
                keyValues: new object[] { new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a62"), new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61") });

            migrationBuilder.DeleteData(
                table: "PermissionKeyRole",
                keyColumns: new[] { "PermissionKeysId", "RolesId" },
                keyValues: new object[] { new Guid("e43167ad-158b-4a39-8f5d-c1a69b32d7cf"), new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61") });

            migrationBuilder.DeleteData(
                table: "PermissionKeys",
                keyColumn: "Id",
                keyValue: new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a62"));

            migrationBuilder.DeleteData(
                table: "PermissionKeys",
                keyColumn: "Id",
                keyValue: new Guid("e43167ad-158b-4a39-8f5d-c1a69b32d7cf"));

            migrationBuilder.InsertData(
                table: "PermissionKeys",
                columns: new[] { "Id", "Value" },
                values: new object[,]
                {
                    { new Guid("265ab018-9afa-4f99-acaa-7d9082bfe5ad"), "CanCreateADevice" },
                    { new Guid("8aed0b92-ab5b-47f3-af36-220ab60b66e4"), "CanCreateCompany" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("205e7ec9-673c-4db2-911d-10fe2b9c159a"),
                column: "CreatedAt",
                value: new DateTime(2024, 10, 7, 15, 35, 19, 397, DateTimeKind.Local).AddTicks(7850));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"),
                column: "CreatedAt",
                value: new DateTime(2024, 10, 7, 15, 35, 19, 437, DateTimeKind.Local).AddTicks(4270));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61"),
                column: "CreatedAt",
                value: new DateTime(2024, 10, 7, 15, 35, 19, 437, DateTimeKind.Local).AddTicks(4430));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e43167ad-158b-4a39-8f5d-c0a69b32d7cf"),
                column: "CreatedAt",
                value: new DateTime(2024, 10, 7, 15, 35, 19, 437, DateTimeKind.Local).AddTicks(4410));

            migrationBuilder.InsertData(
                table: "PermissionKeyRole",
                columns: new[] { "PermissionKeysId", "RolesId" },
                values: new object[,]
                {
                    { new Guid("265ab018-9afa-4f99-acaa-7d9082bfe5ad"), new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61") },
                    { new Guid("8aed0b92-ab5b-47f3-af36-220ab60b66e4"), new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PermissionKeyRole",
                keyColumns: new[] { "PermissionKeysId", "RolesId" },
                keyValues: new object[] { new Guid("265ab018-9afa-4f99-acaa-7d9082bfe5ad"), new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61") });

            migrationBuilder.DeleteData(
                table: "PermissionKeyRole",
                keyColumns: new[] { "PermissionKeysId", "RolesId" },
                keyValues: new object[] { new Guid("8aed0b92-ab5b-47f3-af36-220ab60b66e4"), new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61") });

            migrationBuilder.DeleteData(
                table: "PermissionKeys",
                keyColumn: "Id",
                keyValue: new Guid("265ab018-9afa-4f99-acaa-7d9082bfe5ad"));

            migrationBuilder.DeleteData(
                table: "PermissionKeys",
                keyColumn: "Id",
                keyValue: new Guid("8aed0b92-ab5b-47f3-af36-220ab60b66e4"));

            migrationBuilder.InsertData(
                table: "PermissionKeys",
                columns: new[] { "Id", "Value" },
                values: new object[,]
                {
                    { new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a62"), "CanCreateCompany" },
                    { new Guid("e43167ad-158b-4a39-8f5d-c1a69b32d7cf"), "CanCreateADevice" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("205e7ec9-673c-4db2-911d-10fe2b9c159a"),
                column: "CreatedAt",
                value: new DateTime(2024, 10, 7, 10, 45, 47, 736, DateTimeKind.Local).AddTicks(90));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"),
                column: "CreatedAt",
                value: new DateTime(2024, 10, 7, 10, 45, 47, 775, DateTimeKind.Local).AddTicks(4020));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61"),
                column: "CreatedAt",
                value: new DateTime(2024, 10, 7, 10, 45, 47, 775, DateTimeKind.Local).AddTicks(4160));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e43167ad-158b-4a39-8f5d-c0a69b32d7cf"),
                column: "CreatedAt",
                value: new DateTime(2024, 10, 7, 10, 45, 47, 775, DateTimeKind.Local).AddTicks(4140));

            migrationBuilder.InsertData(
                table: "PermissionKeyRole",
                columns: new[] { "PermissionKeysId", "RolesId" },
                values: new object[,]
                {
                    { new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a62"), new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61") },
                    { new Guid("e43167ad-158b-4a39-8f5d-c1a69b32d7cf"), new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61") }
                });
        }
    }
}
