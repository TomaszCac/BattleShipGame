namespace BattleShipGame_Frontend
{
    partial class LoginWindow
    {
        private System.ComponentModel.IContainer components = null;

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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Label title;
            LoginButton = new Button();
            UsernameLabel = new Label();
            PasswordLabel = new Label();
            PasswordTextBox = new TextBox();
            UsernameTextBox = new TextBox();
            ForgotLinkLabel = new LinkLabel();
            RegisterButton = new Button();
            StatusLabel = new Label();
            title = new Label();
            SuspendLayout();
            // 
            // title
            // 
            title.AutoSize = true;
            title.Font = new Font("Yu Gothic", 70F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            title.Location = new Point(271, 31);
            title.Name = "title";
            title.Size = new Size(765, 121);
            title.TabIndex = 5;
            title.Text = "Warships Game";
            // 
            // LoginButton
            // 
            LoginButton.Location = new Point(723, 489);
            LoginButton.Name = "LoginButton";
            LoginButton.Size = new Size(158, 37);
            LoginButton.TabIndex = 0;
            LoginButton.Text = "Login";
            LoginButton.UseVisualStyleBackColor = true;
            LoginButton.Click += LoginButton_Click;
            // 
            // UsernameLabel
            // 
            UsernameLabel.AutoSize = true;
            UsernameLabel.Font = new Font("Segoe UI", 13F);
            UsernameLabel.Location = new Point(544, 286);
            UsernameLabel.Name = "UsernameLabel";
            UsernameLabel.Size = new Size(95, 25);
            UsernameLabel.TabIndex = 6;
            UsernameLabel.Text = "Username:";
            // 
            // PasswordLabel
            // 
            PasswordLabel.AutoSize = true;
            PasswordLabel.Font = new Font("Segoe UI", 13F);
            PasswordLabel.Location = new Point(544, 396);
            PasswordLabel.Name = "PasswordLabel";
            PasswordLabel.Size = new Size(91, 25);
            PasswordLabel.TabIndex = 7;
            PasswordLabel.Text = "Password:";
            // 
            // PasswordTextBox
            // 
            PasswordTextBox.Font = new Font("Segoe UI", 12F);
            PasswordTextBox.Location = new Point(544, 424);
            PasswordTextBox.Name = "PasswordTextBox";
            PasswordTextBox.PasswordChar = '*';
            PasswordTextBox.Size = new Size(219, 29);
            PasswordTextBox.TabIndex = 8;
            // 
            // UsernameTextBox
            // 
            UsernameTextBox.Font = new Font("Segoe UI", 12F);
            UsernameTextBox.Location = new Point(544, 314);
            UsernameTextBox.Name = "UsernameTextBox";
            UsernameTextBox.Size = new Size(219, 29);
            UsernameTextBox.TabIndex = 9;
            // 
            // ForgotLinkLabel
            // 
            ForgotLinkLabel.AutoSize = true;
            ForgotLinkLabel.Location = new Point(723, 552);
            ForgotLinkLabel.Name = "ForgotLinkLabel";
            ForgotLinkLabel.Size = new Size(100, 15);
            ForgotLinkLabel.TabIndex = 10;
            ForgotLinkLabel.TabStop = true;
            ForgotLinkLabel.Text = "Forgot password?";
            ForgotLinkLabel.LinkClicked += ForgotLinkLabel_LinkClicked;
            // 
            // RegisterButton
            // 
            RegisterButton.Location = new Point(427, 489);
            RegisterButton.Name = "RegisterButton";
            RegisterButton.Size = new Size(158, 37);
            RegisterButton.TabIndex = 11;
            RegisterButton.Text = "Register";
            RegisterButton.UseVisualStyleBackColor = true;
            RegisterButton.Click += RegisterButton_Click;
            // 
            // StatusLabel
            // 
            StatusLabel.AutoSize = true;
            StatusLabel.Font = new Font("Segoe UI", 12F);
            StatusLabel.Location = new Point(37, 214);
            StatusLabel.Name = "StatusLabel";
            StatusLabel.Size = new Size(0, 21);
            StatusLabel.TabIndex = 12;
            // 
            // MainMenuWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1360, 795);
            Controls.Add(StatusLabel);
            Controls.Add(RegisterButton);
            Controls.Add(ForgotLinkLabel);
            Controls.Add(UsernameTextBox);
            Controls.Add(PasswordTextBox);
            Controls.Add(PasswordLabel);
            Controls.Add(UsernameLabel);
            Controls.Add(title);
            Controls.Add(LoginButton);
            Name = "MainMenuWindow";
            Text = "Login Menu";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button LoginButton;
        private Label UsernameLabel;
        private Label PasswordLabel;
        private TextBox PasswordTextBox;
        private TextBox UsernameTextBox;
        private LinkLabel ForgotLinkLabel;
        private Button RegisterButton;
        private Label StatusLabel;
    }
}
