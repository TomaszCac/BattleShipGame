using Microsoft.AspNetCore.Identity;

namespace BattleShipGame.Domain.Entities
{
    public class User : IdentityUser
    {
        public int? Wins { get; set; } = 0;
        public int? Losses { get; set; } = 0;
    }
}
