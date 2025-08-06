using System;
using System.Drawing;
using System.Windows.Forms;

namespace BaiTapGK
{
    public class HandGestureAnimationControl : UserControl
    {
        private System.Windows.Forms.Timer? animationTimer;
        private int animationStep = 0;
        private string finalChoice = "";
        private bool isAnimating = false;
        private Font gestureFont;
        private Action<string>? onAnimationComplete;

        // Cac frames animation cho "quoi quoi"
        private readonly string[] shakeFrames = {
            "?", "?", "?", "?", "?", "?", "?" 
        };

        // Final gestures - Su dung ky tu don gian thay vi emoji phuc tap
        private readonly string rockGesture = "?";      // Bua
        private readonly string paperGesture = "?";     // Giay  
        private readonly string scissorsGesture = "?";   // Keo

        public HandGestureAnimationControl()
        {
            this.Size = new Size(150, 150);
            this.BackColor = Color.Transparent;
            
            // Tao font an toan hon, uu tien fonts ho tro ky tu dac biet
            gestureFont = CreateSafeGestureFont();
            
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | 
                         ControlStyles.UserPaint | 
                         ControlStyles.DoubleBuffer | 
                         ControlStyles.ResizeRedraw, true);
        }

        private Font CreateSafeGestureFont()
        {
            // Thu cac font theo thu tu uu tien
            string[] fontNames = {
                "Segoe UI Emoji",
                "Microsoft YaHei UI", 
                "Arial Unicode MS",
                "Segoe UI Symbol",
                "Arial",
                "Microsoft Sans Serif"
            };

            foreach (string fontName in fontNames)
            {
                try
                {
                    Font testFont = new Font(fontName, 48, FontStyle.Regular);
                    
                    // Test xem font co hien thi duoc ky tu khong
                    using (Bitmap testBitmap = new Bitmap(100, 100))
                    using (Graphics testGraphics = Graphics.FromImage(testBitmap))
                    {
                        testGraphics.DrawString("?", testFont, Brushes.Black, 0, 0);
                        // Neu khong crash thi font OK
                        return testFont;
                    }
                }
                catch
                {
                    // Thu font tiep theo
                    continue;
                }
            }

            // Fallback cuoi cung
            return new Font(FontFamily.GenericSansSerif, 48, FontStyle.Regular);
        }

        /// <summary>
        /// Bat dau animation "quoi quoi" va ket thuc voi choice cu the
        /// </summary>
        public void StartShakeAnimation(string choice, Action<string>? onComplete = null)
        {
            // Validate parameters
            if (!ErrorHandler.ValidateAnimationParameters(choice, this))
            {
                onComplete?.Invoke(choice ?? "");
                return;
            }

            if (isAnimating) 
            {
                StopAnimation(); // Stop current animation before starting new one
            }

            finalChoice = choice;
            onAnimationComplete = onComplete;
            animationStep = 0;
            isAnimating = true;

            // Phat am thanh bat dau
            ErrorHandler.SafePlaySound(() => SoundEffectHelper.PlayCountdownSound());

            // Ensure we're on UI thread
            ErrorHandler.SafeInvoke(this, () =>
            {
                animationTimer = new System.Windows.Forms.Timer();
                animationTimer.Interval = 200; // 200ms moi frame
                animationTimer.Tick += AnimationTimer_Tick;
                animationTimer.Start();

                this.Invalidate(); // Repaint
            });
        }

