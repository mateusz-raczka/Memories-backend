using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MemoriesBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsLoggedInColumnToUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("1ea837d4-099b-4d8f-aa1f-a9c81c185ddf"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("4e8db2e6-b5aa-4a4e-a6d0-eefb5148e330"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("592538cb-b8c7-4f5e-823d-3777fb9e316a"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("7d1d718c-a74b-4498-a3e2-76653df74d91"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("9cb5e6df-264b-4e7b-b758-d93702f0d0b8"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("c52e7f68-5618-4efe-9241-063018d5a5fd"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "30e5912d-1756-492d-bd19-0792ad5be4c3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "356537eb-cd03-41b2-a98b-f9ad5c3e91e4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b338e502-9ed1-43da-86e1-924dad51229e");

            migrationBuilder.AddColumn<bool>(
                name: "isLoggedIn",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.InsertData(
                table: "ActivityTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("0a4944e0-e0f4-4704-b579-17850a13e268"), "Share" },
                    { new Guid("53be0b6c-a709-4514-89df-3fc6ddee4c67"), "Create" },
                    { new Guid("5437bab6-1b51-4b83-aebe-69cbe1b03ce2"), "Open" },
                    { new Guid("73ae8c33-8333-4cde-891e-ce37822bc5ef"), "Edit" },
                    { new Guid("80dac9c4-4f1b-4cc7-ae9d-5564e839ff24"), "Delete" },
                    { new Guid("ceea5d4f-8e22-4305-846a-a378acf637fa"), "Transfer" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6960da9c-a0e0-484f-84f2-01d68827b92b", null, "ADMIN", "ADMIN" },
                    { "802705b1-5781-46bd-a347-26bb2fd9f6a3", null, "USER", "USER" },
                    { "8cc64f82-c195-4893-8337-4404433276df", null, "OWNER", "OWNER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("0a4944e0-e0f4-4704-b579-17850a13e268"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("53be0b6c-a709-4514-89df-3fc6ddee4c67"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("5437bab6-1b51-4b83-aebe-69cbe1b03ce2"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("73ae8c33-8333-4cde-891e-ce37822bc5ef"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("80dac9c4-4f1b-4cc7-ae9d-5564e839ff24"));

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: new Guid("ceea5d4f-8e22-4305-846a-a378acf637fa"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6960da9c-a0e0-484f-84f2-01d68827b92b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "802705b1-5781-46bd-a347-26bb2fd9f6a3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8cc64f82-c195-4893-8337-4404433276df");

            migrationBuilder.DropColumn(
                name: "isLoggedIn",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "ActivityTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("1ea837d4-099b-4d8f-aa1f-a9c81c185ddf"), "Open" },
                    { new Guid("4e8db2e6-b5aa-4a4e-a6d0-eefb5148e330"), "Share" },
                    { new Guid("592538cb-b8c7-4f5e-823d-3777fb9e316a"), "Edit" },
                    { new Guid("7d1d718c-a74b-4498-a3e2-76653df74d91"), "Transfer" },
                    { new Guid("9cb5e6df-264b-4e7b-b758-d93702f0d0b8"), "Delete" },
                    { new Guid("c52e7f68-5618-4efe-9241-063018d5a5fd"), "Create" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "30e5912d-1756-492d-bd19-0792ad5be4c3", null, "OWNER", "OWNER" },
                    { "356537eb-cd03-41b2-a98b-f9ad5c3e91e4", null, "ADMIN", "ADMIN" },
                    { "b338e502-9ed1-43da-86e1-924dad51229e", null, "USER", "USER" }
                });
        }
    }
}
