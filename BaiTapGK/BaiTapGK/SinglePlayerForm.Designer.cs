namespace BaiTapGK
{
    partial class SinglePlayerForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitle;
        private Label lblPlayerName;
        private Label lblScore;
        private Button btnRock;
        private Button btnPaper;
        private Button btnScissors;
        private Label lblPlayerChoice;
        private Label lblComputerChoice;
        private Label lblResult;
        private Button btnReset;
        private Button btnBack;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new Label();
            this.lblPlayerName = new Label();
            this.lblScore = new Label();
            this.btnRock = new Button();
            this.btnPaper = new Button();
            this.btnScissors = new Button();
            this.lblPlayerChoice = new Label();
            this.lblComputerChoice = new Label();
            this.lblResult = new Label();
            this.btnReset = new Button();
            this.btnBack = new Button();
            this.SuspendLayout();
            
            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Bold, GraphicsUnit.Point);
            this.lblTitle.ForeColor = Color.DarkBlue;
            this.lblTitle.Location = new Point(150, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(200, 29);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "CHOI DON VOI MAY";
            
            // lblPlayerName
            this.lblPlayerName.AutoSize = true;
            this.lblPlayerName.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point);
            this.lblPlayerName.Location = new Point(30, 70);
            this.lblPlayerName.Name = "lblPlayerName";
            this.lblPlayerName.Size = new Size(150, 20);
            this.lblPlayerName.TabIndex = 1;
            this.lblPlayerName.Text = "Nguoi choi: ";
            
            // lblScore
            this.lblScore.AutoSize = true;
            this.lblScore.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point);
            this.lblScore.ForeColor = Color.DarkGreen;
            this.lblScore.Location = new Point(30, 100);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new Size(200, 20);
            this.lblScore.TabIndex = 2;
            this.lblScore.Text = "Ty so: 0 - 0";
            
            // btnRock
            this.btnRock.BackColor = Color.LightGray;
            this.btnRock.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold, GraphicsUnit.Point);
            this.btnRock.Location = new Point(50, 150);
            this.btnRock.Name = "btnRock";
            this.btnRock.Size = new Size(100, 80);
            this.btnRock.TabIndex = 3;
            this.btnRock.Text = "?\nDA";
            this.btnRock.UseVisualStyleBackColor = false;
            this.btnRock.Click += new EventHandler(this.btnRock_Click);
            
            // btnPaper
            this.btnPaper.BackColor = Color.LightBlue;
            this.btnPaper.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold, GraphicsUnit.Point);
            this.btnPaper.Location = new Point(200, 150);
            this.btnPaper.Name = "btnPaper";
            this.btnPaper.Size = new Size(100, 80);
            this.btnPaper.TabIndex = 4;
            this.btnPaper.Text = "?\nGIAY";
            this.btnPaper.UseVisualStyleBackColor = false;
            this.btnPaper.Click += new EventHandler(this.btnPaper_Click);
            
            // btnScissors
            this.btnScissors.BackColor = Color.LightYellow;
            this.btnScissors.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold, GraphicsUnit.Point);
            this.btnScissors.Location = new Point(350, 150);
            this.btnScissors.Name = "btnScissors";
            this.btnScissors.Size = new Size(100, 80);
            this.btnScissors.TabIndex = 5;
            this.btnScissors.Text = "??\nKEO";
            this.btnScissors.UseVisualStyleBackColor = false;
            this.btnScissors.Click += new EventHandler(this.btnScissors_Click);
            
            // lblPlayerChoice
            this.lblPlayerChoice.AutoSize = true;
            this.lblPlayerChoice.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblPlayerChoice.Location = new Point(50, 260);
            this.lblPlayerChoice.Name = "lblPlayerChoice";
            this.lblPlayerChoice.Size = new Size(120, 20);
            this.lblPlayerChoice.TabIndex = 6;
            this.lblPlayerChoice.Text = "Ban chon: ---";
            
            // lblComputerChoice
            this.lblComputerChoice.AutoSize = true;
            this.lblComputerChoice.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblComputerChoice.Location = new Point(50, 290);
            this.lblComputerChoice.Name = "lblComputerChoice";
            this.lblComputerChoice.Size = new Size(120, 20);
            this.lblComputerChoice.TabIndex = 7;
            this.lblComputerChoice.Text = "May chon: ---";
            
            // lblResult
            this.lblResult.AutoSize = true;
            this.lblResult.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold, GraphicsUnit.Point);
            this.lblResult.Location = new Point(50, 330);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new Size(120, 24);
            this.lblResult.TabIndex = 8;
            this.lblResult.Text = "Ket qua: ---";
            
            // btnReset
            this.btnReset.BackColor = Color.LightCoral;
            this.btnReset.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold, GraphicsUnit.Point);
            this.btnReset.Location = new Point(250, 380);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new Size(100, 35);
            this.btnReset.TabIndex = 9;
            this.btnReset.Text = "RESET";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new EventHandler(this.btnReset_Click);
            
            // btnBack
            this.btnBack.BackColor = Color.LightGray;
            this.btnBack.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold, GraphicsUnit.Point);
            this.btnBack.Location = new Point(370, 380);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new Size(100, 35);
            this.btnBack.TabIndex = 10;
            this.btnBack.Text = "QUAY LAI";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new EventHandler(this.btnBack_Click);
            
            // SinglePlayerForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.LightGreen;
            this.ClientSize = new Size(500, 450);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.lblComputerChoice);
            this.Controls.Add(this.lblPlayerChoice);
            this.Controls.Add(this.btnScissors);
            this.Controls.Add(this.btnPaper);
            this.Controls.Add(this.btnRock);
            this.Controls.Add(this.lblScore);
            this.Controls.Add(this.lblPlayerName);
            this.Controls.Add(this.lblTitle);
            this.Name = "SinglePlayerForm";
            this.Text = "Choi Don - Rock Paper Scissors";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}