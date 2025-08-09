using System;
using System.Drawing;
using System.Windows.Forms;

namespace BaiTapGK
{
    /// <summary>
    /// Control hien thi ket qua battle voi layout ro rang
    /// </summary>
    public class BattleResultControl : UserControl
    {
        private Label lblPlayerName;
        private Label lblOpponentName;
        private HandGestureAnimationControl playerGesture;
        private HandGestureAnimationControl opponentGesture;
        private Label lblVS;
        private Label lblResult;
        private Label lblResultIcon;
        private bool isVisible = false;

        public BattleResultControl()
        {
            InitializeComponents();
            SetupLayout();
            this.BackColor = Color.FromArgb(240, 248, 255); // AliceBlue
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Size = new Size(400, 280);
            this.Visible = false;
        }

        private void InitializeComponents()
        {
            // Player name label
            lblPlayerName = new Label
            {
                Text = "BAN",
                Font = new Font("Microsoft Sans Serif", 12, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(100, 25),
                Location = new Point(50, 20)
            };

            // Opponent name label  
            lblOpponentName = new Label
            {
                Text = "DOI THU",
                Font = new Font("Microsoft Sans Serif", 12, FontStyle.Bold),
                ForeColor = Color.DarkRed,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(100, 25),
                Location = new Point(250, 20)
            };

            // VS label
            lblVS = new Label
            {
                Text = "VS",
                Font = new Font("Microsoft Sans Serif", 16, FontStyle.Bold),
                ForeColor = Color.Purple,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(40, 30),
                Location = new Point(180, 100)
            };

            // Player gesture
            playerGesture = new HandGestureAnimationControl
            {
                Size = new Size(80, 80),
                Location = new Point(60, 60)
            };

            // Opponent gesture
            opponentGesture = new HandGestureAnimationControl
            {
                Size = new Size(80, 80),
                Location = new Point(260, 60)
            };

            // Result label
            lblResult = new Label
            {
                Text = "",
                Font = new Font("Microsoft Sans Serif", 20, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(380, 40),
                Location = new Point(10, 160)
            };

            // Result icon (su dung Label thay vi PictureBox de tranh loi emoji)
            lblResultIcon = new Label
            {
                Text = "",
                Font = new Font("Arial", 32, FontStyle.Regular), // Font don gian hon
                Size = new Size(60, 60),
                Location = new Point(170, 200),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
        }

        private void SetupLayout()
        {
            this.Controls.AddRange(new Control[] {
                lblPlayerName,
                lblOpponentName,
                playerGesture,
                opponentGesture,
                lblVS,
                lblResult,
                lblResultIcon
            });

            // Bring important elements to front
            lblResult.BringToFront();
            lblVS.BringToFront();
        }

        /// <summary>
        /// Hien thi ket qua battle voi animation
        /// </summary>
        public void ShowBattleResult(string playerChoice, string opponentChoice, string result, string playerName = "BAN", string opponentName = "DOI THU")
        {
            // Update names (bo dau tieng viet)
            lblPlayerName.Text = RemoveVietnameseDiacritics(playerName.ToUpper());
            lblOpponentName.Text = RemoveVietnameseDiacritics(opponentName.ToUpper());

            // Show the control
            this.Visible = true;
            isVisible = true;
            this.BringToFront();

            // Start sequence animation
            StartBattleSequence(playerChoice, opponentChoice, result);
        }

        private void StartBattleSequence(string playerChoice, string opponentChoice, string result)
        {
            // Phase 1: Show gestures simultaneously
            playerGesture.StartShakeAnimation(playerChoice, (_) => {
                // After player animation completes
            });

            // Small delay for opponent
            System.Windows.Forms.Timer delayTimer = new System.Windows.Forms.Timer();
            delayTimer.Interval = 300;
            delayTimer.Tick += (sender, e) =>
            {
                opponentGesture.StartShakeAnimation(opponentChoice, (_) => {
                    // After both animations complete, show result
                    ShowFinalResult(result);
                });
                delayTimer.Stop();
                delayTimer.Dispose();
            };
            delayTimer.Start();
        }

        private void ShowFinalResult(string result)
        {
            // Determine result styling
            Color resultColor;
            string resultText;
            string iconText = "";

            if (result.Contains("thang"))
            {
                resultColor = Color.Green;
                resultText = "*** BAN THANG! ***";
                iconText = "?"; // Kim cuong thay vi trophy
                this.BackColor = Color.FromArgb(240, 255, 240); // Light green
            }
            else if (result.Contains("thua"))
            {
                resultColor = Color.Red;
                resultText = "*** BAN THUA! ***";
                iconText = "X"; // X thay vi broken heart
                this.BackColor = Color.FromArgb(255, 240, 240); // Light red
            }
            else
            {
                resultColor = Color.Orange;
                resultText = "*** HOA! ***";
                iconText = "="; // Dau bang thay vi handshake
                this.BackColor = Color.FromArgb(255, 248, 240); // Light orange
            }

            // Animate result appearance
            AnimateResultDisplay(resultText, resultColor, iconText);
        }

        private void AnimateResultDisplay(string resultText, Color resultColor, string iconText)
        {
            lblResult.Text = resultText;
            lblResult.ForeColor = resultColor;
            lblResultIcon.Text = iconText;
            lblResultIcon.ForeColor = resultColor;

            // Scale animation for result
            System.Windows.Forms.Timer scaleTimer = new System.Windows.Forms.Timer();
            scaleTimer.Interval = 50;
            int scaleStep = 0;
            Font originalFont = lblResult.Font;

            scaleTimer.Tick += (sender, e) =>
            {
                scaleStep++;

                if (scaleStep <= 10)
                {
                    // Scale up
                    float newSize = originalFont.Size + (scaleStep * 2);
                    lblResult.Font = new Font(originalFont.FontFamily, Math.Min(newSize, 32), originalFont.Style);
                }
                else if (scaleStep <= 15)
                {
                    // Scale back down
                    float newSize = 32 - ((scaleStep - 10) * 2);
                    lblResult.Font = new Font(originalFont.FontFamily, Math.Max(newSize, 20), originalFont.Style);
                }
                else
                {
                    // Final size
                    lblResult.Font = new Font(originalFont.FontFamily, 20, originalFont.Style);
                    scaleTimer.Stop();
                    scaleTimer.Dispose();

                    // Start auto-hide timer
                    StartAutoHideTimer();
                }
            };

            scaleTimer.Start();

            // Play appropriate sound
            if (resultText.Contains("THANG"))
            {
                SoundEffectHelper.PlayWinSound();
            }
            else if (resultText.Contains("THUA"))
            {
                SoundEffectHelper.PlayLoseSound();
            }
        }

        private void StartAutoHideTimer()
        {
            System.Windows.Forms.Timer hideTimer = new System.Windows.Forms.Timer();
            hideTimer.Interval = 3000; // 3 seconds
            hideTimer.Tick += (sender, e) =>
            {
                HideBattleResult();
                hideTimer.Stop();
                hideTimer.Dispose();
            };
            hideTimer.Start();
        }

        /// <summary>
        /// An ket qua battle voi fade animation
        /// </summary>
        public void HideBattleResult()
        {
            if (!isVisible) return;

            // Fade out animation
            System.Windows.Forms.Timer fadeTimer = new System.Windows.Forms.Timer();
            fadeTimer.Interval = 50;
            int fadeStep = 0;

            fadeTimer.Tick += (sender, e) =>
            {
                fadeStep++;
                double alpha = Math.Max(0, 1.0 - (fadeStep * 0.1));
                
                // Can't change control alpha directly, so we'll just hide it
                if (fadeStep >= 10)
                {
                    this.Visible = false;
                    isVisible = false;
                    fadeTimer.Stop();
                    fadeTimer.Dispose();

                    // Reset to default state
                    ResetToDefault();
                }
            };
            fadeTimer.Start();
        }

        private void ResetToDefault()
        {
            lblResult.Text = "";
            lblPlayerName.Text = "BAN";
            lblOpponentName.Text = "DOI THU";
            lblResultIcon.Text = "";
            this.BackColor = Color.FromArgb(240, 248, 255);
            
            playerGesture.Reset();
            opponentGesture.Reset();
        }

        /// <summary>
        /// Cap nhat vi tri control de center tren parent
        /// </summary>
        public void CenterOnParent()
        {
            if (this.Parent != null)
            {
                this.Location = new Point(
                    (this.Parent.Width - this.Width) / 2,
                    (this.Parent.Height - this.Width) / 2
                );
            }
        }

        /// <summary>
        /// Scale control cho fullscreen
        /// </summary>
        public void ScaleForFullscreen(float scaleFactor)
        {
            // Scale the main control
            Size originalSize = new Size(400, 280);
            this.Size = new Size(
                (int)(originalSize.Width * scaleFactor),
                (int)(originalSize.Height * scaleFactor)
            );

            // Scale internal components
            ScaleInternalComponents(scaleFactor);
            
            // Re-center after scaling
            CenterOnParent();
        }

        private void ScaleInternalComponents(float scaleFactor)
        {
            // Scale fonts
            if (lblPlayerName != null)
            {
                lblPlayerName.Font = new Font(lblPlayerName.Font.FontFamily, 
                    lblPlayerName.Font.Size * scaleFactor, lblPlayerName.Font.Style);
            }
            
            if (lblOpponentName != null)
            {
                lblOpponentName.Font = new Font(lblOpponentName.Font.FontFamily, 
                    lblOpponentName.Font.Size * scaleFactor, lblOpponentName.Font.Style);
            }
            
            if (lblVS != null)
            {
                lblVS.Font = new Font(lblVS.Font.FontFamily, 
                    lblVS.Font.Size * scaleFactor, lblVS.Font.Style);
            }
            
            if (lblResult != null)
            {
                lblResult.Font = new Font(lblResult.Font.FontFamily, 
                    lblResult.Font.Size * scaleFactor, lblResult.Font.Style);
            }
            
            if (lblResultIcon != null)
            {
                lblResultIcon.Font = new Font(lblResultIcon.Font.FontFamily, 
                    lblResultIcon.Font.Size * scaleFactor, lblResultIcon.Font.Style);
            }

            // Scale gesture controls
            if (playerGesture != null)
            {
                playerGesture.Size = new Size(
                    (int)(80 * scaleFactor),
                    (int)(80 * scaleFactor)
                );
            }
            
            if (opponentGesture != null)
            {
                opponentGesture.Size = new Size(
                    (int)(80 * scaleFactor),
                    (int)(80 * scaleFactor)
                );
            }

            // Reposition components based on new scale
            RepositionComponentsForScale(scaleFactor);
        }

        private void RepositionComponentsForScale(float scaleFactor)
        {
            // Reposition based on scaled control size
            int baseY = 20;
            int gestureY = 60;
            int vsY = 100;
            int resultY = 160;
            int iconY = 200;

            if (lblPlayerName != null)
            {
                lblPlayerName.Location = new Point((int)(50 * scaleFactor), (int)(baseY * scaleFactor));
            }
            
            if (lblOpponentName != null)
            {
                lblOpponentName.Location = new Point((int)(250 * scaleFactor), (int)(baseY * scaleFactor));
            }
            
            if (playerGesture != null)
            {
                playerGesture.Location = new Point((int)(60 * scaleFactor), (int)(gestureY * scaleFactor));
            }
            
            if (opponentGesture != null)
            {
                opponentGesture.Location = new Point((int)(260 * scaleFactor), (int)(gestureY * scaleFactor));
            }
            
            if (lblVS != null)
            {
                lblVS.Location = new Point((int)(180 * scaleFactor), (int)(vsY * scaleFactor));
            }
            
            if (lblResult != null)
            {
                lblResult.Location = new Point((int)(10 * scaleFactor), (int)(resultY * scaleFactor));
                lblResult.Size = new Size((int)(380 * scaleFactor), (int)(40 * scaleFactor));
            }
            
            if (lblResultIcon != null)
            {
                lblResultIcon.Location = new Point((int)(170 * scaleFactor), (int)(iconY * scaleFactor));
                lblResultIcon.Size = new Size((int)(60 * scaleFactor), (int)(60 * scaleFactor));
            }
        }

        /// <summary>
        /// Bo dau tieng viet
        /// </summary>
        private string RemoveVietnameseDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            string[] vietnameseChars = {
                "aáà?ã???????â?????",
                "AÁÀ?Ã???????Â?????",
                "d?", "D?",
                "eéè???ê?????",
                "EÉÈ???Ê?????",
                "iíì???", "IÍÌ???",
                "oóò?õ?ô???????????",
                "OÓÒ?Õ?Ô???????????",
                "uúù?????????",
                "UÚÙ?????????",
                "yý????", "YÝ????"
            };

            string[] replaceChars = {
                "aaaaaaaaaaaaaaaa",
                "AAAAAAAAAAAAAAAA",
                "dd", "DD",
                "eeeeeeeeeee",
                "EEEEEEEEEEE",
                "iiiiii", "IIIIII",
                "ooooooooooooooooo",
                "OOOOOOOOOOOOOOOOO",
                "uuuuuuuuuuuu",
                "UUUUUUUUUUUU",
                "yyyyyy", "YYYYYY"
            };

            for (int i = 0; i < vietnameseChars.Length; i++)
            {
                for (int j = 0; j < vietnameseChars[i].Length; j++)
                {
                    text = text.Replace(vietnameseChars[i][j], replaceChars[i][0]);
                }
            }

            return text;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                playerGesture?.Dispose();
                opponentGesture?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}