namespace BattleShipGame.Application.Interfaces
{
    public interface IGameNotificationService
    {
        public Task ShipHit(int x, int y, int sessionId, bool isHit);
        public Task WinGame(int sessionId, bool turn);
        public Task StartFight(int sessionId);

    }
}
