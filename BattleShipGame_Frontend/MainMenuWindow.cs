using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using BattleShipGame_Frontend.Configuration;
using BattleShipGame_Frontend.Models;
using BattleShipGame_Frontend.Services;
using Microsoft.VisualBasic.ApplicationServices;

namespace BattleShipGame_Frontend
{
    public partial class MainMenuWindow : Form
    {
        private readonly TokenService _tokenService;

        public MainMenuWindow(TokenService tokenService)
        {
            _tokenService = tokenService;
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, EventArgs e) { }

        private async void LoginButton_Click(object sender, EventArgs e)
        {
            var x = await LoginAsync(ConnectionClient.sharedClient);
            if (!string.IsNullOrEmpty(x))
            {
                StatusLabel.Text = "User found!";
                _tokenService.SetToken(x);
                ConnectionClient.sharedClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _tokenService.GetToken());
                var user = await GetUser(ConnectionClient.sharedClient);

                await Task.Delay(2000);
                MessageBox.Show(
                    $"Id:{user.Id}, Username:{user.UserName}, Password: {user.Password}, Wins: {user.Wins}, Losses: {user.Losses} "
                );
            }
        }

        private async Task<string?> LoginAsync(HttpClient httpClient)
        {
            var user = new Models.User()
            {
                Id = "0",
                UserName = UsernameTextBox.Text,
                Password = PasswordTextBox.Text,
                Wins = 0,
                Losses = 0,
            };
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(user),
                Encoding.UTF8,
                "application/json"
            );
            StatusLabel.Text = "Connecting...";

            using HttpResponseMessage response = await httpClient.PostAsync(
                "user/login",
                jsonContent
            );

            var responseOutput = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return responseOutput;
            }
            else
            {
                StatusLabel.Text = responseOutput;
                return null;
            }
        }

        private async Task<Models.User> GetUser(HttpClient httpClient)
        {
            using HttpResponseMessage response = await httpClient.GetAsync("user/current");
            return await response.Content.ReadFromJsonAsync<Models.User>();
        }

        private void ForgotLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) { }
    }
}
