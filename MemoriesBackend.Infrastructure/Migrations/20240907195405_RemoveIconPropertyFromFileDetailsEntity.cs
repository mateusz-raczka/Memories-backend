using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MemoriesBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIconPropertyFromFileDetailsEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Folders_OwnerId",
                table: "Folders");

            migrationBuilder.DropIndex(
                name: "IX_Files_OwnerId",
                table: "Files");

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("1db6d4f4-5271-4816-b276-8f29336ad4d4"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("33bc8c28-ddc4-4a7b-bc77-8ae24acf716d"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("417eb45c-5198-49bf-837d-be852c0e0095"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("7cd73cef-ce05-4c11-b0cf-3dc8fd3ee795"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("acd3928d-f5e4-45fb-b63d-096547d074b5"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("e420b96d-ebdf-42db-9177-df1b30634568"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2fc97ab1-5485-41fc-a4c5-23bbef93ef19");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9f5760c1-94ed-4cf3-abea-7ead46e0af2b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c2b690a1-fa71-46cc-9d4f-054997ce2d81");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "FileDetails");

            migrationBuilder.InsertData(
                table: "ActivityTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("010b9244-d4f2-4c9e-b235-ae064ac6823b"), "Share" },
                    { new Guid("200ed180-5a7b-469b-9cfc-105076f57b7e"), "Transfer" },
                    { new Guid("301879a9-ef65-457e-9b54-135111eb99ca"), "Delete" },
                    { new Guid("500f7468-213a-4c22-bf11-28224ecacedf"), "Edit" },
                    { new Guid("903c6fac-85e9-4ab8-a618-7fb836aaaa8e"), "Open" },
                    { new Guid("ff0a7133-0ee0-464c-bc2f-12191a7c7a91"), "Create" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "327e43ce-5d35-4150-a436-47d06fff4cf7", null, "ADMIN", "ADMIN" },
                    { "864b84de-3e1f-48c1-b656-b32e691f607b", null, "USER", "USER" },
                    { "e35aa4a5-eb7f-414a-98e9-896e65ff9ee3", null, "OWNER", "OWNER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("010b9244-d4f2-4c9e-b235-ae064ac6823b"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("200ed180-5a7b-469b-9cfc-105076f57b7e"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("301879a9-ef65-457e-9b54-135111eb99ca"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("500f7468-213a-4c22-bf11-28224ecacedf"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("903c6fac-85e9-4ab8-a618-7fb836aaaa8e"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("ff0a7133-0ee0-464c-bc2f-12191a7c7a91"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "327e43ce-5d35-4150-a436-47d06fff4cf7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "864b84de-3e1f-48c1-b656-b32e691f607b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e35aa4a5-eb7f-414a-98e9-896e65ff9ee3");

            migrationBuilder.AddColumn<byte[]>(
                name: "Icon",
                table: "FileDetails",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "ActivityTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("1db6d4f4-5271-4816-b276-8f29336ad4d4"), "Transfer" },
                    { new Guid("33bc8c28-ddc4-4a7b-bc77-8ae24acf716d"), "Create" },
                    { new Guid("417eb45c-5198-49bf-837d-be852c0e0095"), "Edit" },
                    { new Guid("7cd73cef-ce05-4c11-b0cf-3dc8fd3ee795"), "Share" },
                    { new Guid("acd3928d-f5e4-45fb-b63d-096547d074b5"), "Delete" },
                    { new Guid("e420b96d-ebdf-42db-9177-df1b30634568"), "Open" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2fc97ab1-5485-41fc-a4c5-23bbef93ef19", null, "OWNER", "OWNER" },
                    { "9f5760c1-94ed-4cf3-abea-7ead46e0af2b", null, "ADMIN", "ADMIN" },
                    { "c2b690a1-fa71-46cc-9d4f-054997ce2d81", null, "USER", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Folders_OwnerId",
                table: "Folders",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_OwnerId",
                table: "Files",
                column: "OwnerId");
        }
    }
}
