using AutoMapper;
using GrpcChatDemo.Models;

namespace GrpcChatDemo;

public class AutoMapperProfile: Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Persistance.ChatSession, ChatSessionDto>();
        CreateMap<Persistance.User, UserDto>();
        CreateMap<Persistance.Message, MessageDto>();
    }
}