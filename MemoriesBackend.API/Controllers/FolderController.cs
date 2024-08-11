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

    public FolderController(
        IFolderDatabaseService folderDatabaseService,
        IMapper mapper
    )
    {
        _folderDatabaseService = folderDatabaseService;
        _mapper = mapper;
    }

    [HttpGet("root")]
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
    public async Task<FolderCreateResponse> Create([FromBody] FolderCreateRequest folderDto)
    {
        var folderDomain = _mapper.Map<Folder>(folderDto);

        var createdFolderDomain = await _folderDatabaseService.CreateFolderAsync(folderDomain);

        var response = _mapper.Map<FolderCreateResponse>(createdFolderDomain);

        return response;
    }
}