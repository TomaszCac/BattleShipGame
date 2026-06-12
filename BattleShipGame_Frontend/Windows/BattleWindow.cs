using BattleShipGame_Frontend.Configuration;
using BattleShipGame_Frontend.Models;
using BattleShipGame_Frontend.Services;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System.Net.Http.Json;
using static System.Collections.Specialized.BitVector32;
namespace BattleShipGame_Frontend.Windows
{
    public partial class BattleWindow : Form
    {
        private HubConnection _hubConnection;
        private readonly TokenService _tokenService;
        private readonly bool _isHost;
        private bool _turn;
        private User _currentUser;
        private bool _gameRunning = true;
        private Session _session;
        public Button changeRotationButton;
        public Button restartBoardButton;
        public Button quitButton;
        private Button readyButton;
        private Label statusLabel;
        private Label idLabel;
        public Label[,] boardVisual = new Label[10, 10];
        public int[,] board = new int[10, 10];
        public char[] alphabet = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J'];
        public int[] shipSizes = [2, 3, 3, 4, 5];
        public int currentShipIndex = 4;
        public bool rotation = true;
        public Color[] shipColors = [Color.Blue, Color.Red, Color.Yellow, Color.Violet, Color.Pink];


        public BattleWindow(Session session, bool isHost, User currentUser, TokenService tokenService)
        {
            _tokenService = tokenService;
            _isHost = isHost;
            _currentUser = currentUser;
            _session = session;
            _turn = _isHost;
            InitializeComponent();
            EstablishConnection();
            StartConnection();
            ShowInitialMessage();
            
        }
        private void EstablishConnection()
        {
            _hubConnection = new HubConnectionBuilder().WithUrl("http://localhost:5000/hubs/session").Build();
            _hubConnection.Closed += HubConnection_Closed;
            _hubConnection.On<string>("PlayerConnected",(note) => { statusLabel.Invoke(PlayerConnected, note); } );
            _hubConnection.On("StartFight", () => { statusLabel.Invoke(StartFight); });
            _hubConnection.On<int, int, bool>("ShipHit", (x, y, isHit) => { this.Invoke(ShipHit, x, y, isHit); });
            _hubConnection.On<bool>("WinGame", (turn) => { this.Invoke(WinGame, turn); });

        }
        public async void StartConnection()
        {
            await _hubConnection.StartAsync();
            if (!_isHost)
                await _hubConnection.InvokeAsync(
                    "PlayerConnected",
                    _session.Id.ToString(),
                    _currentUser.UserName
                );
            else
                await _hubConnection.InvokeAsync("HostGame", _session.Id.ToString());
        }
        private async Task HubConnection_Closed(Exception? arg)
        {
            await Task.Delay(new Random().Next(0, 5) * 100);
            await _hubConnection.StartAsync();
        }
        private async Task PlayerConnected(string note) {
            statusLabel.Text = note;
            await Task.Delay(2000);
            this.Controls.Clear();
            SetupBoard();
        }
        private void ShowInitialMessage()
        {
            statusLabel = new Label();
            statusLabel.Location = new Point(550, 200);
            statusLabel.Width = 300;
            statusLabel.Height = 100;
            statusLabel.Text = "Waiting for opponent";
            this.Controls.Add(statusLabel);
            idLabel = new Label();
            idLabel.Height = 15;
            idLabel.Name = "IdLabel";
            idLabel.Text = $"Session ID: {_session.Id}";
            idLabel.Location = new Point(674, 752);
        }
        private void SetupBoard()
        {
            for (int x = 0; x < 11; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    if (x == 0)
                    {
                        Label pbCordsY = new Label();
                        pbCordsY.Width = 50;
                        pbCordsY.Height = 50;
                        pbCordsY.Name = $"{alphabet[y]}";
                        pbCordsY.Text = $"{alphabet[y]}";
                        pbCordsY.Location = new Point(0, y * 50 + 50);
                        pbCordsY.TextAlign = ContentAlignment.MiddleCenter;
                        Label pbCordsX = new Label();
                        pbCordsX.Width = 50;
                        pbCordsX.Height = 50;
                        pbCordsX.Name = $"{y}";
                        pbCordsX.Text = $"{y}";
                        pbCordsX.Location = new Point(y * 50 + 50, 0);
                        pbCordsX.TextAlign = ContentAlignment.MiddleCenter;
                        this.Controls.Add(pbCordsY);
                        this.Controls.Add(pbCordsX);
                    } else
                    {
                        Label pb = new Label();
                        pb.Width = 50;
                        pb.Height = 50;
                        pb.BorderStyle = BorderStyle.FixedSingle;
                        pb.BackColor = Color.White;
                        pb.Name = $"{x-1},{y}";
                        pb.Text = $"{alphabet[x-1]} {y}";
                        pb.Location = new Point(y * 50 + 50, (x - 1) * 50 + 50);
                        pb.MouseEnter += Label_MouseEnter;
                        pb.MouseLeave += Label_MouseLeave;
                        pb.Click += Label_Click;
                        pb.TextAlign = ContentAlignment.MiddleCenter;
                        boardVisual[x-1, y] = pb;
                        board[x-1, y] = 0;
                        this.Controls.Add(pb);
                    }
                    
                }
            }
            changeRotationButton = new Button();
            changeRotationButton.Location = new Point(625, 50);
            changeRotationButton.Text = "Rotate";
            changeRotationButton.Width = 100;
            changeRotationButton.Height = 50;
            changeRotationButton.Name = "ChangeRotationButton";
            changeRotationButton.Click += ChangeRotationButton_Click;
            restartBoardButton = new Button();
            restartBoardButton.Location = new Point(625, 100);
            restartBoardButton.Text = "Restart Board";
            restartBoardButton.Width = 100;
            restartBoardButton.Height = 50;
            restartBoardButton.Name = "RestartBoardButton";
            restartBoardButton.Click += RestartBoardButton_Click;
            readyButton = new Button();
            readyButton.Text = "Ready";
            readyButton.Enabled = false;
            readyButton.Location = new Point(625, 150);
            readyButton.Width = 100;
            readyButton.Height = 50;
            readyButton.Name = "ReadyButton";
            readyButton.Click += ReadyButton_Click;
            statusLabel = new Label();
            statusLabel.Location = new Point(575, 300);
            statusLabel.Width = 200;
            statusLabel.Height = 100;
            quitButton = new Button();
            quitButton.Text = "Quit";
            quitButton.Name = "QuitButton";
            quitButton.Width = 100;
            quitButton.Height = 50;
            quitButton.Location = new Point(12, 735);
            quitButton.Click += QuitButton_Click;
            idLabel = new Label();
            idLabel.Height = 15;
            idLabel.Name = "IdLabel";
            idLabel.Text = $"Session ID: {_session.Id}";
            idLabel.Location = new Point(674, 752);
            this.Controls.Add(idLabel);
            this.Controls.Add(quitButton);
            this.Controls.Add(statusLabel);
            this.Controls.Add(readyButton);
            this.Controls.Add(restartBoardButton);
            this.Controls.Add(changeRotationButton);
        }
        public void SetupEnemyBoard()
        {
            for (int x = 0; x < 11; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    if (x == 0)
                    {
                        Label pbCordsY = new Label();
                        pbCordsY.Width = 50;
                        pbCordsY.Height = 50;
                        pbCordsY.Name = $"{alphabet[y]}";
                        pbCordsY.Text = $"{alphabet[y]}";
                        pbCordsY.Location = new Point(800, y * 50 + 50);
                        pbCordsY.TextAlign = ContentAlignment.MiddleCenter;
                        Label pbCordsX = new Label();
                        pbCordsX.Width = 50;
                        pbCordsX.Height = 50;
                        pbCordsX.Name = $"{y}";
                        pbCordsX.Text = $"{y}";
                        pbCordsX.Location = new Point(800 + y * 50 + 50, 0);
                        pbCordsX.TextAlign = ContentAlignment.MiddleCenter;

                        this.Controls.Add(pbCordsY);
                        this.Controls.Add(pbCordsX);
                    }
                    else
                    {
                        Button button = new Button();
                        button.Name = $"{x-1},{y}";
                        button.Text = $"{alphabet[x-1]} {y}";
                        button.Width = 50;
                        button.Height = 50;
                        button.Location = new Point(800 + y * 50 + 50, (x-1) * 50 + 50);
                        button.Click += ShootEnemyButton_Click;
                        this.Controls.Add(button);
                    }
                    
                }
            }
        }
        private void ChangeRotationButton_Click(object sender, EventArgs e)
        {
            rotation = !rotation;
        }
        private void RestartBoardButton_Click(object sender, EventArgs e)
        {
            currentShipIndex = 4;
            rotation = true;
            this.Controls.Clear();
            SetupBoard();
        }
        private void QuitButton_Click(object sender, EventArgs e)
        {
            MenuWindow menuWindow = new MenuWindow(_currentUser, _tokenService);
            menuWindow.Show();
            this.Close();
        }
        private void ReadyButton_Click(object sender, EventArgs e)
        {
            restartBoardButton.Enabled = false;
            changeRotationButton.Enabled = false;
            readyButton.Enabled = false;
            SendBoard(ConnectionClient.sharedClient);
        }
        private async void SendBoard(HttpClient httpClient)
        {
            var boardJson = JsonConvert.SerializeObject(board);
            using HttpResponseMessage response = await httpClient.PostAsJsonAsync($"session/place?sessionId={_session.Id}&host={_isHost}", boardJson);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                statusLabel.Text = "Your board is ready! Waiting for opponent";
            }
            else
            {
                statusLabel.Text = "Failure";
            }
        }
        private async void StartFight()
        {
            statusLabel.Text = "Both players are ready!";
            await Task.Delay(2000);
            SetupEnemyBoard();
        }
        private async void ShootEnemyButton_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            string[] stringArray = button.Name.Split(',');
            int coordinateX = Convert.ToInt32(stringArray[0]);
            int coordinateY = Convert.ToInt32(stringArray[1]);
            if (_turn)
            {
                var isHit = await ShootEnemy(ConnectionClient.sharedClient, coordinateX, coordinateY);
                if (isHit)
                {
                    button.BackColor = Color.Blue;
                }
                else
                {
                    button.BackColor = Color.Black;
                }
                button.ForeColor = Color.White;

                button.Enabled = false;
            }
        }
        private async Task<bool> ShootEnemy(HttpClient httpClient, int x, int y)
        {
            var response = await httpClient.PostAsync(
                    $"session/shoot?x={x}&y={y}&sessionId={_session.Id}&turn={_isHost}",
                    null
                );
            return await response.Content.ReadFromJsonAsync<bool>();
        }
        private void ShipHit(int x, int y, bool isHit)
        {
           if (_turn)
            {
                if(isHit)
                {
                    statusLabel.Text = "Ship has been hit!";
                }
                else
                {
                    statusLabel.Text = "You missed!";
                }
            } else
            {
                if (isHit)
                {
                    statusLabel.Text = "Your ship has been hit!";
                }
                else
                {
                    statusLabel.Text = "Enemy missed!";
                }
                boardVisual[x, y].BackColor = Color.Black;
                board[x, y] = -1;
            }
            CycleTurn();

        }
        private void CycleTurn()
        {
            _turn = !_turn;
        }
        private void WinGame(bool hostWon)
        {
            this.Controls.Clear();
            statusLabel = new Label();
            statusLabel.Location = new Point(550, 200);
            statusLabel.Width = 300;
            statusLabel.Height = 100;

            if (hostWon == _isHost)
            {
                statusLabel.Text = "You Won!";
                _currentUser.Wins++;
            }
            else
            {
                statusLabel.Text = "You Lost!";
                _currentUser.Losses--;
            }
            _gameRunning = false;
            this.Controls.Add(statusLabel);
        }
        public void ChangeShipPlacementLabel()
        {
            if (currentShipIndex >= 0)
            {
                statusLabel.Text = $"Place {shipSizes[currentShipIndex]} tile sized ship";
            }
            else
                statusLabel.Text = "Done! Now click Ready button or Reset your board! \n" +
                    "if you click Ready you can't make any changes then!";
        }
        public bool CheckIfAvailable(int x, int y, bool rotation, int currentShip, bool direction)
        {
            for(int length = 0; length < currentShip; length++)
            {
                if (rotation)
                {
                    if (board[direction ? x + length : x - length, y] != 0)
                    {
                        return false;
                    }
                }
                else
                {
                    if (board[x, direction ? y + length : y - length] != 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public void DrawTiles(int x, int y, bool rotation, int currentship, bool direction, Color color, bool isClicked)
        {
            for(int length = 0; length < currentship; length++)
            {
                if(rotation)
                {
                    boardVisual[direction ? x + length : x - length, y].BackColor = color;
                    if(isClicked)
                    {
                        board[direction ? x + length : x - length, y] = currentship;
                    }
                } else
                {
                    boardVisual[x, direction ? y + length : y - length].BackColor = color;
                    if(isClicked)
                    {
                        board[x, direction ? y + length : y - length] = currentship;
                    }
                }
            }
        }
        private void Label_MouseEnter(object? sender, EventArgs e) {
            if(currentShipIndex >= 0)
            {
                var label = (Label)sender;
                string[] stringArray = label.Name.Split(',');
                int positionX = Convert.ToInt32(stringArray[0]);
                int positionY = Convert.ToInt32(stringArray[1]);
                ModifyTile(positionX, positionY, shipSizes[currentShipIndex], Color.LightBlue, false);
            }
        }
        private void ModifyTile(int x, int y, int currentShipSize, Color color, bool isClicked)
        {
            if (rotation)
            {
                if (x <= 9 - currentShipSize)
                {
                    if (CheckIfAvailable(x, y, rotation, currentShipSize, true))
                    {
                        DrawTiles(x, y, rotation, currentShipSize, true, color, isClicked);
                        NextShip(isClicked);

                    }
                }
                else if (x >= 0 + currentShipSize)
                {
                    if (CheckIfAvailable(x, y, rotation, currentShipSize, false))
                    {
                        DrawTiles(x, y, rotation, currentShipSize, false, color, isClicked);
                        NextShip(isClicked);

                    }
                }
            }
            else
            {
                if (y <= 9 - currentShipSize)
                {
                    if (CheckIfAvailable(x, y, rotation, currentShipSize, true))
                    {
                        DrawTiles(x, y, rotation, currentShipSize, true, color, isClicked);
                        NextShip(isClicked);

                    }
                }
                else if (CheckIfAvailable(x, y, rotation, currentShipSize, false))
                {
                    DrawTiles(x, y, rotation, currentShipSize, false, color, isClicked);
                    NextShip(isClicked);
                }
            }
        }
        private void NextShip(bool isClicked)
        {
            if (isClicked)
            {
                currentShipIndex--;
                ChangeShipPlacementLabel();
                if (currentShipIndex < 0)
                {
                    readyButton.Enabled = true;
                }
            }

        }
        private void Label_MouseLeave(object sender, EventArgs e)
        {
            if (currentShipIndex >= 0)
            {
                var label = (Label)sender;
                string[] stringArray = label.Name.Split(',');
                int positionX = Convert.ToInt32(stringArray[0]);
                int positionY = Convert.ToInt32(stringArray[1]);
                ModifyTile(positionX, positionY, shipSizes[currentShipIndex], Color.White, false);
            }
        }
        private void Label_Click(object sender, EventArgs e)
        {
            if (currentShipIndex >= 0)
            {
                var label = (Label)sender;
                string[] stringArray = label.Name.Split(',');
                int positionX = Convert.ToInt32(stringArray[0]);
                int positionY = Convert.ToInt32(stringArray[1]);
                ModifyTile(positionX, positionY, shipSizes[currentShipIndex], shipColors[currentShipIndex], true);
            }
        }
    }
}
