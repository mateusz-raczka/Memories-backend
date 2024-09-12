using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MemoriesBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFileUploadProgressTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("14234fd7-01f2-402d-947e-d9c068dec679"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("72269638-224e-4c5d-89d4-01a30edaee07"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("a489ba86-3d87-4be5-a16e-05564cd60fcf"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("b2197da5-463a-4621-bfc1-ed38ec1decac"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("c95d5b6f-730d-46d1-9d79-eeb71977f84c"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("d5cdbc00-39a1-4a60-9ca8-657cb827a0c5"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "43d428da-0b3b-4efa-9e13-2c5008ec0a4e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "91c5ef10-08a5-4fdf-a860-0fc201680585");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "96d34c0f-fba6-4cc2-913c-1f0378784e03");

            migrationBuilder.CreateTable(
                name: "FileUploadProgress",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    ChunkIndex = table.Column<int>(type: "int", nullable: false),
                    TotalChunks = table.Column<int>(type: "int", nullable: false),
                    RelativePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileUploadProgress", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ActivityTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("0bb34b14-4956-482e-adc2-656118f403f8"), "Open" },
                    { new Guid("1be9dadf-54af-4b5e-9c7a-815cb0331151"), "Edit" },
                    { new Guid("3c030a6d-c66f-4fbf-a942-12410b45aee0"), "Share" },
                    { new Guid("4ffb5a4c-1377-48e4-9b3f-a0ed516d5636"), "Create" },
                    { new Guid("8c570b94-7809-4662-8d72-c25f90613808"), "Transfer" },
                    { new Guid("d5d42717-84a7-4403-9f45-c72bc38d9d65"), "Delete" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1b650316-85a8-4d19-a725-1f74b6b29560", null, "USER", "USER" },
                    { "ad994e65-6ad0-4451-8c83-711abbacb5a7", null, "OWNER", "OWNER" },
                    { "d4d75c56-7900-467f-b924-02858816a0d6", null, "ADMIN", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileUploadProgress");

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("0bb34b14-4956-482e-adc2-656118f403f8"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("1be9dadf-54af-4b5e-9c7a-815cb0331151"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("3c030a6d-c66f-4fbf-a942-12410b45aee0"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("4ffb5a4c-1377-48e4-9b3f-a0ed516d5636"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("8c570b94-7809-4662-8d72-c25f90613808"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("d5d42717-84a7-4403-9f45-c72bc38d9d65"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1b650316-85a8-4d19-a725-1f74b6b29560");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ad994e65-6ad0-4451-8c83-711abbacb5a7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d4d75c56-7900-467f-b924-02858816a0d6");

            migrationBuilder.InsertData(
                table: "ActivityTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("14234fd7-01f2-402d-947e-d9c068dec679"), "Create" },
                    { new Guid("72269638-224e-4c5d-89d4-01a30edaee07"), "Edit" },
                    { new Guid("a489ba86-3d87-4be5-a16e-05564cd60fcf"), "Share" },
                    { new Guid("b2197da5-463a-4621-bfc1-ed38ec1decac"), "Open" },
                    { new Guid("c95d5b6f-730d-46d1-9d79-eeb71977f84c"), "Transfer" },
                    { new Guid("d5cdbc00-39a1-4a60-9ca8-657cb827a0c5"), "Delete" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "43d428da-0b3b-4efa-9e13-2c5008ec0a4e", null, "ADMIN", "ADMIN" },
                    { "91c5ef10-08a5-4fdf-a860-0fc201680585", null, "OWNER", "OWNER" },
                    { "96d34c0f-fba6-4cc2-913c-1f0378784e03", null, "USER", "USER" }
                });
        }
    }
}
