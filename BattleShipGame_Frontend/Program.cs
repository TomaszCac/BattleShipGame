using BattleShipGame_Frontend.Configuration;
using BattleShipGame_Frontend.Services;

namespace BattleShipGame_Frontend
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            var tokenService = new TokenService();
            ApplicationConfiguration.Initialize();
            Application.Run(new MainMenuWindow(tokenService));
        }
    }
}
