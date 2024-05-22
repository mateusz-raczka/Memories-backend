using AutoMapper;
using MemoriesBackend.API.AutoMapper.Profiles;

namespace MemoriesBackend.API.AutoMapper

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
                x.AddProfile<FolderDetailsMapperProfiles>();
                x.AddProfile<RegisterMapperProfiles>();
                x.AddProfile<LoginMapperProfiles>();
                x.AddProfile<TokenMapperProfiles>();
                x.AddProfile<UserMapperProfiles>();
            });

            return config;
        }
    }
}