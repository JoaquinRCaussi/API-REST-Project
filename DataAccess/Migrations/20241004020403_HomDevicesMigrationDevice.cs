using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class HomDevicesMigrationDevice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Homes_HomeId",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_HomeId",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "HomeId",
                table: "Devices");

            migrationBuilder.AddColumn<Guid>(
                name: "HomeId",
                table: "HomeDevices",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "Id", "Description", "DeviceType", "Model", "Name", "Photo" },
                values: new object[,]
                {
                    { new Guid("5d95af52-c4b9-4bc7-8c63-6e4f6f24a73a"), "Lampara de techo", 1, "Modelo 1", "Lampara", "https://www.google.com" },
                    { new Guid("6d95af53-c4b9-4bc7-8c63-6e4f6f24a73a"), "Lampara de avion", 1, "Modelo 2", "Lampara de avion", "https://www.avion.com" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_HomeDevices_HomeId",
                table: "HomeDevices",
                column: "HomeId");

            migrationBuilder.AddForeignKey(
                name: "FK_HomeDevices_Homes_HomeId",
                table: "HomeDevices",
                column: "HomeId",
                principalTable: "Homes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HomeDevices_Homes_HomeId",
                table: "HomeDevices");

            migrationBuilder.DropIndex(
                name: "IX_HomeDevices_HomeId",
                table: "HomeDevices");

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "Id",
                keyValue: new Guid("5d95af52-c4b9-4bc7-8c63-6e4f6f24a73a"));

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "Id",
                keyValue: new Guid("6d95af53-c4b9-4bc7-8c63-6e4f6f24a73a"));

            migrationBuilder.DropColumn(
                name: "HomeId",
                table: "HomeDevices");

            migrationBuilder.AddColumn<Guid>(
                name: "HomeId",
                table: "Devices",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_HomeId",
                table: "Devices",
                column: "HomeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Homes_HomeId",
                table: "Devices",
                column: "HomeId",
                principalTable: "Homes",
                principalColumn: "Id");
        }
    }
}
