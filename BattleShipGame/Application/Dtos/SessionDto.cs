using BattleShipGame.Domain.Entities;

namespace BattleShipGame.Application.Dtos
{
    public class SessionDto
    {
        public int Id { get; set; }
        public int[,]? BoardHost { get; set; }
        public int[,]? BoardGuest { get; set; }
        public UserDto Host { get; set; }
        public UserDto? Guest { get; set; }
    }
}
