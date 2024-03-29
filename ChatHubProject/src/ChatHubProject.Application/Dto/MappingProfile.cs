using AutoMapper;
using ChatHubProject.Application.Model;

namespace ChatHubProject.Application.Dto
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}