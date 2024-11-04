using AutoMapper;
using MemoriesBackend.API.DTO.ShareFile.Request;
using MemoriesBackend.API.DTO.ShareFile.Response;
using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace MemoriesBackend.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShareController : ControllerBase
{
    private readonly IShareService _shareService;
    private readonly IMapper _mapper;

    public ShareController(
        IShareService shareService,
        IMapper mapper
        )
    {
        _mapper = mapper;
        _shareService = shareService;
    } 

    [HttpPost]
    public async Task<ShareFileAddResponse> Share(ShareFileAddRequest shareFileDto)
    {
        var shareDomain = _mapper.Map<ShareFile>(shareFileDto);

        var createdShareDomain = await _shareService.ShareFileAsync(shareDomain);

        var response = _mapper.Map<ShareFileAddResponse>(createdShareDomain);

        return response;
    }
}
