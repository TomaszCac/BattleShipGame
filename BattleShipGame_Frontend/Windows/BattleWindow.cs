using BattleShipGame_Frontend.Configuration;
using BattleShipGame_Frontend.Models;
using BattleShipGame_Frontend.Services;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System.Net.Http.Json;
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
        private bool _playerConnected = false;
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
        /// <summary>
        /// Establishes all needed handlers
        /// </summary>
        private void EstablishConnection()
        {
            _hubConnection = new HubConnectionBuilder().WithUrl("http://localhost:5000/hubs/session").Build();
            _hubConnection.Closed += HubConnection_Closed;
            _hubConnection.On<string>("PlayerConnected",(note) => { statusLabel.Invoke(PlayerConnected, note); } );
            _hubConnection.On("StartFight", () => { statusLabel.Invoke(StartFight); });
            _hubConnection.On<int, int, bool>("ShipHit", (x, y, isHit) => { this.Invoke(ShipHit, x, y, isHit); });
            _hubConnection.On<bool>("WinGame", (turn) => { this.Invoke(WinGame, turn); });
            _hubConnection.On<string>("PlayerDisconnected", (note) => {PlayerDisconnected(note); });

        }
        private void PlayerDisconnected(string note)
        {
            this.Invoke(() =>
            {
                this.Controls.Clear();
                _gameRunning = false;
                statusLabel = new Label();
                statusLabel.Location = new Point(550, 200);
                statusLabel.Width = 300;
                statusLabel.Height = 100;
                statusLabel.Text = note;
                this.Controls.Add(statusLabel);
                this.Controls.Add(quitButton);
            });
            
        }
        /// <summary>
        /// Starts hub connection
        /// </summary>
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
            _playerConnected = true;
            await Task.Delay(2000);
            if(_gameRunning)
            {
                this.Controls.Clear();
                SetupBoard();
            }
        }
        /// <summary>
        /// Sets initial message to display
        /// </summary>
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
            quitButton = new Button();
            quitButton.Text = "Quit";
            quitButton.Name = "QuitButton";
            quitButton.Width = 100;
            quitButton.Height = 50;
            quitButton.Location = new Point(12, 735);
            quitButton.Click += QuitButton_Click;
            this.Controls.Add(quitButton);

        }
        /// <summary>
        /// Organizes and displays tiles for user
        /// </summary>
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
        /// <summary>
        /// Organizes and displays enemy board buttons
        /// </summary>
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
        private async void QuitButton_Click(object sender, EventArgs e)
        {
            await QuitBattle(ConnectionClient.sharedClient);
            MenuWindow menuWindow = new MenuWindow(_currentUser, _tokenService);
            menuWindow.StartPosition = FormStartPosition.Manual;
            menuWindow.Location = new Point(this.Location.X, this.Location.Y);
            menuWindow.Show();
            this.Close();
        }
        /// <summary>
        /// Stops connection to hub and deletes current session
        /// </summary>
        /// <param name="httpClient">HttpClient for connection</param>
        /// <returns></returns>
        private async Task QuitBattle(HttpClient httpClient)
        {
            if (_gameRunning)
            {
                _gameRunning = false;
                if (_playerConnected)
                {
                    await _hubConnection.InvokeAsync("Disconnected", _session.Id.ToString(), _currentUser.UserName);
                    _currentUser.Losses++;

                }
                await _hubConnection.StopAsync();
                await httpClient.DeleteAsync(
                    $"session/end?sessionId={_session.Id}&host={_isHost}"
                );
                
            }

        }
        private void ReadyButton_Click(object sender, EventArgs e)
        {
            restartBoardButton.Enabled = false;
            changeRotationButton.Enabled = false;
            readyButton.Enabled = false;
            SendBoard(ConnectionClient.sharedClient);
        }
        /// <summary>
        /// Sends current set board to server
        /// </summary>
        /// <param name="httpClient">HttpClient for connection</param>
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
        /// <summary>
        /// Displays message about both users ready and starts display of enemy board
        /// </summary>
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
        /// <summary>
        /// Sends to server coordinates of selected tile to shoot
        /// </summary>
        /// <param name="httpClient">HttpClient for connection</param>
        /// <param name="x">Coordinate x in (x,y) board</param>
        /// <param name="y">Coordinate y in (x,y) board</param>
        /// <returns>Bool value if ship has been hit</returns>
        private async Task<bool> ShootEnemy(HttpClient httpClient, int x, int y)
        {
            var response = await httpClient.PostAsync(
                    $"session/shoot?x={x}&y={y}&sessionId={_session.Id}&turn={_isHost}",
                    null
                );
            return await response.Content.ReadFromJsonAsync<bool>();
        }
        /// <summary>
        /// Displays message if ship has been hit
        /// </summary>
        /// <param name="x">Coordinate x in (x,y) board</param>
        /// <param name="y">Coordinate y in (x,y) board</param>
        /// <param name="isHit">Bool value if ship has been hit</param>
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
        /// <summary>
        /// Cycles turn between host and guest
        /// </summary>
        private void CycleTurn()
        {
            _turn = !_turn;
        }
        /// <summary>
        /// Displays message and increments local value win/lose of user
        /// </summary>
        /// <param name="hostWon">Bool value if user won</param>
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
                _currentUser.Losses++;
            }
            _gameRunning = false;
            this.Controls.Add(quitButton);
            this.Controls.Add(statusLabel);
        }
        /// <summary>
        /// Displays message about current ship to place
        /// </summary>
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
        /// <summary>
        /// Checks if on current row/column ship can be placed
        /// </summary>
        /// <param name="x">Initial coordinate x in (x,y) board</param>
        /// <param name="y">Initial coordinate y in (x,y) board</param>
        /// <param name="rotation">Bool value true for column and false for row</param>
        /// <param name="currentShip">Int value of ship size</param>
        /// <param name="direction">Bool value true for decreasing coordinate false for increasing coordinate</param>
        /// <returns>Bool value if ship can be placed</returns>
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
        /// <summary>
        /// Displays color of tiles
        /// </summary>
        /// <param name="x">Initial coordinate x in (x,y) board</param>
        /// <param name="y">Initial coordinate y in (x,y) board</param>
        /// <param name="rotation">Bool value true for column and false for row</param>
        /// <param name="currentship">Int value of ship size</param>
        /// <param name="direction">Bool value true for decreasing coordinate false for increasing coordinate</param>
        /// <param name="color">Color in which tiles will be drawn</param>
        /// <param name="isClicked">Bool value if tile has been clicked</param>
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
        /// <summary>
        /// Modifies tile
        /// </summary>
        /// <param name="x">Initial coordinate x in (x,y) board</param>
        /// <param name="y">Initial coordinate y in (x,y) board</param>
        /// <param name="currentShipSize">Int value of ship size</param>
        /// <param name="color">Color in which tiles will be drawn</param>
        /// <param name="isClicked">Bool value if tile has been clicked</param>
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
        /// <summary>
        /// Changes current ship size
        /// </summary>
        /// <param name="isClicked">Bool value if tile has been clicked</param>
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
        private async void BattleWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(_gameRunning)
            {
                await QuitBattle(ConnectionClient.sharedClient);
            }
            ApplicationLifeTimeService.ShutdownApplication();
        }
    }
}
