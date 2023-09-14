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
            CreateMap<MessageDto, Message>();
            CreateMap<Message, MessageDto>();
            CreateMap<FriendRequestDto, FriendRequest>();
            CreateMap<FriendRequest, FriendRequestDto>();
        }
    }
}