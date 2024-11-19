using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations;

/// <inheritdoc />
public partial class CompanyOwnerCanGetCompanies2 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"),
            column: "CreatedAt",
            value: new DateTime(2024, 11, 18, 13, 36, 14, 488, DateTimeKind.Local).AddTicks(3900));
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"),
            column: "CreatedAt",
            value: new DateTime(2024, 11, 18, 12, 35, 7, 594, DateTimeKind.Local).AddTicks(3980));
    }
}
