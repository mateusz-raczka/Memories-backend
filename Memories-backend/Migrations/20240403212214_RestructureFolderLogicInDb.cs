using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Memoriesbackend.Migrations
{
    /// <inheritdoc />
    public partial class RestructureFolderLogicInDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("1fc24d92-1748-4397-ba91-c3552941e66a"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("77cfcb13-8b1e-40b4-ab0a-94b906895ac1"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("889f57c2-006f-477a-acad-830e8315e9eb"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("c6ce1f38-f793-4425-a59b-b7c445da69ca"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("cfa106ad-1d5e-41fc-a4f7-f56f4ae1b102"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("d62325ed-4be2-4194-a4c4-3cb386a3f384"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1004e7d9-9d08-45bd-84ce-827cf22e228c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "940cea9f-137f-4b7c-8236-d13a0ee4a174");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f5cfd13e-d42b-47e8-aed3-95f9c17b9dcf");

            migrationBuilder.DropColumn(
                name: "isFolder",
                table: "Files");

            migrationBuilder.InsertData(
                table: "ActivityTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("0a92ef87-08da-42df-b183-139bb63d1317"), "Share" },
                    { new Guid("0c264378-418c-4a68-840d-32f3b464efbe"), "Create" },
                    { new Guid("1bb7fbcf-f9e1-4fe2-9441-135c31740093"), "Edit" },
                    { new Guid("7d8bb8ea-8a0e-4a53-be10-1c92ac62570c"), "Open" },
                    { new Guid("ddf7740b-cf23-4e05-806d-59f5a8ba729a"), "Transfer" },
                    { new Guid("df6d5af5-e5fe-44b5-a464-88529953b1d5"), "Delete" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "92f71e3c-93c2-4b76-8781-5cf8c5fdaf90", null, "ADMIN", "ADMIN" },
                    { "d0da70cb-7ad4-44a0-97f5-6ec51a9d4b74", null, "USER", "USER" },
                    { "dad7f82a-81eb-4ae6-ad82-b5286addf3ea", null, "OWNER", "OWNER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("0a92ef87-08da-42df-b183-139bb63d1317"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("0c264378-418c-4a68-840d-32f3b464efbe"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("1bb7fbcf-f9e1-4fe2-9441-135c31740093"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("7d8bb8ea-8a0e-4a53-be10-1c92ac62570c"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("ddf7740b-cf23-4e05-806d-59f5a8ba729a"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("df6d5af5-e5fe-44b5-a464-88529953b1d5"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "92f71e3c-93c2-4b76-8781-5cf8c5fdaf90");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d0da70cb-7ad4-44a0-97f5-6ec51a9d4b74");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dad7f82a-81eb-4ae6-ad82-b5286addf3ea");

            migrationBuilder.AddColumn<bool>(
                name: "isFolder",
                table: "Files",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "ActivityTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("1fc24d92-1748-4397-ba91-c3552941e66a"), "Share" },
                    { new Guid("77cfcb13-8b1e-40b4-ab0a-94b906895ac1"), "Transfer" },
                    { new Guid("889f57c2-006f-477a-acad-830e8315e9eb"), "Create" },
                    { new Guid("c6ce1f38-f793-4425-a59b-b7c445da69ca"), "Delete" },
                    { new Guid("cfa106ad-1d5e-41fc-a4f7-f56f4ae1b102"), "Open" },
                    { new Guid("d62325ed-4be2-4194-a4c4-3cb386a3f384"), "Edit" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1004e7d9-9d08-45bd-84ce-827cf22e228c", null, "OWNER", "OWNER" },
                    { "940cea9f-137f-4b7c-8236-d13a0ee4a174", null, "ADMIN", "ADMIN" },
                    { "f5cfd13e-d42b-47e8-aed3-95f9c17b9dcf", null, "USER", "USER" }
                });
        }
    }
}
