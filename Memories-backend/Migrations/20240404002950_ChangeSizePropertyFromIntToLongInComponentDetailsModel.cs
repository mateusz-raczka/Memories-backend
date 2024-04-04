using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Memoriesbackend.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSizePropertyFromIntToLongInComponentDetailsModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<long>(
                name: "Size",
                table: "FileDetails",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "ActivityTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("4a093e21-a1e6-4b6c-be41-07395450efd2"), "Delete" },
                    { new Guid("7929bb32-2b95-4ebf-8e53-7f8fabc237c5"), "Create" },
                    { new Guid("875efe3d-1ada-4ab7-a15b-64be3e351a20"), "Open" },
                    { new Guid("d4b72f18-39f3-4097-947d-aa7d8d66f5dd"), "Edit" },
                    { new Guid("e5897983-f7d8-4fc9-b3e4-53ee7f985a5e"), "Share" },
                    { new Guid("f5c8816a-afee-4830-adf2-607e8e6ca715"), "Transfer" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "c3623671-8716-46a4-975e-b7765d7852ef", null, "USER", "USER" },
                    { "cf97b70c-e175-424b-ac2f-2a89af690c0d", null, "ADMIN", "ADMIN" },
                    { "d7b81620-a137-4a9d-a859-4f2812cf4a22", null, "OWNER", "OWNER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("4a093e21-a1e6-4b6c-be41-07395450efd2"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("7929bb32-2b95-4ebf-8e53-7f8fabc237c5"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("875efe3d-1ada-4ab7-a15b-64be3e351a20"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("d4b72f18-39f3-4097-947d-aa7d8d66f5dd"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("e5897983-f7d8-4fc9-b3e4-53ee7f985a5e"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("f5c8816a-afee-4830-adf2-607e8e6ca715"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c3623671-8716-46a4-975e-b7765d7852ef");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cf97b70c-e175-424b-ac2f-2a89af690c0d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d7b81620-a137-4a9d-a859-4f2812cf4a22");

            migrationBuilder.AlterColumn<int>(
                name: "Size",
                table: "FileDetails",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

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
    }
}
