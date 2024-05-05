using AutoMapper;
using Memories_backend.AutoMapper.Profiles;

namespace Memories_backend.AutoMapper
{
    public static class AutoMapperConfiguration
    {
        public static MapperConfiguration Configure()
        {
            var config = new MapperConfiguration(x =>
            {
                x.AddProfile<FileMapperProfiles>();
                x.AddProfile<AuthMapperProfiles>();
                x.AddProfile<TagMapperProfiles>();
                x.AddProfile<FileDetailsMapperProfiles>();
                x.AddProfile<CategoryMapperProfiles>();
                x.AddProfile<FolderMapperProfiles>();
            });

            return config;
        }
    }
}