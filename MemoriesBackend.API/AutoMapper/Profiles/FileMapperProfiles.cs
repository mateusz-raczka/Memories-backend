﻿using AutoMapper;
using MemoriesBackend.API.DTO.File.Request;
using MemoriesBackend.API.DTO.File.Response;
using File = MemoriesBackend.Domain.Entities.File;

namespace MemoriesBackend.API.AutoMapper.Profiles
{
    public class FileMapperProfiles : Profile
    {
        public FileMapperProfiles()
        {
            CreateMap<File, FileGetAllResponse>().ReverseMap();
            CreateMap<File, FileUpdateRequest>().ReverseMap();
            CreateMap<File, FileAddResponse>().ReverseMap();
            CreateMap<File, FileGetByIdResponse>().ReverseMap();
            CreateMap<File, FileCopyPasteResponse>().ReverseMap();
        }
    }
}
