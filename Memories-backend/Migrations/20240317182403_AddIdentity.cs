using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Memoriesbackend.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("5a634db2-f3a6-4615-aaa7-2e953570c748"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("664e8143-d984-415b-948a-b224473b9b4c"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("8a88de3b-ab7d-4740-a782-1580d6945474"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("c99e5150-e895-4ef5-8136-6b39689d8ccf"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("dabccd5b-37ce-4c5d-8489-b157e8ed0dbd"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("eb93e4a8-5f5c-441d-862b-e85de8419fd3"));

            migrationBuilder.InsertData(
                table: "ActivityTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("0befa8f6-72d1-4597-9859-95f9413b61fe"), "Edit" },
                    { new Guid("3cfaf018-37d5-49a7-b1f2-4bbcf8f0b181"), "Open" },
                    { new Guid("3e992911-7a79-44eb-a0cd-4c0fe74ba441"), "Create" },
                    { new Guid("729b090a-63de-41ec-8896-ea8c33cede8e"), "Delete" },
                    { new Guid("85925c4e-9e90-4d43-88f0-b1463bb659ad"), "Transfer" },
                    { new Guid("dd2f2e3d-12a8-4c91-8300-67ecc29c916c"), "Share" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("0befa8f6-72d1-4597-9859-95f9413b61fe"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("3cfaf018-37d5-49a7-b1f2-4bbcf8f0b181"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("3e992911-7a79-44eb-a0cd-4c0fe74ba441"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("729b090a-63de-41ec-8896-ea8c33cede8e"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("85925c4e-9e90-4d43-88f0-b1463bb659ad"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("dd2f2e3d-12a8-4c91-8300-67ecc29c916c"));

            migrationBuilder.InsertData(
                table: "ActivityTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("5a634db2-f3a6-4615-aaa7-2e953570c748"), "Open" },
                    { new Guid("664e8143-d984-415b-948a-b224473b9b4c"), "Create" },
                    { new Guid("8a88de3b-ab7d-4740-a782-1580d6945474"), "Share" },
                    { new Guid("c99e5150-e895-4ef5-8136-6b39689d8ccf"), "Transfer" },
                    { new Guid("dabccd5b-37ce-4c5d-8489-b157e8ed0dbd"), "Edit" },
                    { new Guid("eb93e4a8-5f5c-441d-862b-e85de8419fd3"), "Delete" }
                });
        }
    }
}
