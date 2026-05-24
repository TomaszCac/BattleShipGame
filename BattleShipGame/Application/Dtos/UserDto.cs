namespace BattleShipGame.Application.Dtos
{
    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string? Password { get; set; }
        public int? Wins { get; set; }
        public int? Losses { get; set; }
    }
}
