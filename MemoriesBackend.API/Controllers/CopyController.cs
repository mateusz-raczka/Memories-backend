using AutoMapper;
using MemoriesBackend.API.DTO.File.Request;
using MemoriesBackend.API.DTO.File.Response;
using MemoriesBackend.API.DTO.Folder.Request;
using MemoriesBackend.API.DTO.Folder.Response;
using MemoriesBackend.API.DTO.FolderAndFile.Request;
using MemoriesBackend.API.DTO.FolderAndFile.Response;
using MemoriesBackend.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace MemoriesBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CopyController : Controller
    {

        ICopyAndPasteService _copyAndPasteService;
        IMapper _mapper;
        public CopyController(
            ICopyAndPasteService copyAndPasteService,
            IMapper mapper
            )
        {
            _copyAndPasteService = copyAndPasteService;
            _mapper = mapper;
        }

        [HttpPost("file")]
        public async Task<IEnumerable<FileCopyPasteResponse>> CopyAndPaste(FileCopyAndPasteRequest fileCopyAndPasteRequest)
        {
            var filesDomain = await _copyAndPasteService.CopyAndPasteFilesAsync(fileCopyAndPasteRequest.FilesIds, fileCopyAndPasteRequest.TargetFolderId);

            var response = _mapper.Map<IEnumerable<FileCopyPasteResponse>>(filesDomain);

            return response;
        }

        [HttpPost("folder")]
        public async Task<FolderCopyAndPasteResponse> CopyAndPaste([FromBody] FolderCopyAndPasteRequest folderCopyAndPasteDto)
        {
            var folderDomain = await _copyAndPasteService.CopyAndPasteFolderAsync(folderCopyAndPasteDto.SourceFolderId, folderCopyAndPasteDto.TargetFolderId);

            var response = _mapper.Map<FolderCopyAndPasteResponse>(folderDomain);

            return response;
        }

        [HttpPost("foldersandfiles")]
        public async Task<FolderAndFileCopyAndPasteResponse> CopyAndPaste([FromBody] FolderAndFileCopyAndPasteRequest foldersAndFilesCopyAndPasteDto)
        {
            var folderDomain = await _copyAndPasteService.CopyAndPasteFoldersAndFilesAsync(
                foldersAndFilesCopyAndPasteDto.FilesIds, 
                foldersAndFilesCopyAndPasteDto.FoldersIds, 
                foldersAndFilesCopyAndPasteDto.TargetFolderId
                );

            var response = _mapper.Map<FolderAndFileCopyAndPasteResponse>(folderDomain);

            return response;
        }
    }
}
