using Memories_backend.Models.DTO.File.Response;
using Memories_backend.Models.DTO.FolderDetails.Response;

namespace Memories_backend.Models.DTO.Folder.Response
{
    public class FolderDtoFetchByIdResponse
    {
        public Guid Id { get; set; }
        public Guid? FolderId { get; set; }

        public FolderDetailsDtoResponse FolderDetails { get; set; }
        public IEnumerable<FileDtoFetchResponse> Files { get; set; }
        public IEnumerable<FolderDtoFetchAllResponse> ChildFolders { get; set; }
    }
}
