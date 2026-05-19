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
            CreateMap<UserDto, User>().ConvertUsing<UserConverter>();
        }
    }

    public class UserConverter : ITypeConverter<UserDto, User>
    {
        public User Convert(UserDto source, User destination, ResolutionContext context)
        {
            var user = new User();
            var passwordHasher = new PasswordHasher<User>();
            user.UserName = source.UserName;
            user.Wins = 0;
            user.Losses = 0;
            user.PasswordHash = passwordHasher.HashPassword(user, source.Password);

            return user;
        }
    }
}
