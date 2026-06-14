using BattleShipGame_Frontend.Configuration;
using BattleShipGame_Frontend.Models;
using BattleShipGame_Frontend.Services;
using BattleShipGame_Frontend.Windows;
using Newtonsoft.Json;

namespace BattleShipGame_Frontend
{
    public partial class SessionsWindow : Form
    {
        private readonly TokenService _tokenService;
        private List<Session> _sessions;
        private readonly User _currentUser;

        public SessionsWindow(List<Session> sessions, TokenService tokenService, User currentUser)
        {
            InitializeComponent();
            _sessions = sessions;
            _tokenService = tokenService;
            _currentUser = currentUser;
            InsertSessions();
        }
        /// <summary>
        /// Display sessions in list
        /// </summary>
        private void InsertSessions()
        {
            if (_sessions.Count != 0)
            {
                SessionsLayout.Controls.Clear();
                foreach (var x in _sessions)
                {
                    Panel card = new Panel();
                    card.BorderStyle = BorderStyle.FixedSingle;
                    card.Width = 1300;
                    card.Height = 50;
                    Label idlabel = new Label();
                    Label nameLabel = new Label();
                    Label winsLabel = new Label();
                    Label lossesLabel = new Label();
                    idlabel.Location = new Point(10, 15);
                    nameLabel.Location = new Point(130, 15);
                    nameLabel.Width = 150;
                    winsLabel.Location = new Point(350, 15);
                    lossesLabel.Location = new Point(450, 15);
                    idlabel.Text = $"ID: {x.Id.ToString()}";
                    nameLabel.Text = $"Host name: {x.Host.UserName}";
                    winsLabel.Text = $"Wins: {x.Host.Wins.ToString()}";
                    lossesLabel.Text = $"Losses: {x.Host.Losses.ToString()}";
                    Button button = new Button();
                    button.Name = $"{x.Id.ToString()}";
                    button.Location = new Point(1220, 10);
                    button.Text = "Join";
                    button.Click += JoinGameButton_Click;
                    card.Controls.Add(idlabel);
                    card.Controls.Add(nameLabel);
                    card.Controls.Add(winsLabel);
                    card.Controls.Add(lossesLabel);
                    card.Controls.Add(button);
                    SessionsLayout.Controls.Add(card);
                }
            }
            else
            {
                StatusLabel.Text = "There are no current sessions available.";
            }
        }
        private async void JoinGameButton_Click(Object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            using HttpResponseMessage response = await ConnectionClient.sharedClient.GetAsync(
                $"session/join/{btn.Name}"
            );
            var stringContent = await response.Content.ReadAsStringAsync();
            Session session = JsonConvert.DeserializeObject<Session>(stringContent);

            BattleWindow battleWindow = new(session, false, _currentUser, _tokenService);
            battleWindow.StartPosition = FormStartPosition.Manual;
            battleWindow.Location = new Point(this.Location.X, this.Location.Y);
            battleWindow.Show();
            this.Close();
        }
        private async void BackButton_Click(Object sender, EventArgs e)
        {
            MenuWindow menuWindow = new(_currentUser, _tokenService);
            menuWindow.StartPosition = FormStartPosition.Manual;
            menuWindow.Location = new Point(this.Location.X, this.Location.Y);
            menuWindow.Show();
            this.Close();
        }
        private async void RefreshButton_Click(Object sender, EventArgs e)
        {
            using HttpResponseMessage response = await ConnectionClient.sharedClient.GetAsync(
               "session"
           );
            var stringContent = await response.Content.ReadAsStringAsync();
            _sessions = JsonConvert.DeserializeObject<List<Session>>(stringContent);
            InsertSessions();
        }

        private void SessionsWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            ApplicationLifeTimeService.ShutdownApplication();

        }
    }
}
