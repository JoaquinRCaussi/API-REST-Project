using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations;

/// <inheritdoc />
public partial class DeviceHasRepositoryAndTPH : Migration
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
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
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
    }
}
