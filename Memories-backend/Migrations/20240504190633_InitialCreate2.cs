using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Memories_backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE GetFolderHierarchy 
                    @FolderId uniqueidentifier
                AS
                BEGIN
                    WITH RecursiveFolder AS (
                        SELECT Id, FolderId, CAST(Id AS NVARCHAR(MAX)) AS FolderPath
                        FROM Folders
                        WHERE Id = @FolderId
                        
                        UNION ALL
                        
                        SELECT f.Id, f.FolderId, CONCAT(r.FolderPath, '/', f.Id)
                        FROM Folders f
                        JOIN RecursiveFolder r ON r.FolderId = f.Id
                        WHERE f.Id <> @FolderId
                    )
                    SELECT Id, FolderId
                    FROM RecursiveFolder
                    ORDER BY FolderPath DESC;
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE GetFolderHierarchy");
        }
    }
}
