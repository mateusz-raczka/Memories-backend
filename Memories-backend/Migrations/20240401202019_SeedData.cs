using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Memoriesbackend.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("4f021aa6-5ffa-4276-971b-04b34e6187e3"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("624eab2e-c2cb-4106-9dc1-931d6af0e296"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("9e100488-4150-4b9e-8133-8e84996c743c"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("a6bbe172-2685-4544-8650-3fe54d0f26da"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("b9ca03f8-8c38-4fc5-94d3-334a5d0b14ec"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("ed53c997-0600-461f-adcf-43a03fe8d9de"));

            migrationBuilder.AlterColumn<Guid>(
                name: "Path",
                table: "FileDetails",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "ActivityTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("0e5ffb13-3b3d-40ae-8cb4-f175c7ef1c62"), "Delete" },
                    { new Guid("3c096a43-c95d-4959-bde6-1a27554004b2"), "Edit" },
                    { new Guid("619400fb-d861-4413-8169-fb5fce9eb938"), "Create" },
                    { new Guid("70d5384a-c52a-458f-a584-4a0d63dd7b43"), "Open" },
                    { new Guid("d7ee145e-0491-47ec-9208-3a31977bc803"), "Share" },
                    { new Guid("fc106407-fdb9-4284-962b-a9bf4423ae0d"), "Transfer" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1dff3be2-02b5-4969-87e3-e8d15480d51f", null, "OWNER", null },
                    { "67b066e7-8846-427e-b2f2-69d4f3108eac", null, "ADMIN", null },
                    { "8a9ee048-b6eb-4042-b6b2-082a511d359f", null, "USER", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("0e5ffb13-3b3d-40ae-8cb4-f175c7ef1c62"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("3c096a43-c95d-4959-bde6-1a27554004b2"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("619400fb-d861-4413-8169-fb5fce9eb938"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("70d5384a-c52a-458f-a584-4a0d63dd7b43"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("d7ee145e-0491-47ec-9208-3a31977bc803"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("fc106407-fdb9-4284-962b-a9bf4423ae0d"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1dff3be2-02b5-4969-87e3-e8d15480d51f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "67b066e7-8846-427e-b2f2-69d4f3108eac");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8a9ee048-b6eb-4042-b6b2-082a511d359f");

            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "FileDetails",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.InsertData(
                table: "ActivityTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("4f021aa6-5ffa-4276-971b-04b34e6187e3"), "Transfer" },
                    { new Guid("624eab2e-c2cb-4106-9dc1-931d6af0e296"), "Edit" },
                    { new Guid("9e100488-4150-4b9e-8133-8e84996c743c"), "Open" },
                    { new Guid("a6bbe172-2685-4544-8650-3fe54d0f26da"), "Create" },
                    { new Guid("b9ca03f8-8c38-4fc5-94d3-334a5d0b14ec"), "Delete" },
                    { new Guid("ed53c997-0600-461f-adcf-43a03fe8d9de"), "Share" }
                });
        }
    }
}
