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
    private readonly IFolderManagementService _folderManagementService;

    public FolderController(
        IFolderDatabaseService folderDatabaseService,
        IFolderManagementService folderManagementService,
        IMapper mapper
    )
    {
        _folderDatabaseService = folderDatabaseService;
        _folderManagementService = folderManagementService;
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
        var folderDomain = await _folderDatabaseService.GetFolderByIdWithRelations(id);

        var response = _mapper.Map<FolderGetByIdResponse>(folderDomain);

        return response;
    }

    [HttpGet("path/{id:Guid}")]
    public async Task<FolderGetByIdWithPathResponse> GetByIdWithPath(Guid id)
    {
        var folderDomain = await _folderDatabaseService.GetFolderByIdWithRelationsAndDescendants(id);

        var response = _mapper.Map<FolderGetByIdWithPathResponse>(folderDomain);

        return response;
    }

    [HttpPost]
    public async Task<FolderAddResponse> Create([FromBody] FolderAddRequest folderCreateDto)
    {
        var folderDomain = _mapper.Map<Folder>(folderCreateDto);

        var createdFolderDomain = await _folderDatabaseService.CreateFolderAsync(folderDomain);

        await _folderDatabaseService.SaveAsync();

        var response = _mapper.Map<FolderAddResponse>(createdFolderDomain);

        return response;
    }

    [HttpDelete("{id:Guid}")]
    public async Task Delete(Guid id)
    {
        await _folderManagementService.DeleteFolderAsync(id);
    }
}