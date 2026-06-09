using BattleShipGame_Frontend.Models;
using BattleShipGame_Frontend.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BattleShipGame_Frontend.Windows
{
    public partial class BattleWindow : Form
    {
        private readonly TokenService _tokenService;
        private readonly bool _isHost;
        private User _currentUser;
        private Session _session;
        public BattleWindow(Session session, bool isHost, User currentUser, TokenService tokenService)
        {
            _tokenService = tokenService;
            _isHost = isHost;
            _currentUser = currentUser;
            _session = session;
            InitializeComponent();
            IdLabel.Text = $"Session ID: {session.Id}";

        }
    }
}
