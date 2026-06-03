using System.Net;
using BattleShipGame_Frontend.Configuration;
using BattleShipGame_Frontend.Models;
using BattleShipGame_Frontend.Services;

namespace BattleShipGame_Frontend.Windows
{
    public partial class MenuWindow : Form
    {
        private readonly TokenService _tokenService;

        public MenuWindow(User user, TokenService tokenService)
        {
            _tokenService = tokenService;
            InitializeComponent();
            WelcomeLabel.Text = $"Welcome {user.UserName}!";
            WinsLabel.Text = $"Wins: {user.Wins}";
            LossesLabel.Text = $"Losses: {user.Losses}";
        }

        private void JoinGameButton_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            button.Enabled = false;
            ListSessions(ConnectionClient.sharedClient);
        }

        private async void ListSessions(HttpClient httpClient)
        {
            using HttpResponseMessage response = await httpClient.GetAsync("session");
            MessageBox.Show(await response.Content.ReadAsStringAsync());
        }

        private void CreateGameButton_Click(object sender, EventArgs e) { }

        private void LogOutButton_Click(object sender, EventArgs e)
        {
            LoginWindow loginWindow = new(_tokenService);
            ConnectionClient.sharedClient.DefaultRequestHeaders.Authorization = null;
            loginWindow.Show();
            this.Close();
        }

        private async void DeleteAccountButton_Click(object sender, EventArgs e)
        {
            if (await DeleteAccount(ConnectionClient.sharedClient))
            {
                LoginWindow loginWindow = new(_tokenService);
                ConnectionClient.sharedClient.DefaultRequestHeaders.Authorization = null;
                loginWindow.Show();
                this.Close();
            }
        }

        private async Task<bool> DeleteAccount(HttpClient httpClient)
        {
            using HttpResponseMessage response = await httpClient.DeleteAsync("user");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }
    }
}
