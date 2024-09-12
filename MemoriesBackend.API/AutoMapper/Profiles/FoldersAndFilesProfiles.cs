using AutoMapper;
using MemoriesBackend.API.DTO.FolderAndFile.Response;
using MemoriesBackend.Domain.Models.FolderAndFileManagement;

namespace MemoriesBackend.API.AutoMapper.Profiles
{
    public class FoldersAndFilesProfiles : Profile
    {
        public FoldersAndFilesProfiles() 
        {
            CreateMap<CopyAndPasteFoldersAndFilesResult, FolderAndFileCopyAndPasteResponse>().ReverseMap();
        }
    }
}
