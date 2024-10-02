using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations;

/// <inheritdoc />
public partial class UpgradingBD : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Members",
            table: "Homes");

        migrationBuilder.AddColumn<Guid>(
            name: "HomeId",
            table: "Users",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "OwnerId",
            table: "Homes",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"),
            column: "HomeId",
            value: null);

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61"),
            column: "HomeId",
            value: null);

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("e43167ad-158b-4a39-8f5d-c0a69b32d7cf"),
            column: "HomeId",
            value: null);

        migrationBuilder.CreateIndex(
            name: "IX_Users_HomeId",
            table: "Users",
            column: "HomeId");

        migrationBuilder.CreateIndex(
            name: "IX_Homes_OwnerId",
            table: "Homes",
            column: "OwnerId");

        migrationBuilder.AddForeignKey(
            name: "FK_Homes_Users_OwnerId",
            table: "Homes",
            column: "OwnerId",
            principalTable: "Users",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Users_Homes_HomeId",
            table: "Users",
            column: "HomeId",
            principalTable: "Homes",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Homes_Users_OwnerId",
            table: "Homes");

        migrationBuilder.DropForeignKey(
            name: "FK_Users_Homes_HomeId",
            table: "Users");

        migrationBuilder.DropIndex(
            name: "IX_Users_HomeId",
            table: "Users");

        migrationBuilder.DropIndex(
            name: "IX_Homes_OwnerId",
            table: "Homes");

        migrationBuilder.DropColumn(
            name: "HomeId",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "OwnerId",
            table: "Homes");

        migrationBuilder.AddColumn<string>(
            name: "Members",
            table: "Homes",
            type: "nvarchar(max)",
            nullable: true);
    }
}
