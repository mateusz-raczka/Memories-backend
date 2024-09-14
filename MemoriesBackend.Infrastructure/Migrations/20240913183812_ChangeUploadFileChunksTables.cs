using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MemoriesBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUploadFileChunksTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DropColumn(
                name: "ChunkIndex",
                table: "FileUploadProgress");

            migrationBuilder.CreateTable(
                name: "FileChunks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    ChunkIndex = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileUploadProgressId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileChunks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileChunks_FileUploadProgress_FileUploadProgressId",
                        column: x => x.FileUploadProgressId,
                        principalTable: "FileUploadProgress",
                        principalColumn: "Id");
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_FileChunks_FileUploadProgressId",
                table: "FileChunks",
                column: "FileUploadProgressId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileChunks");

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

            migrationBuilder.AddColumn<int>(
                name: "ChunkIndex",
                table: "FileUploadProgress",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
    }
}
