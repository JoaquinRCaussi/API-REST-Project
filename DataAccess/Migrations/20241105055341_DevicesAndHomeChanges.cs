using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations;

/// <inheritdoc />
public partial class devicesandhomechanges : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "state",
            table: "HomeDevices",
            newName: "State");

        migrationBuilder.AddColumn<string>(
            name: "Name",
            table: "HomeDevices",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "RoomId",
            table: "HomeDevices",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Photo",
            table: "Devices",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.CreateTable(
            name: "Rooms",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                HomeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Rooms", x => x.Id);
                table.ForeignKey(
                    name: "FK_Rooms_Homes_HomeId",
                    column: x => x.HomeId,
                    principalTable: "Homes",
                    principalColumn: "Id");
            });

        migrationBuilder.InsertData(
            table: "Permissions",
            columns: new[] { "Id", "Value" },
            values: new object[,]
            {
                { new Guid("4c6b8b8d-3f92-4fda-87a4-202020202020"), "CanCreateRoom" },
                { new Guid("5d7fa8f6-ace7-42e5-8bdf-303030303030"), "CanAsociateDeviceToRoom" },
                { new Guid("6ebd4f21-3cd4-431f-97e7-404040404040"), "CanChangeHomeName" },
                { new Guid("7bcde1b8-5ad2-4f6c-92c7-505050505050"), "CanChangeHomeDevicesNames" }
            });

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"),
            column: "CreatedAt",
            value: new DateTime(2024, 11, 5, 2, 53, 40, 246, DateTimeKind.Local).AddTicks(8012));

        migrationBuilder.CreateIndex(
            name: "IX_HomeDevices_RoomId",
            table: "HomeDevices",
            column: "RoomId");

        migrationBuilder.CreateIndex(
            name: "IX_Rooms_HomeId",
            table: "Rooms",
            column: "HomeId");

        migrationBuilder.AddForeignKey(
            name: "FK_HomeDevices_Rooms_RoomId",
            table: "HomeDevices",
            column: "RoomId",
            principalTable: "Rooms",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_HomeDevices_Rooms_RoomId",
            table: "HomeDevices");

        migrationBuilder.DropTable(
            name: "Rooms");

        migrationBuilder.DropIndex(
            name: "IX_HomeDevices_RoomId",
            table: "HomeDevices");

        migrationBuilder.DeleteData(
            table: "Permissions",
            keyColumn: "Id",
            keyValue: new Guid("4c6b8b8d-3f92-4fda-87a4-202020202020"));

        migrationBuilder.DeleteData(
            table: "Permissions",
            keyColumn: "Id",
            keyValue: new Guid("5d7fa8f6-ace7-42e5-8bdf-303030303030"));

        migrationBuilder.DeleteData(
            table: "Permissions",
            keyColumn: "Id",
            keyValue: new Guid("6ebd4f21-3cd4-431f-97e7-404040404040"));

        migrationBuilder.DeleteData(
            table: "Permissions",
            keyColumn: "Id",
            keyValue: new Guid("7bcde1b8-5ad2-4f6c-92c7-505050505050"));

        migrationBuilder.DropColumn(
            name: "Name",
            table: "HomeDevices");

        migrationBuilder.DropColumn(
            name: "RoomId",
            table: "HomeDevices");

        migrationBuilder.RenameColumn(
            name: "State",
            table: "HomeDevices",
            newName: "state");

        migrationBuilder.AlterColumn<string>(
            name: "Photo",
            table: "Devices",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"),
            column: "CreatedAt",
            value: new DateTime(2024, 10, 8, 13, 44, 5, 927, DateTimeKind.Local).AddTicks(8950));
    }
}