        private void AnimationTimer_Tick(object? sender, EventArgs e)
        {
            try
            {
                if (!isAnimating || animationTimer == null)
                {
                    return; // Safety check
                }

                animationStep++;

                if (animationStep <= shakeFrames.Length)
                {
                    // Phase 1: Shake animation (quoi quoi)
                    this.Invalidate();
                }
                else if (animationStep <= shakeFrames.Length + 5)
                {
                    // Phase 2: Suspense pause (keep shaking slower)
                    if (animationStep == shakeFrames.Length + 1)
                    {
                        animationTimer.Interval = 100; // Faster for final shakes
                    }
                    this.Invalidate();
                }
                else
                {
                    // Phase 3: Final reveal
                    StopAnimationTimer();
                    isAnimating = false;
                    
                    // Play final sound
                    ErrorHandler.SafePlaySound(() => SoundEffectHelper.PlayClickSound());
                    
                    this.Invalidate();
                    
                    // Callback after animation complete
                    try
                    {
                        onAnimationComplete?.Invoke(finalChoice);
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.HandleAnimationError(ex, "Animation Callback");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleAnimationError(ex, "Animation Timer");
                StopAnimation(); // Stop animation on error
            }
        }

        private void StopAnimationTimer()
        {
            if (animationTimer != null)
            {
                animationTimer.Stop();
                animationTimer.Tick -= AnimationTimer_Tick;
                animationTimer.Dispose();
                animationTimer = null;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (e?.Graphics == null) return;

            try
            {
                base.OnPaint(e);

                Graphics g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                string displayGesture = "";
                Color textColor = Color.Black;
                
                if (!isAnimating)
                {
                    // Show final choice
                    displayGesture = GetGestureByChoice(finalChoice);
                    textColor = GetColorByChoice(finalChoice);
                }
                else if (animationStep <= shakeFrames.Length && animationStep > 0)
                {
                    // Show shaking animation
                    int frameIndex = (animationStep - 1) % shakeFrames.Length;
                    displayGesture = shakeFrames[frameIndex];
                    textColor = Color.Gray;
                }
                else
                {
                    // Suspense phase - show closed fist
                    displayGesture = "?";
                    textColor = Color.DarkGray;
                }

                if (!string.IsNullOrEmpty(displayGesture))
                {
                    // Ve background nhe de gesture noi bat hon
                    using (Brush bgBrush = new SolidBrush(Color.FromArgb(240, 248, 255)))
                    {
                        g.FillEllipse(bgBrush, 10, 10, this.Width - 20, this.Height - 20);
                    }

                    // Ve border
                    using (Pen borderPen = new Pen(Color.LightGray, 2))
                    {
                        g.DrawEllipse(borderPen, 10, 10, this.Width - 20, this.Height - 20);
                    }

                    // Calculate text position for centering
                    SizeF textSize = g.MeasureString(displayGesture, gestureFont);
                    float x = Math.Max(0, (this.Width - textSize.Width) / 2);
                    float y = Math.Max(0, (this.Height - textSize.Height) / 2);

                    // Ve shadow effect
                    using (Brush shadowBrush = new SolidBrush(Color.FromArgb(100, Color.Black)))
                    {
                        g.DrawString(displayGesture, gestureFont, shadowBrush, x + 2, y + 2);
                    }

                    // Ve main gesture
                    using (Brush mainBrush = new SolidBrush(textColor))
                    {
                        g.DrawString(displayGesture, gestureFont, mainBrush, x, y);
                    }
                }

                // Ve glow effect neu dang animate
                if (isAnimating && animationStep > shakeFrames.Length)
                {
                    using (Pen glowPen = new Pen(Color.Gold, 4))
                    {
                        int margin = 8;
                        Rectangle glowRect = new Rectangle(margin, margin, 
                            Math.Max(1, this.Width - 2 * margin), 
                            Math.Max(1, this.Height - 2 * margin));
                        g.DrawEllipse(glowPen, glowRect);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleAnimationError(ex, "Paint");
                
                // Fallback rendering in case of error
                try
                {
                    using (Brush errorBrush = new SolidBrush(Color.Red))
                    using (Font errorFont = new Font("Arial", 24))
                    {
                        e.Graphics.DrawString("?", errorFont, errorBrush, 
                            Math.Max(0, this.Width / 2 - 12), Math.Max(0, this.Height / 2 - 12));
                    }
                }
                catch
                {
                    // Ultimate fallback - just ignore
                }
            }
        }

        private string GetGestureByChoice(string choice)
        {
            if (string.IsNullOrEmpty(choice)) return rockGesture;
            
            return choice switch
            {
                "Da" => rockGesture,    // Bua
                "Giay" => paperGesture, // Giay (khong dau)
                "Keo" => scissorsGesture, // Keo (khong dau)
                _ => rockGesture
            };
        }

        private Color GetColorByChoice(string choice)
        {
            if (string.IsNullOrEmpty(choice)) return Color.Black;
            
            return choice switch
            {
                "Da" => Color.DarkSlateGray,     // Mau dam cho bua
                "Giay" => Color.DarkBlue,        // Mau xanh cho giay
                "Keo" => Color.DarkGoldenrod,    // Mau vang cho keo
                _ => Color.Black
            };
        }

        /// <summary>
        /// Dung animation neu dang chay
        /// </summary>
        public void StopAnimation()
        {
            isAnimating = false;
            StopAnimationTimer();
            
            ErrorHandler.SafeInvoke(this, () => this.Invalidate());
        }

        /// <summary>
        /// Reset ve trang thai ban dau
        /// </summary>
        public void Reset()
        {
            StopAnimation();
            finalChoice = "";
            animationStep = 0;
            onAnimationComplete = null;
            
            ErrorHandler.SafeInvoke(this, () => this.Invalidate());
        }

        /// <summary>
        /// Hien thi static gesture khong co animation
        /// </summary>
        public void ShowStaticGesture(string choice)
        {
            StopAnimation();
            finalChoice = choice ?? "";
            
            ErrorHandler.SafeInvoke(this, () => this.Invalidate());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    StopAnimation();
                    gestureFont?.Dispose();
                }
                catch (Exception ex)
                {
                    ErrorHandler.HandleAnimationError(ex, "Dispose");
                }
            }
            base.Dispose(disposing);
        }
    }

    /// <summary>
    /// Helper class cho countdown animation giong nhu "San-ko-ho"
    /// </summary>
    public static class CountdownAnimationHelper
    {
        /// <summary>
        /// Tao countdown animation giong nhu trong game that
        /// </summary>
        public static void StartCountdown(Control container, Action? onComplete)
        {
            if (container == null || container.IsDisposed)
            {
                onComplete?.Invoke();
                return;
            }

            // Ensure we're on UI thread
            if (container.InvokeRequired)
            {
                try
                {
                    container.Invoke(new Action(() => StartCountdown(container, onComplete)));
                }
                catch
                {
                    // If invoke fails, just call the completion callback
                    onComplete?.Invoke();
                }
                return;
            }

            Label countdownLabel = new Label
            {
                Text = "",
                Font = new Font("Microsoft Sans Serif", 36, FontStyle.Bold),
                ForeColor = Color.Red,
                BackColor = Color.Transparent,
                Size = new Size(200, 100),
                Location = new Point(Math.Max(0, container.Width / 2 - 100), 
                                   Math.Max(0, container.Height / 2 - 50)),
                TextAlign = ContentAlignment.MiddleCenter
            };

            try
            {
                container.Controls.Add(countdownLabel);
                countdownLabel.BringToFront();
            }
            catch
            {
                // If adding control fails, cleanup and call completion
                countdownLabel.Dispose();
                onComplete?.Invoke();
                return;
            }

            System.Windows.Forms.Timer countdownTimer = new System.Windows.Forms.Timer();
            countdownTimer.Interval = 800; // 0.8 giay
            int countStep = 0;
            string[] countTexts = { "SAN!", "KO!", "HO!", "" };

            countdownTimer.Tick += (sender, e) =>
            {
                try
                {
                    if (countStep < countTexts.Length - 1)
                    {
                        if (!countdownLabel.IsDisposed)
                        {
                            countdownLabel.Text = countTexts[countStep];
                            
                            // Animate scale
                            AnimateCountdownText(countdownLabel);
                            
                            // Play sound
                            try
                            {
                                SoundEffectHelper.PlayCountdownSound();
                            }
                            catch
                            {
                                // Continue without sound if error
                            }
                        }
                        
                        countStep++;
                    }
                    else
                    {
                        // Finish countdown
                        countdownTimer.Stop();
                        countdownTimer.Dispose();
                        
                        if (!countdownLabel.IsDisposed && !container.IsDisposed)
                        {
                            try
                            {
                                container.Controls.Remove(countdownLabel);
                            }
                            catch
                            {
                                // Ignore removal errors
                            }
                        }
                        
                        countdownLabel.Dispose();
                        
                        try
                        {
                            onComplete?.Invoke();
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Countdown completion error: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Clean up on error
                    System.Diagnostics.Debug.WriteLine($"Countdown timer error: {ex.Message}");
                    countdownTimer.Stop();
                    countdownTimer.Dispose();
                    
                    try
                    {
                        if (!countdownLabel.IsDisposed && !container.IsDisposed)
                        {
                            container.Controls.Remove(countdownLabel);
                        }
                        countdownLabel.Dispose();
                        onComplete?.Invoke();
                    }
                    catch
                    {
                        // Final fallback
                    }
                }
            };

            countdownTimer.Start();
        }

        private static void AnimateCountdownText(Label label)
        {
            if (label == null || label.IsDisposed) return;

            Font originalFont = label.Font;
            System.Windows.Forms.Timer scaleTimer = new System.Windows.Forms.Timer();
            scaleTimer.Interval = 50;
            int scaleStep = 0;

            scaleTimer.Tick += (sender, e) =>
            {
                try
                {
                    if (label.IsDisposed)
                    {
                        scaleTimer.Stop();
                        scaleTimer.Dispose();
                        return;
                    }

                    scaleStep++;
                    
                    if (scaleStep <= 5)
                    {
                        // Scale up
                        float newSize = Math.Min(originalFont.Size + scaleStep * 4, 72); // Cap max size
                        using (Font newFont = new Font(originalFont.FontFamily, newSize, originalFont.Style))
                        {
                            label.Font = new Font(newFont.FontFamily, newFont.Size, newFont.Style);
                        }
                    }
                    else
                    {
                        // Scale back to original
                        label.Font = new Font(originalFont.FontFamily, originalFont.Size, originalFont.Style);
                        scaleTimer.Stop();
                        scaleTimer.Dispose();
                    }
                }
                catch
                {
                    // Clean up on error
                    scaleTimer.Stop();
                    scaleTimer.Dispose();
                }
            };
            
            scaleTimer.Start();
        }
    }
}