using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Memoriesbackend.Migrations
{
    /// <inheritdoc />
    public partial class AddNormalizedNameForSeededRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "ActivityTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("0456041f-ee87-459b-8bf7-9e490c7f356e"), "Edit" },
                    { new Guid("3ebd921e-1776-400c-90b2-63892fb873e6"), "Transfer" },
                    { new Guid("3fe4f7ab-f659-4ed9-88bf-956b6eed4d92"), "Delete" },
                    { new Guid("699e3cfb-4eb2-4caf-918b-d162998b04ca"), "Create" },
                    { new Guid("e1635e6c-36aa-4039-871a-9c3e4e24eba3"), "Open" },
                    { new Guid("ec9d13ee-2335-4e01-b042-28f3bb9e15f2"), "Share" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3ab1fe59-552c-4c79-8939-50664b7b6c5e", null, "OWNER", "OWNER" },
                    { "78c83e71-c300-493d-9809-14ef85019933", null, "USER", "USER" },
                    { "87c38d61-8fc0-464c-8cdf-7aaaacdebb4f", null, "ADMIN", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("0456041f-ee87-459b-8bf7-9e490c7f356e"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("3ebd921e-1776-400c-90b2-63892fb873e6"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("3fe4f7ab-f659-4ed9-88bf-956b6eed4d92"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("699e3cfb-4eb2-4caf-918b-d162998b04ca"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("e1635e6c-36aa-4039-871a-9c3e4e24eba3"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("ec9d13ee-2335-4e01-b042-28f3bb9e15f2"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3ab1fe59-552c-4c79-8939-50664b7b6c5e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "78c83e71-c300-493d-9809-14ef85019933");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "87c38d61-8fc0-464c-8cdf-7aaaacdebb4f");

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
    }
}
