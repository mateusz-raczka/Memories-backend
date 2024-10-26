using AutoMapper;
using MemoriesBackend.API.DTO.File.Response;
using MemoriesBackend.API.DTO.FileDetails.Request;
using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using File = MemoriesBackend.Domain.Entities.File;

namespace MemoriesBackend.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FileController : ControllerBase
{
    private readonly IFileDatabaseService _fileDatabaseService;
    private readonly IFileManagementService _fileManagementService;
    private readonly IMapper _mapper;

    public FileController(
        IFileDatabaseService fileDatabaseService,
        IFileManagementService fileManagementService,
        IMapper mapper
    )
    {
        _fileDatabaseService = fileDatabaseService;
        _fileManagementService = fileManagementService;
        _mapper = mapper;
    }

    [HttpGet("all")]
    public async Task<IEnumerable<FileGetAllResponse>> GetAllFiles(
        int? pageNumber,
        int? pageSize,
        string? filterName = null
    )
    {
        Expression<Func<File, bool>> filter = null;

        if (!string.IsNullOrEmpty(filterName)) filter = entity => entity.FileDetails.Name.Contains(filterName);

        Func<IQueryable<File>, IOrderedQueryable<File>> orderBy = query =>
            query.OrderBy(entity => entity.FileDetails.Name);

        var filesDomain = await _fileDatabaseService.GetAllFilesAsync(
            filter,
            pageNumber,
            pageSize,
            orderBy,
            true
        );

        var response = _mapper.Map<IEnumerable<FileGetAllResponse>>(filesDomain);

        return response;
    }

    [HttpGet("{id:Guid}")]
    public async Task<FileGetByIdResponse> GetFileById(Guid id)
    {
        var fileDomain = await _fileDatabaseService.GetFileByIdWithDetailsAsync(id);

        var response = _mapper.Map<FileGetByIdResponse>(fileDomain);

        return response;
    }

    [HttpPost("{folderId:Guid}")]
    public async Task<FileAddResponse> AddFile(IFormFile fileData, Guid folderId)
    {
        var fileDomain = await _fileManagementService.AddFileAsync(fileData, folderId);

        var response = _mapper.Map<FileAddResponse>(fileDomain);

        return response;
    }

    [HttpPost("Chunk")]
    public async Task<FileAddResponse> AddFileChunk(IFormFile fileData, [FromForm] string fileName, [FromForm] int chunkIndex, [FromForm] int totalChunks, [FromForm] Guid folderId, [FromForm] Guid fileId)
    {
        using (var stream = fileData.OpenReadStream())
        {

            var fileDomain = await _fileManagementService.AddFileUsingChunksAsync(stream, fileName, chunkIndex, totalChunks, folderId, fileId);

            var response = _mapper.Map<FileAddResponse>(fileDomain);

            return response;
        }
    }

    [HttpPatch("Rename")]
    public async Task RenameFile([FromBody] FileDetailsNamePatchRequest fileDetailsDto)
    {
        var fileDetailsDomain = _mapper.Map<FileDetails>(fileDetailsDto);

        _fileDatabaseService.PatchFileDetails(fileDetailsDomain,
            fd => fd.Name
            );

        await _fileDatabaseService.SaveAsync();
    }

    [HttpPatch("Star")]
    public async Task ChangeFileIsStared([FromBody] FileDetailsIsStaredPatchRequest fileDetailsDto)
    {
        var fileDetailsDomain = _mapper.Map<FileDetails>(fileDetailsDto);

        _fileDatabaseService.PatchFileDetails(fileDetailsDomain,
            fd => fd.IsStared
            );

        await _fileDatabaseService.SaveAsync();
    }

    [HttpPatch("Description")]
    public async Task ChangeFileDescription([FromBody] FileDetailsDescriptionPatchRequest fileDetailsDto)
    {
        var fileDetailsDomain = _mapper.Map<FileDetails>(fileDetailsDto);

        _fileDatabaseService.PatchFileDetails(fileDetailsDomain,
            fd => fd.Description
            );

        await _fileDatabaseService.SaveAsync();
    }

    [HttpDelete("{id:Guid}")]
    public async Task DeleteFile(Guid id)
    {
        await _fileManagementService.DeleteFileAsync(id);
    }

    [HttpGet("Download/{id:Guid}")]
    public async Task<FileContentResult> DownloadFile(Guid id)
    {
        return await _fileManagementService.DownloadFileAsync(id);
    }

    [HttpGet("Preview/{id:Guid}")]
    public async Task<FileStreamResult> PreviewFile(Guid id)
    {
        return await _fileManagementService.StreamFileAsync(id);
    }
}