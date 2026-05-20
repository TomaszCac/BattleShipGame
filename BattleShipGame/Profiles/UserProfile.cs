using AutoMapper;
using BattleShipGame.Dtos;
using BattleShipGame.Models;
using Microsoft.AspNetCore.Identity;

namespace BattleShipGame.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>().ForMember(dest => dest.Password, opt => opt.Ignore());
            CreateMap<UserDto, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
        }
    }
}
