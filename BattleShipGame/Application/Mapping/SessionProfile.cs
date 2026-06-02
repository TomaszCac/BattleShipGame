using AutoMapper;
using BattleShipGame.Application.Dtos;
using BattleShipGame.Domain.Entities;

namespace BattleShipGame.Application.Mapping
{
    public class SessionProfile : Profile
    {
        public SessionProfile()
        {
            CreateMap<Session, SessionDto>();
        }
    }
}
