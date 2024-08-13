using AutoMapper;
using MemoriesBackend.API.DTO.Folder.Request;
using MemoriesBackend.API.DTO.Folder.Response;
using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace MemoriesBackend.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FolderController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IFolderDatabaseService _folderDatabaseService;
    private readonly IFileManagementSystemService _fileManagementSystemService;

    public FolderController(
        IFolderDatabaseService folderDatabaseService,
        IFileManagementSystemService fileManagementSystemService,
        IMapper mapper
    )
    {
        _folderDatabaseService = folderDatabaseService;
        _fileManagementSystemService = fileManagementSystemService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<FolderGetByIdResponse> GetRootFolder()
    {
        var folderDomain = await _folderDatabaseService.GetRootFolderAsync();

        var response = _mapper.Map<FolderGetByIdResponse>(folderDomain);

        return response;
    }

    [HttpGet("{id:Guid}")]
    public async Task<FolderGetByIdResponse> GetById(Guid id)
    {
        var folderDomain = await _folderDatabaseService.GetFolderByIdAsync(id);

        var response = _mapper.Map<FolderGetByIdResponse>(folderDomain);

        return response;
    }

    [HttpPost]
    public async Task<FolderCreateResponse> Create([FromBody] FolderCreateRequest folderCreateDto)
    {
        var folderDomain = _mapper.Map<Folder>(folderCreateDto);

        var createdFolderDomain = await _folderDatabaseService.CreateFolderAsync(folderDomain);

        var response = _mapper.Map<FolderCreateResponse>(createdFolderDomain);

        return response;
    }

    [HttpPost("copy")]
    public async Task<FolderCreateResponse> CopyAndPaste([FromBody] FolderCopyAndPasteRequest folderCopyPasteDto)
    {
        var folderDomain = await _fileManagementSystemService.CopyAndPasteFolderAsync(folderCopyPasteDto.SourceFolderId, folderCopyPasteDto.TargetFolderId);

        var response = _mapper.Map<FolderCreateResponse>(folderDomain);

        return response;
    }
}