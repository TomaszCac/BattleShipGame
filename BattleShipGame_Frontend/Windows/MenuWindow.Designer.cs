namespace BattleShipGame_Frontend.Windows
{
    partial class MenuWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            WelcomeLabel = new Label();
            JoinGameButton = new Button();
            CreateGameButton = new Button();
            LogOutButton = new Button();
            DeleteAccountButton = new Button();
            WinsLabel = new Label();
            LossesLabel = new Label();
            SuspendLayout();
            // 
            // WelcomeLabel
            // 
            WelcomeLabel.AutoSize = true;
            WelcomeLabel.Font = new Font("Times New Roman", 25F);
            WelcomeLabel.Location = new Point(536, 55);
            WelcomeLabel.Name = "WelcomeLabel";
            WelcomeLabel.Size = new Size(258, 39);
            WelcomeLabel.TabIndex = 1;
            WelcomeLabel.Text = "Welcome {User}!";
            // 
            // JoinGameButton
            // 
            JoinGameButton.Location = new Point(558, 195);
            JoinGameButton.Name = "JoinGameButton";
            JoinGameButton.Size = new Size(205, 74);
            JoinGameButton.TabIndex = 2;
            JoinGameButton.Text = "Join Game";
            JoinGameButton.UseVisualStyleBackColor = true;
            JoinGameButton.Click += JoinGameButton_Click;
            // 
            // CreateGameButton
            // 
            CreateGameButton.Location = new Point(558, 326);
            CreateGameButton.Name = "CreateGameButton";
            CreateGameButton.Size = new Size(205, 74);
            CreateGameButton.TabIndex = 3;
            CreateGameButton.Text = "Create Game";
            CreateGameButton.UseVisualStyleBackColor = true;
            // 
            // LogOutButton
            // 
            LogOutButton.Location = new Point(558, 675);
            LogOutButton.Name = "LogOutButton";
            LogOutButton.Size = new Size(205, 74);
            LogOutButton.TabIndex = 4;
            LogOutButton.Text = "Log Out";
            LogOutButton.UseVisualStyleBackColor = true;
            // 
            // DeleteAccountButton
            // 
            DeleteAccountButton.Location = new Point(1167, 708);
            DeleteAccountButton.Name = "DeleteAccountButton";
            DeleteAccountButton.Size = new Size(164, 53);
            DeleteAccountButton.TabIndex = 5;
            DeleteAccountButton.Text = "Delete Account";
            DeleteAccountButton.UseVisualStyleBackColor = true;
            // 
            // WinsLabel
            // 
            WinsLabel.AutoSize = true;
            WinsLabel.Font = new Font("Segoe UI", 15F);
            WinsLabel.Location = new Point(74, 283);
            WinsLabel.Name = "WinsLabel";
            WinsLabel.Size = new Size(119, 28);
            WinsLabel.TabIndex = 6;
            WinsLabel.Text = "Wins: {Wins}";
            // 
            // LossesLabel
            // 
            LossesLabel.AutoSize = true;
            LossesLabel.Font = new Font("Segoe UI", 15F);
            LossesLabel.Location = new Point(74, 345);
            LossesLabel.Name = "LossesLabel";
            LossesLabel.Size = new Size(143, 28);
            LossesLabel.TabIndex = 7;
            LossesLabel.Text = "Losses: {Losses}";
            // 
            // MenuWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1360, 795);
            Controls.Add(LossesLabel);
            Controls.Add(WinsLabel);
            Controls.Add(DeleteAccountButton);
            Controls.Add(LogOutButton);
            Controls.Add(CreateGameButton);
            Controls.Add(JoinGameButton);
            Controls.Add(WelcomeLabel);
            Name = "MenuWindow";
            Text = "MenuWindow";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label WelcomeLabel;
        private Button JoinGameButton;
        private Button CreateGameButton;
        private Button LogOutButton;
        private Button DeleteAccountButton;
        private Label WinsLabel;
        private Label LossesLabel;
    }
}