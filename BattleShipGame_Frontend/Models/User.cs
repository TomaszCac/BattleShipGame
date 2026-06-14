using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShipGame_Frontend.Models
{
    public class User
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string? Password { get; set; }
        public int? Wins { get; set; }
        public int? Losses { get; set; }
    }
}
