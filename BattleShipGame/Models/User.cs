namespace BattleShipGame.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public int? Wins { get; set; }
        public int? Losses { get; set; }
    }
}
