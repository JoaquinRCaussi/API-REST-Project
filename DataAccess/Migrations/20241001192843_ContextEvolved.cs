using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations;

/// <inheritdoc />
public partial class ContextEvolved : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Role",
            table: "PermissionKeys");

        migrationBuilder.AlterColumn<string>(
            name: "Value",
            table: "PermissionKeys",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.CreateTable(
            name: "PermissionKeyRole",
            columns: table => new
            {
                PermissionKeysId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                RolesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PermissionKeyRole", x => new { x.PermissionKeysId, x.RolesId });
                table.ForeignKey(
                    name: "FK_PermissionKeyRole_PermissionKeys_PermissionKeysId",
                    column: x => x.PermissionKeysId,
                    principalTable: "PermissionKeys",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_PermissionKeyRole_Roles_RolesId",
                    column: x => x.RolesId,
                    principalTable: "Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.InsertData(
            table: "PermissionKeys",
            columns: new[] { "Id", "Value" },
            values: new object[,]
            {
                { new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"), "CanCreateAdmin" },
                { new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61"), "CanCreateHomeOwner" },
                { new Guid("e43167ad-158b-4a39-8f5d-c0a69b32d7cf"), "CanCreateCompanyOwner" }
            });

        migrationBuilder.InsertData(
            table: "PermissionKeyRole",
            columns: new[] { "PermissionKeysId", "RolesId" },
            values: new object[,]
            {
                { new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"), new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74") },
                { new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61"), new Guid("e43167ad-158b-4a39-8f5d-c0a69b32d7cf") },
                { new Guid("e43167ad-158b-4a39-8f5d-c0a69b32d7cf"), new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74") }
            });

        migrationBuilder.CreateIndex(
            name: "IX_PermissionKeyRole_RolesId",
            table: "PermissionKeyRole",
            column: "RolesId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "PermissionKeyRole");

        migrationBuilder.DeleteData(
            table: "PermissionKeys",
            keyColumn: "Id",
            keyValue: new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"));

        migrationBuilder.DeleteData(
            table: "PermissionKeys",
            keyColumn: "Id",
            keyValue: new Guid("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61"));

        migrationBuilder.DeleteData(
            table: "PermissionKeys",
            keyColumn: "Id",
            keyValue: new Guid("e43167ad-158b-4a39-8f5d-c0a69b32d7cf"));

        migrationBuilder.AlterColumn<string>(
            name: "Value",
            table: "PermissionKeys",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "Role",
            table: "PermissionKeys",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
    }
}
