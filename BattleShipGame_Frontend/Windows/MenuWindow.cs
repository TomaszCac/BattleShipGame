using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using BattleShipGame_Frontend.Configuration;
using BattleShipGame_Frontend.Models;
using BattleShipGame_Frontend.Services;
using Newtonsoft.Json;

namespace BattleShipGame_Frontend.Windows
{
    public partial class MenuWindow : Form
    {
        private readonly TokenService _tokenService;
        private readonly User _currentUser;

        public MenuWindow(User user, TokenService tokenService)
        {
            _tokenService = tokenService;
            _currentUser = user;
            InitializeComponent();
            WelcomeLabel.Text = $"Welcome {_currentUser.UserName}!";
            WinsLabel.Text = $"Wins: {_currentUser.Wins}";
            LossesLabel.Text = $"Losses: {_currentUser.Losses}";
        }

        private async void JoinGameButton_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            button.Enabled = false;
            var sessions = await ListSessions(ConnectionClient.sharedClient);
            SessionsWindow sessionsWindow = new(sessions, _tokenService, _currentUser);
            sessionsWindow.StartPosition = FormStartPosition.Manual;
            sessionsWindow.Location = new Point(this.Location.X, this.Location.Y);
            sessionsWindow.Show();
            this.Close();

        }

        /// <summary>
        /// Returns current available sessions from server
        /// </summary>
        /// <param name="httpClient">HttpClient for connection</param>
        /// <returns>List of session classes</returns>
        private async Task<List<Session>> ListSessions(HttpClient httpClient)
        {
            using HttpResponseMessage response = await httpClient.GetAsync("session");
            return JsonConvert.DeserializeObject<List<Session>>(await response.Content.ReadAsStringAsync());
        }

        private async void CreateGameButton_Click(object sender, EventArgs e)
        {
            var session = await CreateSession(ConnectionClient.sharedClient);
            BattleWindow battleWindow = new(session, true, _currentUser, _tokenService);
            battleWindow.StartPosition = FormStartPosition.Manual;
            battleWindow.Location = new Point(this.Location.X, this.Location.Y);
            battleWindow.Show();
            this.Close();

        }
        /// <summary>
        /// Sends session creation request to server and returns session or null
        /// </summary>
        /// <param name="httpClient">HttpClient for connection</param>
        /// <returns>Session class or null if something went wrong on server</returns>
        private async Task<Session> CreateSession(HttpClient httpClient)
        {
            using HttpResponseMessage response = await httpClient.PostAsync("session", null);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return await response.Content.ReadFromJsonAsync<Session>();
            }
            return null;
        }

        private void LogOutButton_Click(object sender, EventArgs e)
        {
            LoginWindow loginWindow = new(_tokenService);
            ConnectionClient.sharedClient.DefaultRequestHeaders.Authorization = null;
            loginWindow.StartPosition = FormStartPosition.Manual;
            loginWindow.Location = new Point(this.Location.X, this.Location.Y);
            loginWindow.Show();
            this.Close();
        }

        private async void DeleteAccountButton_Click(object sender, EventArgs e)
        {
            DialogResult selectBox = MessageBox.Show("Are you sure you want to delete your account?","Warning", MessageBoxButtons.YesNo,MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (DialogResult.Yes == selectBox)
            {
                if (await DeleteAccount(ConnectionClient.sharedClient))
                {
                    StatusLabel.Text = "Connecting...";
                    StatusLabel.Text = "User deleted successfully";
                    LoginWindow loginWindow = new(_tokenService);
                    ConnectionClient.sharedClient.DefaultRequestHeaders.Authorization = null;
                    await Task.Delay(2000);
                    loginWindow.StartPosition = FormStartPosition.Manual;
                    loginWindow.Location = new Point(this.Location.X, this.Location.Y);
                    loginWindow.Show();
                    this.Close();
                }
            }
        }

        /// <summary>
        /// Sends account deletion request
        /// </summary>
        /// <param name="httpClient">HttpClient for connection</param>
        /// <returns></returns>
        private async Task<bool> DeleteAccount(HttpClient httpClient)
        {
            using HttpResponseMessage response = await httpClient.DeleteAsync("user");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

        private void MenuWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            ApplicationLifeTimeService.ShutdownApplication();

        }
    }
}
