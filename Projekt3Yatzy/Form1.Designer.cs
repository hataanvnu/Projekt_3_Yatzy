namespace Projekt3Yatzy
{
    partial class FormStartPage
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
            this.labelEnterYourName = new System.Windows.Forms.Label();
            this.textBoxEnterYourName = new System.Windows.Forms.TextBox();
            this.buttonStartGame = new System.Windows.Forms.Button();
            this.labelWaitingForPlayer = new System.Windows.Forms.Label();
            this.labelUserNameTaken = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelEnterYourName
            // 
            this.labelEnterYourName.AutoSize = true;
            this.labelEnterYourName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEnterYourName.Location = new System.Drawing.Point(183, 134);
            this.labelEnterYourName.Name = "labelEnterYourName";
            this.labelEnterYourName.Size = new System.Drawing.Size(207, 29);
            this.labelEnterYourName.TabIndex = 0;
            this.labelEnterYourName.Text = "Enter your name:";
            // 
            // textBoxEnterYourName
            // 
            this.textBoxEnterYourName.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxEnterYourName.Location = new System.Drawing.Point(77, 166);
            this.textBoxEnterYourName.Name = "textBoxEnterYourName";
            this.textBoxEnterYourName.Size = new System.Drawing.Size(444, 45);
            this.textBoxEnterYourName.TabIndex = 1;
            // 
            // buttonStartGame
            // 
            this.buttonStartGame.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStartGame.Location = new System.Drawing.Point(188, 297);
            this.buttonStartGame.Name = "buttonStartGame";
            this.buttonStartGame.Size = new System.Drawing.Size(207, 42);
            this.buttonStartGame.TabIndex = 2;
            this.buttonStartGame.Text = "Start Game";
            this.buttonStartGame.UseVisualStyleBackColor = true;
            this.buttonStartGame.Click += new System.EventHandler(this.buttonStartGame_Click);
            // 
            // labelWaitingForPlayer
            // 
            this.labelWaitingForPlayer.AutoSize = true;
            this.labelWaitingForPlayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelWaitingForPlayer.Location = new System.Drawing.Point(119, 437);
            this.labelWaitingForPlayer.Name = "labelWaitingForPlayer";
            this.labelWaitingForPlayer.Size = new System.Drawing.Size(352, 20);
            this.labelWaitingForPlayer.TabIndex = 3;
            this.labelWaitingForPlayer.Text = "Please wait. Connecting you to other players...";
            this.labelWaitingForPlayer.Visible = false;
            // 
            // labelUserNameTaken
            // 
            this.labelUserNameTaken.AutoSize = true;
            this.labelUserNameTaken.ForeColor = System.Drawing.Color.Red;
            this.labelUserNameTaken.Location = new System.Drawing.Point(77, 218);
            this.labelUserNameTaken.Name = "labelUserNameTaken";
            this.labelUserNameTaken.Size = new System.Drawing.Size(447, 17);
            this.labelUserNameTaken.TabIndex = 4;
            this.labelUserNameTaken.Text = "Use your imagination instead of other people\'s usernames. Try again.";
            this.labelUserNameTaken.Visible = false;
            // 
            // FormStartPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(594, 560);
            this.Controls.Add(this.labelUserNameTaken);
            this.Controls.Add(this.labelWaitingForPlayer);
            this.Controls.Add(this.buttonStartGame);
            this.Controls.Add(this.textBoxEnterYourName);
            this.Controls.Add(this.labelEnterYourName);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(612, 607);
            this.MinimumSize = new System.Drawing.Size(612, 607);
            this.Name = "FormStartPage";
            this.Text = "Yatzy";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelEnterYourName;
        private System.Windows.Forms.TextBox textBoxEnterYourName;
        private System.Windows.Forms.Button buttonStartGame;
        private System.Windows.Forms.Label labelWaitingForPlayer;
        private System.Windows.Forms.Label labelUserNameTaken;
    }
}

