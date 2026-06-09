using BattleShipGame_Frontend.Models;
using BattleShipGame_Frontend.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BattleShipGame_Frontend
{
    public partial class SessionsWindow : Form
    {
        private readonly TokenService _tokenService;
        private List<Session> _games;

        public SessionsWindow(List<Session> games, TokenService tokenService)
        {
            InitializeComponent();
            _games = games;
            _tokenService = tokenService;
        }
        
    }
}
