using AutoMapper;
using BattleShipGame.Dtos;
using BattleShipGame.Models;

namespace BattleShipGame.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
        }
    }
}
