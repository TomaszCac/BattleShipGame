using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using BattleShipGame_Frontend.Configuration;
using BattleShipGame_Frontend.Models;
using BattleShipGame_Frontend.Services;
using BattleShipGame_Frontend.Windows;

namespace BattleShipGame_Frontend
{
    public partial class LoginWindow : Form
    {
        private readonly TokenService _tokenService;

        public LoginWindow(TokenService tokenService)
        {
            _tokenService = tokenService;
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            button.Enabled = false;
            RegisterUser(ConnectionClient.sharedClient);
        }

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
                MenuWindow menuWindow = new(user, _tokenService);
                menuWindow.StartPosition = FormStartPosition.Manual;
                menuWindow.Location = new Point(this.Location.X, this.Location.Y);
                menuWindow.Show();
                this.Close();
            }
        }

        /// <summary>
        /// Sends login credentials to server and returns JWT token or null
        /// </summary>
        /// <param name="httpClient">HttpClient for connection</param>
        /// <returns>JWT token as string or null if wrong credentials</returns>
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

        /// <summary>
        /// Sends register credentials to server
        /// </summary>
        /// <param name="httpClient">HttpClient for connection</param>
        private async void RegisterUser(HttpClient httpClient)
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
                "user/register",
                jsonContent
            );
            if (response.StatusCode == HttpStatusCode.OK)
            {
                StatusLabel.Text = await response.Content.ReadAsStringAsync();
            }
            else if (response.StatusCode == HttpStatusCode.Conflict)
            {
                StatusLabel.Text = await response.Content.ReadAsStringAsync();
            }
            else
            {
                List<Result>? responseResultOutput = await response.Content.ReadFromJsonAsync<
                    List<Result>
                >();
                var errors = responseResultOutput.Select(b => b.Description);
                StatusLabel.Text = "Registration failed";
                foreach (var error in errors)
                {
                    StatusLabel.Text += $"\n {error}";
                }
                RegisterButton.Enabled = true;
            }
        }

        /// <summary>
        /// Returns user class from server based on current token set
        /// </summary>
        /// <param name="httpClient">HttpClient for connection</param>
        /// <returns>User class from response</returns>
        private async Task<Models.User> GetUser(HttpClient httpClient)
        {
            using HttpResponseMessage response = await httpClient.GetAsync("user/current");
            return await response.Content.ReadFromJsonAsync<Models.User>();
        }

        private void ForgotLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) { }

        private void LoginWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            ApplicationLifeTimeService.ShutdownApplication();

        }
    }
}
