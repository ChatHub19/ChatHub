using AutoMapper;
using ChatHubProject.Application.Model;

namespace ChatHubProject.Application.Dto
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>();

            CreateMap<ServerDto, Server>();
            CreateMap<Server, ServerDto>();
        }
    }
}