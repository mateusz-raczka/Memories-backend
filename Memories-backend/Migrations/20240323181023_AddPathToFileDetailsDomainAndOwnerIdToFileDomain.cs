using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Memoriesbackend.Migrations
{
    /// <inheritdoc />
    public partial class AddPathToFileDetailsDomainAndOwnerIdToFileDomain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("2c29e074-0766-4bc1-9fd6-8d29f1e51a69"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("600026f4-e9e9-49e9-86a3-31f9457aea5c"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("617be47e-23aa-4e0a-9072-9bfdad7f8c93"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("8e3b9329-4b0e-4dbf-9bf5-7ed4e86469d0"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("ab71fdc5-a0f8-4457-8d96-aef77903ba28"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("b7b1ba66-5692-4540-bb88-cf5ba303de00"));

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Files",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "FileDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "ActivityTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("4f021aa6-5ffa-4276-971b-04b34e6187e3"), "Transfer" },
                    { new Guid("624eab2e-c2cb-4106-9dc1-931d6af0e296"), "Edit" },
                    { new Guid("9e100488-4150-4b9e-8133-8e84996c743c"), "Open" },
                    { new Guid("a6bbe172-2685-4544-8650-3fe54d0f26da"), "Create" },
                    { new Guid("b9ca03f8-8c38-4fc5-94d3-334a5d0b14ec"), "Delete" },
                    { new Guid("ed53c997-0600-461f-adcf-43a03fe8d9de"), "Share" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("4f021aa6-5ffa-4276-971b-04b34e6187e3"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("624eab2e-c2cb-4106-9dc1-931d6af0e296"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("9e100488-4150-4b9e-8133-8e84996c743c"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("a6bbe172-2685-4544-8650-3fe54d0f26da"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("b9ca03f8-8c38-4fc5-94d3-334a5d0b14ec"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("ed53c997-0600-461f-adcf-43a03fe8d9de"));

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "FileDetails");

            migrationBuilder.InsertData(
                table: "ActivityTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("2c29e074-0766-4bc1-9fd6-8d29f1e51a69"), "Create" },
                    { new Guid("600026f4-e9e9-49e9-86a3-31f9457aea5c"), "Share" },
                    { new Guid("617be47e-23aa-4e0a-9072-9bfdad7f8c93"), "Transfer" },
                    { new Guid("8e3b9329-4b0e-4dbf-9bf5-7ed4e86469d0"), "Delete" },
                    { new Guid("ab71fdc5-a0f8-4457-8d96-aef77903ba28"), "Edit" },
                    { new Guid("b7b1ba66-5692-4540-bb88-cf5ba303de00"), "Open" }
                });
        }
    }
}
