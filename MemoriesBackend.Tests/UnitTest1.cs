using MemoriesBackend.Application.Services;
using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Repositories;
using MemoriesBackend.Domain.Interfaces.Services;
using Moq;

[TestFixture]
public class FolderManagementServiceTests
{
    private Mock<IFolderDatabaseService> _folderDatabaseServiceMock;
    private Mock<IFileManagementService> _fileManagementServiceMock;
    private Mock<IPathService> _pathServiceMock;
    private Mock<IFolderStorageService> _folderStorageServiceMock;
    private FolderManagementService _folderManagementService;
    private Mock<IFolderRepository> _folderRepositoryMock;

    [SetUp]
    public void Setup()
    {
        _folderDatabaseServiceMock = new Mock<IFolderDatabaseService>();
        _fileManagementServiceMock = new Mock<IFileManagementService>();
        _pathServiceMock = new Mock<IPathService>();
        _folderStorageServiceMock = new Mock<IFolderStorageService>();
        _folderRepositoryMock = new Mock<IFolderRepository>();

        _folderManagementService = new FolderManagementService(
            _fileManagementServiceMock.Object,
            _folderDatabaseServiceMock.Object,
            _pathServiceMock.Object,
            _folderStorageServiceMock.Object
        );
    }

    [Test]
    public async Task MoveFoldersAsync_ShouldMoveFolders_WhenFoldersAndTargetFolderExist()
    {
        // Arrange
        var folderIdToMove = Guid.NewGuid();
        var targetFolderId = Guid.NewGuid();

        // Set up the folders to move
        var foldersToMove = new List<Folder>
    {
        new Folder { Id = folderIdToMove, FolderDetails = new FolderDetails { Name = "Folder1" } }
    };

        var targetFolder = new Folder { Id = targetFolderId };

        // Mocking the repository methods
        _folderDatabaseServiceMock.Setup(f => f.GetFoldersByIdsWithContentAsync(It.IsAny<IEnumerable<Guid>>(), true))
            .ReturnsAsync(foldersToMove);

        _folderDatabaseServiceMock.Setup(f => f.GetFolderByIdWithContentAsync(targetFolderId, true))
            .ReturnsAsync(targetFolder);

        // Act
        var result = await _folderManagementService.MoveFoldersAsync(new List<Guid> { folderIdToMove }, targetFolderId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count());
        Assert.AreEqual(folderIdToMove, result.First().Id);
        _folderDatabaseServiceMock.Verify(f => f.MoveFolderSubTreeAsync(It.IsAny<Folder>(), It.IsAny<Folder>()), Times.Once);
        _folderDatabaseServiceMock.Verify(f => f.SaveAsync(), Times.Once);
    }
}
