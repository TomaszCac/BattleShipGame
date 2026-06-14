using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShipGame_Frontend.Services
{
    internal class ApplicationLifeTimeService
    {
        public static void ShutdownApplication()
        {
            if (Application.OpenForms.Count == 0)
            {
                Application.Exit();
            }
        }
    }
}
