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
        public Button changeDirectionButton;
        public Button restartBoardButton;
        private Button readyButton;
        private Label statusLabel;
        public Label[,] boardVisual = new Label[10, 10];
        public int[,] board = new int[10, 10];
        public char[] alphabet = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J'];


        public BattleWindow(Session session, bool isHost, User currentUser, TokenService tokenService)
        {
            _tokenService = tokenService;
            _isHost = isHost;
            _currentUser = currentUser;
            _session = session;
            InitializeComponent();
            IdLabel.Text = $"Session ID: {session.Id}";
            SetupBoard();
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
                        pb.TextAlign = ContentAlignment.MiddleCenter;
                        boardVisual[x-1, y] = pb;
                        board[x-1, y] = 0;
                        this.Controls.Add(pb);
                    }
                    
                }
            }
            changeDirectionButton = new Button();
            changeDirectionButton.Location = new Point(625, 50);
            changeDirectionButton.Text = "Rotate";
            changeDirectionButton.Width = 100;
            changeDirectionButton.Height = 50;
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
            this.Controls.Add(changeDirectionButton);
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
    }
}
