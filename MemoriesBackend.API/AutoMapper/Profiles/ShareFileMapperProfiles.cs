using AutoMapper;
using MemoriesBackend.API.DTO.ShareFile.Request;
using MemoriesBackend.API.DTO.ShareFile.Response;
using MemoriesBackend.Domain.Entities;

namespace MemoriesBackend.API.AutoMapper.Profiles
{
    public class ShareFileMapperProfiles : Profile
    {
        public ShareFileMapperProfiles()
        {
            CreateMap<ShareFileAddRequest, ShareFile>().ReverseMap();
            CreateMap<ShareFile, ShareFileAddResponse>().ReverseMap();
        }
    }
}
