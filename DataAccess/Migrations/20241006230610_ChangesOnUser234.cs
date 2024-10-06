using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangesOnUser234 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("205e7ec9-673c-4db2-911d-10fe2b9c159a"),
                column: "CreatedAt",
                value: new DateTime(2024, 10, 6, 20, 6, 9, 711, DateTimeKind.Local).AddTicks(4088));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"),
                column: "CreatedAt",
                value: new DateTime(2024, 10, 6, 20, 6, 9, 721, DateTimeKind.Local).AddTicks(8974));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61"),
                column: "CreatedAt",
                value: new DateTime(2024, 10, 6, 20, 6, 9, 721, DateTimeKind.Local).AddTicks(9182));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e43167ad-158b-4a39-8f5d-c0a69b32d7cf"),
                column: "CreatedAt",
                value: new DateTime(2024, 10, 6, 20, 6, 9, 721, DateTimeKind.Local).AddTicks(9163));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("205e7ec9-673c-4db2-911d-10fe2b9c159a"),
                column: "CreatedAt",
                value: new DateTime(2024, 10, 6, 20, 3, 30, 625, DateTimeKind.Local).AddTicks(8556));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"),
                column: "CreatedAt",
                value: new DateTime(2024, 10, 6, 20, 3, 30, 637, DateTimeKind.Local).AddTicks(889));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61"),
                column: "CreatedAt",
                value: new DateTime(2024, 10, 6, 20, 3, 30, 637, DateTimeKind.Local).AddTicks(1089));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e43167ad-158b-4a39-8f5d-c0a69b32d7cf"),
                column: "CreatedAt",
                value: new DateTime(2024, 10, 6, 20, 3, 30, 637, DateTimeKind.Local).AddTicks(1071));
        }
    }
}
