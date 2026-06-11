using BattleShipGame_Frontend.Models;
using BattleShipGame_Frontend.Services;

namespace BattleShipGame_Frontend.Windows
{
    public partial class BattleWindow : Form
    {
        private readonly TokenService _tokenService;
        private readonly bool _isHost;
        private User _currentUser;
        private Session _session;
        public Button changeRotationButton;
        public Button restartBoardButton;
        private Button readyButton;
        private Label statusLabel;
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
            InitializeComponent();
            IdLabel.Text = $"Session ID: {session.Id}";
            SetupBoard();
            ChangeShipPlacementLabel();
            SetupEnemyBoard();

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
            changeRotationButton.Click += ChangeRotationButton_Click;
            restartBoardButton = new Button();
            restartBoardButton.Location = new Point(625, 100);
            restartBoardButton.Text = "Restart Board";
            restartBoardButton.Width = 100;
            restartBoardButton.Height = 50;
            readyButton = new Button();
            readyButton.Text = "Ready";
            readyButton.Enabled = false;
            readyButton.Location = new Point(625, 150);
            readyButton.Width = 100;
            readyButton.Height = 50;
            statusLabel = new Label();
            statusLabel.Location = new Point(575, 300);
            statusLabel.Width = 200;
            statusLabel.Height = 100;
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
                        Button btn = new Button();
                        btn.Name = $"{x-1},{y}";
                        btn.Text = $"{alphabet[x-1]} {y}";
                        btn.Width = 50;
                        btn.Height = 50;
                        btn.Location = new Point(800 + y * 50 + 50, (x-1) * 50 + 50);
                        this.Controls.Add(btn);
                    }
                    
                }
            }
        }
        private void ChangeRotationButton_Click(object sender, EventArgs e)
        {
            rotation = !rotation;
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
                int currentShipSize = shipSizes[currentShipIndex];
                if(rotation)
                {
                    if(positionX <= 9 - currentShipSize)
                    {
                        if(CheckIfAvailable(positionX, positionY, rotation, currentShipSize, true))
                        {
                            DrawTiles(positionX, positionY, rotation, currentShipSize, true, Color.LightBlue, false);
                        }
                    }
                    else if (positionX >= 0 + currentShipSize)
                    {
                        if (CheckIfAvailable(positionX, positionY, rotation, currentShipSize, false)) {
                            DrawTiles(positionX, positionY, rotation, currentShipSize, false, Color.LightBlue, false);
                        }
                    }
                }
                else
                {
                    if(positionY <= 9 - currentShipSize)
                    {
                        if (CheckIfAvailable(positionX, positionY, rotation, currentShipSize, true))
                        {
                            DrawTiles(positionX, positionY, rotation, currentShipSize, true, Color.LightBlue, false);
                        }
                    }
                    else if (CheckIfAvailable(positionX, positionY, rotation, currentShipSize, false))
                    {
                        DrawTiles(positionX, positionY, rotation, currentShipSize, false, Color.LightBlue, false);
                    }
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
                int currentShipSize = shipSizes[currentShipIndex];
                if (rotation)
                {
                    if (positionX <= 9 - currentShipSize)
                    {
                        if (CheckIfAvailable(positionX, positionY, rotation, currentShipSize, true))
                        {
                            DrawTiles(positionX, positionY, rotation, currentShipSize, true, Color.White, false);
                        }
                    }
                    else if (positionX >= 0 + currentShipSize)
                    {
                        if (CheckIfAvailable(positionX, positionY, rotation, currentShipSize, false))
                        {
                            DrawTiles(positionX, positionY, rotation, currentShipSize, false, Color.White, false);
                        }
                    }
                }
                else
                {
                    if (positionY <= 9 - currentShipSize)
                    {
                        if (CheckIfAvailable(positionX, positionY, rotation, currentShipSize, true))
                        {
                            DrawTiles(positionX, positionY, rotation, currentShipSize, true, Color.White, false);
                        }
                    }
                    else if (CheckIfAvailable(positionX, positionY, rotation, currentShipSize, false))
                    {
                        DrawTiles(positionX, positionY, rotation, currentShipSize, false, Color.White, false);
                    }
                }
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
                int currentShipSize = shipSizes[currentShipIndex];
                Color currentShipColor = shipColors[currentShipIndex];
                if (rotation)
                {
                    if (positionX <= 9 - currentShipSize)
                    {
                        if (CheckIfAvailable(positionX, positionY, rotation, currentShipSize, true))
                        {
                            DrawTiles(positionX, positionY, rotation, currentShipSize, true, currentShipColor, true);
                            currentShipIndex--;
                            ChangeShipPlacementLabel();
                            if (currentShipIndex < 0)
                            {
                                readyButton.Enabled = true;
                            }
                        }
                    }
                    else if (positionX >= 0 + currentShipSize)
                    {
                        if (CheckIfAvailable(positionX, positionY, rotation, currentShipSize, false))
                        {
                            DrawTiles(positionX, positionY, rotation, currentShipSize, false, currentShipColor, true);
                            currentShipIndex--;
                            ChangeShipPlacementLabel();
                            if (currentShipIndex < 0)
                            {
                                readyButton.Enabled = true;
                            }

                        }
                    }
                }
                else
                {
                    if (positionY <= 9 - currentShipSize)
                    {
                        if (CheckIfAvailable(positionX, positionY, rotation, currentShipSize, true))
                        {
                            DrawTiles(positionX, positionY, rotation, currentShipSize, true, currentShipColor, true);
                            currentShipIndex--;
                            ChangeShipPlacementLabel();
                            if (currentShipIndex < 0)
                            {
                                readyButton.Enabled = true;
                            }

                        }
                    }
                    else if (CheckIfAvailable(positionX, positionY, rotation, currentShipSize, false))
                    {
                        DrawTiles(positionX, positionY, rotation, currentShipSize, false, currentShipColor, true);
                        currentShipIndex--;
                        ChangeShipPlacementLabel();
                        if (currentShipIndex < 0)
                        {
                            readyButton.Enabled = true;
                        }

                    }
                }
            }
        }
    }
}
