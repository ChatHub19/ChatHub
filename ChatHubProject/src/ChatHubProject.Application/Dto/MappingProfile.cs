using AutoMapper;
using ChatHubProject.Application.Model;

namespace ChatHubProject.Application.Dto
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, AllUserDto>();
            CreateMap<AllUserDto, User>();
        }
    }
}