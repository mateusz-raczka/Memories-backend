﻿using AutoMapper;
using Memories_backend.Models.Domain.Folder.File;
using Memories_backend.Models.DTO.Category.Response;

namespace Memories_backend.AutoMapper.Profiles
{
    public class CategoryMapperProfiles : Profile
    {
        public CategoryMapperProfiles() 
        {
            CreateMap<Category, CategoryDtoResponse>().ReverseMap();
        }
    }
}
