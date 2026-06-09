namespace BattleShipGame_Frontend
{
    partial class SessionsWindow
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
            SessionsLayout = new FlowLayoutPanel();
            BackButton = new Button();
            RefreshButton = new Button();
            StatusLabel = new Label();
            SuspendLayout();
            // 
            // SessionsLayout
            // 
            SessionsLayout.FlowDirection = FlowDirection.TopDown;
            SessionsLayout.Location = new Point(12, 12);
            SessionsLayout.Name = "SessionsLayout";
            SessionsLayout.Size = new Size(1337, 669);
            SessionsLayout.TabIndex = 1;
            // 
            // BackButton
            // 
            BackButton.Location = new Point(25, 712);
            BackButton.Name = "BackButton";
            BackButton.Size = new Size(154, 51);
            BackButton.TabIndex = 2;
            BackButton.Text = "Back";
            BackButton.UseVisualStyleBackColor = true;
            // 
            // RefreshButton
            // 
            RefreshButton.Location = new Point(1184, 712);
            RefreshButton.Name = "RefreshButton";
            RefreshButton.Size = new Size(154, 51);
            RefreshButton.TabIndex = 3;
            RefreshButton.Text = "Refresh";
            RefreshButton.UseVisualStyleBackColor = true;
            // 
            // StatusLabel
            // 
            StatusLabel.AutoSize = true;
            StatusLabel.Font = new Font("Segoe UI", 14F);
            StatusLabel.Location = new Point(688, 722);
            StatusLabel.Name = "StatusLabel";
            StatusLabel.Size = new Size(0, 25);
            StatusLabel.TabIndex = 4;
            // 
            // SessionsWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1360, 795);
            Controls.Add(StatusLabel);
            Controls.Add(RefreshButton);
            Controls.Add(BackButton);
            Controls.Add(SessionsLayout);
            Name = "SessionsWindow";
            Text = "SessionsWindow";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FlowLayoutPanel SessionsLayout;
        private Button BackButton;
        private Button RefreshButton;
        private Label StatusLabel;
    }
}