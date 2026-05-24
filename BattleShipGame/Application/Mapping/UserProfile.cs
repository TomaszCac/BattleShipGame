using AutoMapper;
using BattleShipGame.Application.Dtos;
using BattleShipGame.Domain.Entities;

namespace BattleShipGame.Application.Mapping
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
