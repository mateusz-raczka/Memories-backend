﻿using Memories_backend.Models.DTO.Tag.Request;
using Memories_backend.Models.DTO.Tag.Response;
using AutoMapper;
using Memories_backend.Models.Domain;

namespace Memories_backend.AutoMapper.Profiles
{
    public class TagMapperProfiles : Profile
    {
        public TagMapperProfiles() 
        {
            CreateMap<Tag, TagDtoResponse>().ReverseMap();
            CreateMap<Tag, TagDtoRequest>().ReverseMap();
        }
    }
}
