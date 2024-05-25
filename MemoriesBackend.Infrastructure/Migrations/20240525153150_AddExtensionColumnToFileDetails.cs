using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MemoriesBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExtensionColumnToFileDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("12501170-30da-42eb-a529-a174d16e519c"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("7234c4e8-1cfb-487e-be3e-6772a6b63d4e"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("76ae3981-7ea8-42e6-aca2-573f73a8f944"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("7d323da7-dc83-4251-8af7-a7a5f3db87fb"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("a8360ec2-1a70-4709-81ea-2fa9c4f36766"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("e6a4bbb3-8378-454f-b110-429f6fcb6e14"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "148089b3-e395-44cc-b2d5-01b90f526ac0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "191e9849-fe92-4300-a570-21659b6a28e4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8a0c1404-f382-4f7b-835f-028adf9d65d4");

            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "FileDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "ActivityTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("21c3a581-6296-4079-af68-b92dd125b13d"), "Open" },
                    { new Guid("4581b755-b0e0-4d0f-bb2e-eaee92118d67"), "Create" },
                    { new Guid("693e4946-385a-45dc-a654-2ef223abff11"), "Share" },
                    { new Guid("786e920b-6bd7-4d1e-b0c8-4317ede4f68e"), "Transfer" },
                    { new Guid("803057a3-3d8e-4b3d-96b2-a75583a61cbf"), "Edit" },
                    { new Guid("9d82aa93-e8b0-43c0-b2ba-b5fea24abf1e"), "Delete" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0c6c8009-483b-4453-97a6-fa82472a0ef8", null, "ADMIN", "ADMIN" },
                    { "2f242ee9-9ad0-4fa2-b8b5-7f9ca9b55c90", null, "USER", "USER" },
                    { "d836e159-97d7-4a37-bccc-4e4bb26d2455", null, "OWNER", "OWNER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("21c3a581-6296-4079-af68-b92dd125b13d"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("4581b755-b0e0-4d0f-bb2e-eaee92118d67"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("693e4946-385a-45dc-a654-2ef223abff11"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("786e920b-6bd7-4d1e-b0c8-4317ede4f68e"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("803057a3-3d8e-4b3d-96b2-a75583a61cbf"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("9d82aa93-e8b0-43c0-b2ba-b5fea24abf1e"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0c6c8009-483b-4453-97a6-fa82472a0ef8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2f242ee9-9ad0-4fa2-b8b5-7f9ca9b55c90");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d836e159-97d7-4a37-bccc-4e4bb26d2455");

            migrationBuilder.DropColumn(
                name: "Extension",
                table: "FileDetails");

            migrationBuilder.InsertData(
                table: "ActivityTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("12501170-30da-42eb-a529-a174d16e519c"), "Transfer" },
                    { new Guid("7234c4e8-1cfb-487e-be3e-6772a6b63d4e"), "Open" },
                    { new Guid("76ae3981-7ea8-42e6-aca2-573f73a8f944"), "Share" },
                    { new Guid("7d323da7-dc83-4251-8af7-a7a5f3db87fb"), "Edit" },
                    { new Guid("a8360ec2-1a70-4709-81ea-2fa9c4f36766"), "Delete" },
                    { new Guid("e6a4bbb3-8378-454f-b110-429f6fcb6e14"), "Create" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "148089b3-e395-44cc-b2d5-01b90f526ac0", null, "USER", "USER" },
                    { "191e9849-fe92-4300-a570-21659b6a28e4", null, "ADMIN", "ADMIN" },
                    { "8a0c1404-f382-4f7b-835f-028adf9d65d4", null, "OWNER", "OWNER" }
                });
        }
    }
}
