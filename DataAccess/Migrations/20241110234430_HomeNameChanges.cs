using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations;

/// <inheritdoc />
public partial class HomeNameChanges : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Name",
            table: "Homes",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"),
            column: "CreatedAt",
            value: new DateTime(2024, 11, 10, 20, 44, 28, 597, DateTimeKind.Local).AddTicks(2692));
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Name",
            table: "Homes");

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"),
            column: "CreatedAt",
            value: new DateTime(2024, 11, 10, 2, 31, 22, 578, DateTimeKind.Local).AddTicks(6917));
    }
}
