﻿using AutoMapper;
using MemoriesBackend.API.DTO.Folder.Request;
using MemoriesBackend.API.DTO.Folder.Response;
using MemoriesBackend.Domain.Entities;

namespace MemoriesBackend.API.AutoMapper.Profiles
{
    public class FolderMapperProfiles : Profile
    {
        public FolderMapperProfiles()
        {
            CreateMap<Folder, FolderAddResponse>().ReverseMap();
            CreateMap<Folder, FolderAddRequest>().ReverseMap();
            CreateMap<Folder, FolderGetAllResponse>().ReverseMap();
            CreateMap<Folder, FolderGetByIdResponse>().ReverseMap();
            CreateMap<Folder, FolderCopyAndPasteResponse>().ReverseMap();
        }
    }
}
