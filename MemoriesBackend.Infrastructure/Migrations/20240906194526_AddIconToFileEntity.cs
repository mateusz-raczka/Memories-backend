using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MemoriesBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIconToFileEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("8b40164c-b022-4c97-8d23-2034b309a5e0"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("960063cb-9a42-48a4-94a9-c3a3e84fcff0"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("99d23f5f-74dc-44e7-8d33-069d74bf29c7"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("a454ee77-2e46-405f-8c3b-58af86065129"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("a76daeaf-f806-49c0-a31e-6a61a528fb33"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("e2d442a9-f85a-4335-9157-7746e3bb3f73"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "15c4dedc-6976-46dd-bb70-416bbcd3493c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1e463f8c-7aae-47a8-902d-a7418d16c9fb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4e64528a-d8d0-4bfa-a025-209a081f8e4c");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "ActivityTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("8b40164c-b022-4c97-8d23-2034b309a5e0"), "Open" },
                    { new Guid("960063cb-9a42-48a4-94a9-c3a3e84fcff0"), "Delete" },
                    { new Guid("99d23f5f-74dc-44e7-8d33-069d74bf29c7"), "Create" },
                    { new Guid("a454ee77-2e46-405f-8c3b-58af86065129"), "Edit" },
                    { new Guid("a76daeaf-f806-49c0-a31e-6a61a528fb33"), "Transfer" },
                    { new Guid("e2d442a9-f85a-4335-9157-7746e3bb3f73"), "Share" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "15c4dedc-6976-46dd-bb70-416bbcd3493c", null, "ADMIN", "ADMIN" },
                    { "1e463f8c-7aae-47a8-902d-a7418d16c9fb", null, "OWNER", "OWNER" },
                    { "4e64528a-d8d0-4bfa-a025-209a081f8e4c", null, "USER", "USER" }
                });
        }
    }
}
