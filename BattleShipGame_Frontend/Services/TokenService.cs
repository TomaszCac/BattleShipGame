using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShipGame_Frontend.Services
{
    public class TokenService
    {
        private string? _token;

        public void SetToken(string token)
        {
            _token = token;
        }
        public string? GetToken()
        {
            return _token;
        }
    }
}
