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

        IFileManagementSystemService _fileManagementSystemService;
        IMapper _mapper;
        public CopyController(
            IFileManagementSystemService fileManagementSystemService,
            IMapper mapper
            )
        { 
            _fileManagementSystemService = fileManagementSystemService;
            _mapper = mapper;
        }

        [HttpPost("file")]
        public async Task<IEnumerable<FileCopyPasteResponse>> CopyAndPaste(FileCopyAndPasteRequest fileCopyAndPasteRequest)
        {
            var filesDomain = await _fileManagementSystemService.CopyAndPasteFilesAsync(fileCopyAndPasteRequest.FilesIds, fileCopyAndPasteRequest.TargetFolderId);

            var response = _mapper.Map<IEnumerable<FileCopyPasteResponse>>(filesDomain);

            return response;
        }

        [HttpPost("folder")]
        public async Task<FolderCopyAndPasteResponse> CopyAndPaste([FromBody] FolderCopyAndPasteRequest folderCopyAndPasteDto)
        {
            var folderDomain = await _fileManagementSystemService.CopyAndPasteFolderAsync(folderCopyAndPasteDto.SourceFolderId, folderCopyAndPasteDto.TargetFolderId);

            var response = _mapper.Map<FolderCopyAndPasteResponse>(folderDomain);

            return response;
        }

        [HttpPost("foldersandfiles")]
        public async Task<FolderAndFileCopyAndPasteResponse> CopyAndPaste([FromBody] FolderAndFileCopyAndPasteRequest foldersAndFilesCopyAndPasteDto)
        {
            var folderDomain = await _fileManagementSystemService.CopyAndPasteFoldersAndFilesAsync(
                foldersAndFilesCopyAndPasteDto.FilesIds, 
                foldersAndFilesCopyAndPasteDto.FoldersIds, 
                foldersAndFilesCopyAndPasteDto.TargetFolderId
                );

            var response = _mapper.Map<FolderAndFileCopyAndPasteResponse>(folderDomain);

            return response;
        }
    }
}
