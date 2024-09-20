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
    [Route("api")]
    [ApiController]
    public class OperationsController : Controller
    {

        private readonly IFolderAndFileManagementService _folderAndFileManagementService;
        private readonly IFolderManagementService _folderManagementService;
        private readonly IFileManagementService _fileManagementService;
        private readonly IMapper _mapper;
        public OperationsController(
            IFolderAndFileManagementService folderAndFileManagementService,
            IMapper mapper
            )
        {
            _folderAndFileManagementService = folderAndFileManagementService;
            _mapper = mapper;
        }

        [HttpPost("copy/file")]
        public async Task<IEnumerable<FileCopyPasteResponse>> CopyAndPaste(FileCopyAndPasteRequest fileCopyAndPasteRequest)
        {
            var filesDomain = await _fileManagementService.CopyAndPasteFilesAsync(fileCopyAndPasteRequest.FilesIds, fileCopyAndPasteRequest.TargetFolderId);

            var response = _mapper.Map<IEnumerable<FileCopyPasteResponse>>(filesDomain);

            return response;
        }

        [HttpPost("copy/folder")]
        public async Task<FolderCopyAndPasteResponse> CopyAndPaste([FromBody] FolderCopyAndPasteRequest folderCopyAndPasteDto)
        {
            var folderDomain = await _folderManagementService.CopyAndPasteFolderAsync(folderCopyAndPasteDto.SourceFolderId, folderCopyAndPasteDto.TargetFolderId);

            var response = _mapper.Map<FolderCopyAndPasteResponse>(folderDomain);

            return response;
        }

        [HttpPost("copy/foldersandfiles")]
        public async Task<FoldersAndFilesCopyAndPasteResponse> CopyAndPaste([FromBody] FoldersAndFilesCopyAndPasteRequest foldersAndFilesCopyAndPasteDto)
        {
            var folderDomain = await _folderAndFileManagementService.CopyAndPasteFoldersAndFilesAsync(
                foldersAndFilesCopyAndPasteDto.FilesIds,
                foldersAndFilesCopyAndPasteDto.FoldersIds,
                foldersAndFilesCopyAndPasteDto.TargetFolderId
                );

            var response = _mapper.Map<FoldersAndFilesCopyAndPasteResponse>(folderDomain);

            return response;
        }

        [HttpPost("cut/file")]
        public async Task<IEnumerable<FileCutAndPasteResponse>> CutAndPaste(FileCutAndPasteRequest fileCutAndPasteRequest)
        {
            var filesDomain = await _fileManagementService.CutAndPasteFilesAsync(fileCutAndPasteRequest.FilesIds, fileCutAndPasteRequest.TargetFolderId);

            var response = _mapper.Map<IEnumerable<FileCutAndPasteResponse>>(filesDomain);

            return response;
        }

        [HttpPost("cut/folder")]
        public async Task<FolderCutAndPasteResponse> CutAndPaste([FromBody] FolderCutAndPasteRequest folderCutAndPasteDto)
        {
            var folderDomain = await _folderManagementService.CutAndPasteFolderAsync(folderCutAndPasteDto.SourceFolderId, folderCutAndPasteDto.TargetFolderId);

            var response = _mapper.Map<FolderCutAndPasteResponse>(folderDomain);

            return response;
        }

        [HttpPost("cut/foldersandfiles")]
        public async Task<FoldersAndFilesCutAndPasteResponse> CutAndPaste([FromBody] FoldersAndFilesCutAndPasteRequest foldersAndFilesCutAndPasteDto)
        {
            var folderDomain = await _folderAndFileManagementService.CutAndPasteFoldersAndFilesAsync(
                foldersAndFilesCutAndPasteDto.FilesIds,
                foldersAndFilesCutAndPasteDto.FoldersIds,
                foldersAndFilesCutAndPasteDto.TargetFolderId
                );

            var response = _mapper.Map<FoldersAndFilesCutAndPasteResponse>(folderDomain);

            return response;
        }
    }
}
