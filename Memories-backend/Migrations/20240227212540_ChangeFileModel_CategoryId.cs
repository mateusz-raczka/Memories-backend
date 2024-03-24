using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Memoriesbackend.Migrations
{
    /// <inheritdoc />
    public partial class ChangeFileModelCategoryId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("1b16515f-2e32-45c5-b909-baff650edb41"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("3451a306-1260-4a18-90ee-8f2d57abec59"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("5da05bc3-84b2-4acb-ad49-359044cf2d41"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("7ded3d5f-eedb-46f9-b8cc-1e047684df3d"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("8629a3b9-ae58-4b92-bd0c-9c25ece0e123"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("da1ba639-0e9c-4f93-9d21-cdd5713ad24b"));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                    { new Guid("1b16515f-2e32-45c5-b909-baff650edb41"), "Edit" },
                    { new Guid("3451a306-1260-4a18-90ee-8f2d57abec59"), "Share" },
                    { new Guid("5da05bc3-84b2-4acb-ad49-359044cf2d41"), "Create" },
                    { new Guid("7ded3d5f-eedb-46f9-b8cc-1e047684df3d"), "Transfer" },
                    { new Guid("8629a3b9-ae58-4b92-bd0c-9c25ece0e123"), "Delete" },
                    { new Guid("da1ba639-0e9c-4f93-9d21-cdd5713ad24b"), "Open" }
                });
        }
    }
}
