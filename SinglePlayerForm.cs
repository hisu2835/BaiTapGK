using System;
using System.Drawing;
using System.Windows.Forms;

namespace BaiTapGK
{
    public partial class SinglePlayerForm : Form
    {
        private Random random;
        private int playerScore = 0;
        private int computerScore = 0;
        private string playerName;

        public SinglePlayerForm(string playerName)
        {
            InitializeComponent();
            this.playerName = playerName;
            random = new Random();
            lblPlayerName.Text = $"Nguoi choi: {playerName}";
            UpdateScore();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
        }

        private void btnRock_Click(object sender, EventArgs e)
        {
            PlayGame("Da");
        }

        private void btnPaper_Click(object sender, EventArgs e)
        {
            PlayGame("Giay");
        }

        private void btnScissors_Click(object sender, EventArgs e)
        {
            PlayGame("Keo");
        }

        private void PlayGame(string playerChoice)
        {
            string[] choices = { "Da", "Giay", "Keo" };
            string computerChoice = choices[random.Next(3)];

            lblPlayerChoice.Text = $"Ban chon: {playerChoice}";
            lblComputerChoice.Text = $"May chon: {computerChoice}";

            string result = GetGameResult(playerChoice, computerChoice);
            lblResult.Text = result;

            if (result.Contains("Ban thang"))
            {
                playerScore++;
                lblResult.ForeColor = Color.Green;
            }
            else if (result.Contains("Ban thua"))
            {
                computerScore++;
                lblResult.ForeColor = Color.Red;
            }
            else
            {
                lblResult.ForeColor = Color.Orange;
            }

            UpdateScore();
        }

        private string GetGameResult(string player, string computer)
        {
            if (player == computer)
                return "Hoa!";

            if ((player == "Da" && computer == "Keo") ||
                (player == "Giay" && computer == "Da") ||
                (player == "Keo" && computer == "Giay"))
            {
                return "Ban thang!";
            }
            else
            {
                return "Ban thua!";
            }
        }

        private void UpdateScore()
        {
            lblScore.Text = $"Ty so - {playerName}: {playerScore} | May: {computerScore}";
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            playerScore = 0;
            computerScore = 0;
            lblPlayerChoice.Text = "Ban chon: ---";
            lblComputerChoice.Text = "May chon: ---";
            lblResult.Text = "Ket qua: ---";
            lblResult.ForeColor = Color.Black;
            UpdateScore();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}