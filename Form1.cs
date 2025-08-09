using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BaiTapGK
{
    public partial class Form1 : FullscreenSupportForm
    {
        private string playerName = "";
        private bool isLoggedIn = false;
        
        // Luu original layout info
        private Dictionary<Control, Rectangle> originalBounds = new Dictionary<Control, Rectangle>();
        private Font? originalWelcomeFont;

        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable; // Cho phep resize
            this.MaximizeBox = true; // Cho phep maximize
            this.MinimizeBox = true; // Cho phep minimize
            
            // Set minimum size
            this.MinimumSize = new Size(600, 400);
            
            // Store original layout information
            this.Load += (s, e) => StoreOriginalLayout();
        }

        private void StoreOriginalLayout()
        {
            // Luu layout ban dau de restore sau nay
            foreach (Control control in this.Controls)
            {
                if (control != null)
                {
                    originalBounds[control] = control.Bounds;
                }
            }
            
            // Luu font ban dau
            if (lblWelcome != null)
            {
                originalWelcomeFont = new Font(lblWelcome.Font.FontFamily, lblWelcome.Font.Size, lblWelcome.Font.Style);
            }
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

        protected override void AdjustLayoutForFullscreen(bool isFullscreen)
        {
            if (isFullscreen)
            {
                ApplyFullscreenMainMenuLayout();
            }
            else
            {
                RestoreWindowedMainMenuLayout();
            }
        }

        private void ApplyFullscreenMainMenuLayout()
        {
            SuspendLayout();

            // Calculate optimal scale factor
            float baseWidth = 600f;
            float baseHeight = 480f;
            
            float widthScale = this.ClientSize.Width / baseWidth;
            float heightScale = this.ClientSize.Height / baseHeight;
            
            float scaleFactor = Math.Min(widthScale, heightScale) * 0.75f;
            scaleFactor = Math.Max(scaleFactor, 1.2f);
            scaleFactor = Math.Min(scaleFactor, 2.5f);

            int centerX = this.ClientSize.Width / 2;
            int centerY = this.ClientSize.Height / 2;

            // Title - positioned at top center
            if (lblTitle != null)
            {
                lblTitle.Font = new Font(lblTitle.Font.FontFamily, 
                    Math.Max(lblTitle.Font.Size * scaleFactor, 18), FontStyle.Bold);
                lblTitle.AutoSize = true;
                lblTitle.Location = new Point(
                    centerX - lblTitle.PreferredSize.Width / 2,
                    (int)(50 * scaleFactor)
                );
            }

            // Welcome label - below title
            if (lblWelcome != null)
            {
                lblWelcome.Font = new Font(lblWelcome.Font.FontFamily,
                    Math.Max(lblWelcome.Font.Size * scaleFactor, 12), FontStyle.Bold);
                lblWelcome.AutoSize = true;
                lblWelcome.Location = new Point(
                    centerX - lblWelcome.PreferredSize.Width / 2,
                    (int)(100 * scaleFactor)
                );
            }

            // Picture box - centered below welcome
            if (pictureBox1 != null)
            {
                int pictureSize = (int)(120 * scaleFactor);
                pictureBox1.Size = new Size(pictureSize, (int)(pictureSize * 0.8f));
                pictureBox1.Location = new Point(
                    centerX - pictureBox1.Width / 2,
                    (int)(150 * scaleFactor)
                );
            }

            // Main menu buttons - arranged vertically and centered
            var menuButtons = new[] { btnSinglePlayer, btnMultiPlayer, btnGameIntro };
            var validMenuButtons = menuButtons.Where(b => b != null).ToArray();
            
            if (validMenuButtons.Length > 0)
            {
                int buttonWidth = (int)(250 * scaleFactor);
                int buttonHeight = (int)(50 * scaleFactor);
                int buttonSpacing = (int)(20 * scaleFactor);
                int startY = centerY - ((validMenuButtons.Length * buttonHeight + 
                                       (validMenuButtons.Length - 1) * buttonSpacing) / 2);

                for (int i = 0; i < validMenuButtons.Length; i++)
                {
                    var button = validMenuButtons[i];
                    button.Size = new Size(buttonWidth, buttonHeight);
                    button.Location = new Point(
                        centerX - buttonWidth / 2,
                        startY + (i * (buttonHeight + buttonSpacing))
                    );
                    
                    button.Font = new Font(button.Font.FontFamily,
                        Math.Max(button.Font.Size * scaleFactor, 11), FontStyle.Bold);
                }
            }

            // Control buttons - positioned at bottom center
            var controlButtons = new[] { btnChangeUser, btnExit };
            var validControlButtons = controlButtons.Where(b => b != null).ToArray();
            
            if (validControlButtons.Length > 0)
            {
                int controlButtonWidth = (int)(140 * scaleFactor);
                int controlButtonHeight = (int)(40 * scaleFactor);
                int controlButtonSpacing = (int)(30 * scaleFactor);
                int totalControlWidth = (validControlButtons.Length * controlButtonWidth) + 
                                       ((validControlButtons.Length - 1) * controlButtonSpacing);
                int controlStartX = centerX - totalControlWidth / 2;
                int controlY = this.ClientSize.Height - (int)(80 * scaleFactor);

                for (int i = 0; i < validControlButtons.Length; i++)
                {
                    var button = validControlButtons[i];
                    button.Size = new Size(controlButtonWidth, controlButtonHeight);
                    button.Location = new Point(
                        controlStartX + (i * (controlButtonWidth + controlButtonSpacing)),
                        controlY
                    );
                    
                    button.Font = new Font(button.Font.FontFamily,
                        Math.Max(button.Font.Size * scaleFactor, 9), button.Font.Style);
                }
            }

            ResumeLayout(true);
        }

        private void RestoreWindowedMainMenuLayout()
        {
            SuspendLayout();

            // Restore original layout positions
            if (lblTitle != null)
            {
                lblTitle.Font = new Font(lblTitle.Font.FontFamily, 24, FontStyle.Bold);
                lblTitle.Location = new Point(100, 30);
            }

            if (lblWelcome != null)
            {
                lblWelcome.Font = new Font(lblWelcome.Font.FontFamily, 14, FontStyle.Bold);
                lblWelcome.Location = new Point(200, 80);
            }

            if (pictureBox1 != null)
            {
                pictureBox1.Size = new Size(100, 80);
                pictureBox1.Location = new Point(250, 120);
            }

            // Restore menu buttons
            if (btnSinglePlayer != null)
            {
                btnSinglePlayer.Size = new Size(200, 50);
                btnSinglePlayer.Location = new Point(200, 220);
                btnSinglePlayer.Font = new Font(btnSinglePlayer.Font.FontFamily, 14, FontStyle.Bold);
            }

            if (btnMultiPlayer != null)
            {
                btnMultiPlayer.Size = new Size(200, 50);
                btnMultiPlayer.Location = new Point(200, 280);
                btnMultiPlayer.Font = new Font(btnMultiPlayer.Font.FontFamily, 14, FontStyle.Bold);
            }

            if (btnGameIntro != null)
            {
                btnGameIntro.Size = new Size(200, 50);
                btnGameIntro.Location = new Point(200, 340);
                btnGameIntro.Font = new Font(btnGameIntro.Font.FontFamily, 14, FontStyle.Bold);
            }

            // Restore control buttons
            if (btnChangeUser != null)
            {
                btnChangeUser.Size = new Size(120, 35);
                btnChangeUser.Location = new Point(150, 420);
                btnChangeUser.Font = new Font(btnChangeUser.Font.FontFamily, 10, FontStyle.Bold);
            }

            if (btnExit != null)
            {
                btnExit.Size = new Size(120, 35);
                btnExit.Location = new Point(330, 420);
                btnExit.Font = new Font(btnExit.Font.FontFamily, 10, FontStyle.Bold);
            }

            ResumeLayout(true);
        }

        private float GetCurrentScaleFactor()
        {
            // Estimate current scale factor from button size
            if (btnSinglePlayer != null && originalBounds.ContainsKey(btnSinglePlayer))
            {
                var originalSize = originalBounds[btnSinglePlayer].Size;
                if (originalSize.Width > 0)
                {
                    return (float)btnSinglePlayer.Width / originalSize.Width;
                }
            }
            return 1.0f;
        }

        // Cleanup method to be called from Dispose in Designer.cs
        private void CleanupCustomResources()
        {
            originalWelcomeFont?.Dispose();
            originalBounds.Clear();
        }
    }
}
