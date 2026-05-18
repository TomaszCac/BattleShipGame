namespace BattleShipGame.Models
{
    public class Session
    {
        public int Id { get; set; }
        public int[,]? BoardHost { get; set; }
        public int[,]? BoardGuest { get; set; }
        public User Host { get; set; }
        public User? Guest { get; set; }
    }
}
