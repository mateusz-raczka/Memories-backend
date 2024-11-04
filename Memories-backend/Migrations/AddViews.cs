using Microsoft.EntityFrameworkCore.Migrations;

namespace Memories_backend.Migrations
{
    public class AddViews : Migration
    {
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
