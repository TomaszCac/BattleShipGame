using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShipGame_Frontend.Configuration
{
    internal class ConnectionClient
    {
        public static HttpClient sharedClient = new()
        {
            BaseAddress = new Uri("http://localhost:5000/api/"),
        };
    }
}
