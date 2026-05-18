namespace BattleShipGame.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Password { get; set; }
        public int? Wins { get; set; }
        public int? Losses { get; set; }
    }
}
