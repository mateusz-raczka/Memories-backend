using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Memoriesbackend.Migrations
{
    /// <inheritdoc />
    public partial class ChangeFileModelCategoryIdv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("6e2aabcb-17a9-437e-b08b-641a9f70672e"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("7363a8e8-6901-4cf2-9420-187cdb770616"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("9dc6be93-197b-4d41-84e2-83076625b79d"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("e8251f01-3e02-42a5-8b7d-fa4009576e25"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("ef0d6a85-9045-479b-b023-80ed8cb5d070"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("f55a0907-f2de-43ee-8d1a-4fe2717feb10"));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                    { new Guid("6e2aabcb-17a9-437e-b08b-641a9f70672e"), "Share" },
                    { new Guid("7363a8e8-6901-4cf2-9420-187cdb770616"), "Delete" },
                    { new Guid("9dc6be93-197b-4d41-84e2-83076625b79d"), "Transfer" },
                    { new Guid("e8251f01-3e02-42a5-8b7d-fa4009576e25"), "Create" },
                    { new Guid("ef0d6a85-9045-479b-b023-80ed8cb5d070"), "Open" },
                    { new Guid("f55a0907-f2de-43ee-8d1a-4fe2717feb10"), "Edit" }
                });
        }
    }
}
