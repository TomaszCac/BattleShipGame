namespace BattleShipGame.Application.Interfaces
{
    public interface IGameNotificationService
    {
        public Task StartFight(int sessionId);

    }
}
