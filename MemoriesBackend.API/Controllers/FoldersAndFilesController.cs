using AutoMapper;
using MemoriesBackend.API.DTO.FolderAndFile.Request;
using MemoriesBackend.API.DTO.FolderAndFile.Response;
using MemoriesBackend.API.DTO.FoldersAndFiles.Request;
using MemoriesBackend.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace MemoriesBackend.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class FoldersAndFilesController : Controller
    {

        private readonly IFolderAndFileManagementService _folderAndFileManagementService;
        private readonly IMapper _mapper;
        public FoldersAndFilesController(
            IFolderAndFileManagementService folderAndFileManagementService,
            IMapper mapper
            )
        {
            _folderAndFileManagementService = folderAndFileManagementService;
            _mapper = mapper;
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

        [HttpPost("cut/foldersandfiles")]
        public async Task<FoldersAndFilesCutAndPasteResponse> Move([FromBody] FoldersAndFilesCutAndPasteRequest foldersAndFilesCutAndPasteDto)
        {
            var folderDomain = await _folderAndFileManagementService.MoveFoldersAndFilesAsync(
                foldersAndFilesCutAndPasteDto.FilesIds,
                foldersAndFilesCutAndPasteDto.FoldersIds,
                foldersAndFilesCutAndPasteDto.TargetFolderId
                );

            var response = _mapper.Map<FoldersAndFilesCutAndPasteResponse>(folderDomain);

            return response;
        }

        [HttpDelete("delete/foldersandfiles")]
        public async Task Delete([FromBody] FoldersAndFilesDeleteRequest foldersAndFilesDeleteDto)
        {
            await _folderAndFileManagementService.DeleteFoldersAndFilesAsync(
                foldersAndFilesDeleteDto.FilesIds,
                foldersAndFilesDeleteDto.FoldersIds
                );
        }
    }
}
