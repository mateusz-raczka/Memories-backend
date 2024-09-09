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
    private readonly IFileService _fileService;

    public FolderController(
        IFolderDatabaseService folderDatabaseService,
        IFileService fileService,
        IMapper mapper
    )
    {
        _folderDatabaseService = folderDatabaseService;
        _fileService = fileService;
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
        var folderDomain = await _folderDatabaseService.GetFolderByIdWithAllRelations(id);

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

    [HttpDelete("{id:Guid}")]
    public async Task Delete(Guid id)
    {
        //await _fileManagementSystemService.DeleteFolderAsync(id);
    }
}