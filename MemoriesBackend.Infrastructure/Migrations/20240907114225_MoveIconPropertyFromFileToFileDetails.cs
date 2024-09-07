using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MemoriesBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MoveIconPropertyFromFileToFileDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("04b96ec2-5b83-4083-8cd8-445c6a2c5a5d"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("275b32f8-fb31-411c-8e30-10301395d0f8"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("42dc048e-b0b0-466e-98c2-4509400db538"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("4bf75111-8b02-43b1-94bf-36c18176a677"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("5527e2d5-c750-4485-8e60-298fd195c692"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("b054deff-daaf-423a-b251-59e76fd57495"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "88e056e1-da42-49f9-a43f-0820af981a9a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b7d30042-d519-4b16-81b0-831aeb73ba68");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ea7c3918-49ca-42a1-9ca1-07d35f863d67");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Files");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<byte[]>(
                name: "Icon",
                table: "Files",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "ActivityTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("04b96ec2-5b83-4083-8cd8-445c6a2c5a5d"), "Transfer" },
                    { new Guid("275b32f8-fb31-411c-8e30-10301395d0f8"), "Delete" },
                    { new Guid("42dc048e-b0b0-466e-98c2-4509400db538"), "Create" },
                    { new Guid("4bf75111-8b02-43b1-94bf-36c18176a677"), "Share" },
                    { new Guid("5527e2d5-c750-4485-8e60-298fd195c692"), "Edit" },
                    { new Guid("b054deff-daaf-423a-b251-59e76fd57495"), "Open" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "88e056e1-da42-49f9-a43f-0820af981a9a", null, "OWNER", "OWNER" },
                    { "b7d30042-d519-4b16-81b0-831aeb73ba68", null, "USER", "USER" },
                    { "ea7c3918-49ca-42a1-9ca1-07d35f863d67", null, "ADMIN", "ADMIN" }
                });
        }
    }
}
