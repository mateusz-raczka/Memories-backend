using AutoMapper;
using MemoriesBackend.API.DTO.File.Request;
using MemoriesBackend.API.DTO.File.Response;
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
    private readonly IFileManagementSystemService _fileManagementService;
    private readonly IMapper _mapper;

    public FileController(
        IFileDatabaseService fileDatabaseService,
        IFileManagementSystemService fileManagementService,
        IMapper mapper
    )
    {
        _fileDatabaseService = fileDatabaseService;
        _fileManagementService = fileManagementService;
        _mapper = mapper;
    }

    [HttpGet("all")]
    public async Task<IEnumerable<FileGetAllResponse>> GetAll(
        int? pageNumber,
        int? pageSize,
        string? filterName = null
    )
    {
        Expression<Func<File, bool>> filter = null;

        if (!string.IsNullOrEmpty(filterName)) filter = entity => entity.FileDetails.Name.Contains(filterName);

        // Hardcoded orderby name
        Func<IQueryable<File>, IOrderedQueryable<File>> orderBy = query =>
            query.OrderBy(entity => entity.FileDetails.Name);

        var filesDomain = await _fileDatabaseService.GetAllFilesAsync(
            pageNumber,
            pageSize,
            filter,
            orderBy
        );

        var response = _mapper.Map<IEnumerable<FileGetAllResponse>>(filesDomain);

        return response;
    }

    [HttpGet("{id:Guid}")]
    public async Task<FileGetByIdResponse> GetById(Guid id)
    {
        var fileDomain = await _fileDatabaseService.GetFileByIdAsync(id);

        var response = _mapper.Map<FileGetByIdResponse>(fileDomain);

        return response;
    }

    [HttpPost("{folderId:Guid}")]
    public async Task<FileCreateResponse> Add([FromForm] IFormFile fileData, Guid folderId)
    {
        var fileDomain = await _fileManagementService.AddFileAsync(fileData, folderId);

        var response = _mapper.Map<FileCreateResponse>(fileDomain);

        return response;
    }

    [HttpPut("{id:Guid}")]
    public async Task Update(Guid id, [FromBody] FileUpdateRequest fileDto)
    {
        var fileDomain = _mapper.Map<File>(fileDto);

        await _fileDatabaseService.UpdateFileAsync(id, fileDomain);
    }

    [HttpDelete("{id:Guid}")]
    public async Task Delete(Guid id)
    {
        await _fileManagementService.DeleteFileAsync(id);
    }

    [HttpGet("Download/{id:Guid}")]
    public async Task<FileContentResult> Download(Guid id)
    {
        return await _fileManagementService.DownloadFileAsync(id);
    }

    [HttpPost("copy")]
    public async Task<IEnumerable<FileCopyPasteResponse>> CopyAndPaste(FileCopyPasteRequest fileCopyPasteRequest)
    {
        var filesDomain = await _fileManagementService.CopyAndPasteFilesAsync(fileCopyPasteRequest.FilesId, fileCopyPasteRequest.TargetFolderId);

        var response = _mapper.Map<IEnumerable<FileCopyPasteResponse>>(filesDomain);

        return response;
    }
}