using System.Linq.Expressions;
using AutoMapper;
using MemoriesBackend.API.DTO.Folder.Request;
using MemoriesBackend.API.DTO.Folder.Response;
using MemoriesBackend.Application.Interfaces.Services;
using MemoriesBackend.Domain.Entities;
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

    [HttpGet]
    public async Task<IEnumerable<FolderGetAllResponse>> GetAll(
        int? pageNumber,
        int? pageSize,
        string? filterName = null
    )
    {
        Expression<Func<Folder, bool>> filter = null;

        if (!string.IsNullOrEmpty(filterName)) filter = entity => entity.FolderDetails.Name.Contains(filterName);

        // Hardcoded order by name
        Func<IQueryable<Folder>, IOrderedQueryable<Folder>> orderBy = query =>
            query.OrderBy(entity => entity.FolderDetails.Name);

        var foldersDomain = await _folderDatabaseService.GetAllFoldersAsync(
            pageNumber,
            pageSize,
            filter,
            orderBy
        );

        var response = _mapper.Map<IEnumerable<FolderGetAllResponse>>(foldersDomain);

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