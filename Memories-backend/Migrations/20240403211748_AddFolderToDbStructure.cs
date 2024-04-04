using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Memoriesbackend.Migrations
{
    /// <inheritdoc />
    public partial class AddFolderToDbStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "Folder",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Folder_Folder_FolderId",
                        column: x => x.FolderId,
                        principalTable: "Folder",
                        principalColumn: "Id");
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Files_FolderId",
                table: "Files",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Folder_FolderId",
                table: "Folder",
                column: "FolderId");

            migrationBuilder.AddForeignKey(
                name: "FK_FileDetails_Folder_Id",
                table: "FileDetails",
                column: "Id",
                principalTable: "Folder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Folder_FolderId",
                table: "Files",
                column: "FolderId",
                principalTable: "Folder",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileDetails_Folder_Id",
                table: "FileDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_Folder_FolderId",
                table: "Files");

            migrationBuilder.DropTable(
                name: "Folder");

            migrationBuilder.DropIndex(
                name: "IX_Files_FolderId",
                table: "Files");

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
    }
}
