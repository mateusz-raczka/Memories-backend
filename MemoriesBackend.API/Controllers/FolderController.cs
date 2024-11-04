using AutoMapper;
using MemoriesBackend.API.DTO.Folder.Request;
using MemoriesBackend.API.DTO.Folder.Response;
using MemoriesBackend.API.DTO.FolderDetails.Request;
using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

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

    [HttpPatch("Rename")]
    public async Task RenameFolder([FromBody] FolderDetailsNamePatchRequest folderDetailsDto)
    {
        var folderDetailsDomain = _mapper.Map<FolderDetails>(folderDetailsDto);

        _folderDatabaseService.PatchFolderDetails(folderDetailsDomain, 
            fd => fd.Name
            );

        await _folderDatabaseService.SaveAsync();
    }

    [HttpPatch("Star/{id:Guid}")]
    public async Task ChangeFolderIsStared(Guid id)
    {
        await _folderDatabaseService.SwitchFolderStarAsync(id);

        await _folderDatabaseService.SaveAsync();
    }

    [HttpGet]
    public async Task<FolderGetByIdResponse> GetRootFolder()
    {
        var folderDomain = await _folderDatabaseService.GetRootFolderAsync();

        var response = _mapper.Map<FolderGetByIdResponse>(folderDomain);

        return response;
    }

    [HttpGet("{id:Guid}")]
    public async Task<FolderGetByIdResponse> GetFolderById(Guid id)
    {
        var folderDomain = await _folderDatabaseService.GetFolderByIdWithContentAsync(id);

        var response = _mapper.Map<FolderGetByIdResponse>(folderDomain);

        return response;
    }

    [HttpGet("Path/{id:Guid}")]
    public async Task<FolderGetByIdWithPathResponse> GetFolderByIdWithPath(Guid id)
    {
        var folderDomain = await _folderDatabaseService.GetFolderByIdWithContentAndDescendants(id);

        var response = _mapper.Map<FolderGetByIdWithPathResponse>(folderDomain);

        return response;
    }

    [HttpPost]
    public async Task<FolderAddResponse> CreateFolder([FromBody] FolderAddRequest folderCreateDto)
    {
        var folderDomain = _mapper.Map<Folder>(folderCreateDto);

        var createdFolderDomain = await _folderDatabaseService.CreateFolderAsync(folderDomain);

        await _folderDatabaseService.SaveAsync();

        var response = _mapper.Map<FolderAddResponse>(createdFolderDomain);

        return response;
    }

    [HttpDelete("{id:Guid}")]
    public async Task DeleteFolder(Guid id)
    {
        await _folderManagementService.DeleteFolderAsync(id);
    }
}