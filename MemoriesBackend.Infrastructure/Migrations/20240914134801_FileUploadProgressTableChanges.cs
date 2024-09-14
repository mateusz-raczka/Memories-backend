using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MemoriesBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FileUploadProgressTableChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("1c82688d-81bc-4e6c-90f4-ce895090c57f"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("3bb09ecf-8d70-4eaa-b027-cbd074ca4e56"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("3e07d13d-368f-4445-9726-983b8219d24c"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("3f3c6141-744a-4dc2-bd68-488facc22d5c"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("7dce3f40-6ce5-4df6-b078-0749f38a99a4"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("a575b52b-a56f-495c-9998-706b6da45298"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "28619b03-1188-43ca-9ec7-0e5addb38be8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7da01bdd-1c50-4c78-afb3-a53affe68059");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aa1f9394-16ee-4387-9b1a-c13f517a6bc8");

            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "FileUploadProgress",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "FileUploadProgress",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "ActivityTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("02ff5702-3b3b-44ab-9794-5ebb479ccf47"), "Open" },
                    { new Guid("0be93035-7459-4419-b46b-a20a54a9db1c"), "Create" },
                    { new Guid("72e2daaf-7c5e-4011-9161-a4f617a831f3"), "Delete" },
                    { new Guid("a2f01a6e-fdc5-42f4-8eeb-8e95d83cfa6b"), "Transfer" },
                    { new Guid("deb95113-c6e5-429b-b6a5-8fc0f2c0e808"), "Edit" },
                    { new Guid("ea0d2cda-5131-4955-8434-8b6478aa9cb0"), "Share" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "35714ba5-89db-47f2-9593-9dc4475ff0e8", null, "ADMIN", "ADMIN" },
                    { "b429b068-c648-4006-a492-ddc8918e076a", null, "OWNER", "OWNER" },
                    { "e4c8b4fa-a821-4f49-b360-eb349432284a", null, "USER", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("02ff5702-3b3b-44ab-9794-5ebb479ccf47"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("0be93035-7459-4419-b46b-a20a54a9db1c"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("72e2daaf-7c5e-4011-9161-a4f617a831f3"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("a2f01a6e-fdc5-42f4-8eeb-8e95d83cfa6b"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("deb95113-c6e5-429b-b6a5-8fc0f2c0e808"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("ea0d2cda-5131-4955-8434-8b6478aa9cb0"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "35714ba5-89db-47f2-9593-9dc4475ff0e8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b429b068-c648-4006-a492-ddc8918e076a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e4c8b4fa-a821-4f49-b360-eb349432284a");

            migrationBuilder.DropColumn(
                name: "Extension",
                table: "FileUploadProgress");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "FileUploadProgress");

            migrationBuilder.InsertData(
                table: "ActivityTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("1c82688d-81bc-4e6c-90f4-ce895090c57f"), "Share" },
                    { new Guid("3bb09ecf-8d70-4eaa-b027-cbd074ca4e56"), "Delete" },
                    { new Guid("3e07d13d-368f-4445-9726-983b8219d24c"), "Edit" },
                    { new Guid("3f3c6141-744a-4dc2-bd68-488facc22d5c"), "Create" },
                    { new Guid("7dce3f40-6ce5-4df6-b078-0749f38a99a4"), "Open" },
                    { new Guid("a575b52b-a56f-495c-9998-706b6da45298"), "Transfer" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "28619b03-1188-43ca-9ec7-0e5addb38be8", null, "ADMIN", "ADMIN" },
                    { "7da01bdd-1c50-4c78-afb3-a53affe68059", null, "USER", "USER" },
                    { "aa1f9394-16ee-4387-9b1a-c13f517a6bc8", null, "OWNER", "OWNER" }
                });
        }
    }
}
