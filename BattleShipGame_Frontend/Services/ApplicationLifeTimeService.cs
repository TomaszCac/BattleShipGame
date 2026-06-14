using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShipGame_Frontend.Services
{
    internal class ApplicationLifeTimeService
    {
        /// <summary>
        /// Closes application if all windows are closed
        /// </summary>
        public static void ShutdownApplication()
        {
            if (Application.OpenForms.Count == 0)
            {
                Application.Exit();
            }
        }
    }
}
