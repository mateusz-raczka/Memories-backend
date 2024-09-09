﻿using AutoMapper;
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
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;

    public FileController(
        IFileDatabaseService fileDatabaseService,
        IFileService fileService,
        IMapper mapper
    )
    {
        _fileDatabaseService = fileDatabaseService;
        _fileService = fileService;
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
    public async Task<FileCreateResponse> Add(IFormFile fileData, Guid folderId)
    {
        var fileDomain = await _fileService.AddFileAsync(fileData, folderId);

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
        await _fileService.DeleteFileAsync(id);
    }

    [HttpGet("Download/{id:Guid}")]
    public async Task<FileContentResult> Download(Guid id)
    {
        return await _fileService.DownloadFileAsync(id);
    }

    [HttpGet("Preview/{id:Guid}")]
    public async Task<FileStreamResult> Preview(Guid id)
    {
        return await _fileService.StreamFileAsync(id);
    }
}