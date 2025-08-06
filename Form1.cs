using System;
using System.Drawing;
using System.Windows.Forms;

namespace BaiTapGK
{
    public partial class Form1 : Form
    {
        private string playerName = "";
        private bool isLoggedIn = false;

        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Hien thi form dang nhap khi form load
            ShowLoginForm();
        }

        private void ShowLoginForm()
        {
            LoginForm loginForm = new LoginForm();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                playerName = loginForm.PlayerName;
                isLoggedIn = true;
                if (lblWelcome != null)
                    lblWelcome.Text = $"Chao mung, {playerName}!";
                EnableGameButtons(true);
            }
            else
            {
                // Neu nguoi dung huy dang nhap, dong ung dung
                Application.Exit();
            }
        }

        private void EnableGameButtons(bool enabled)
        {
            if (btnSinglePlayer != null)
                btnSinglePlayer.Enabled = enabled;
            if (btnMultiPlayer != null)
                btnMultiPlayer.Enabled = enabled;
            if (btnGameIntro != null)
                btnGameIntro.Enabled = enabled;
        }

        private void btnSinglePlayer_Click(object sender, EventArgs e)
        {
            if (!isLoggedIn)
            {
                MessageBox.Show("Vui long dang nhap truoc!", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SinglePlayerForm singlePlayerForm = new SinglePlayerForm(playerName);
            singlePlayerForm.ShowDialog();
        }

        private void btnMultiPlayer_Click(object sender, EventArgs e)
        {
            if (!isLoggedIn)
            {
                MessageBox.Show("Vui long dang nhap truoc!", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MultiPlayerForm multiPlayerForm = new MultiPlayerForm(playerName);
            multiPlayerForm.ShowDialog();
        }

        private void btnGameIntro_Click(object sender, EventArgs e)
        {
            GameIntroForm gameIntroForm = new GameIntroForm();
            gameIntroForm.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnChangeUser_Click(object sender, EventArgs e)
        {
            isLoggedIn = false;
            EnableGameButtons(false);
            ShowLoginForm();
        }
    }
}
