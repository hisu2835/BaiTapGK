using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace BaiTapGK
{
    /// <summary>
    /// Helper class for handling errors and logging
    /// </summary>
    public static class ErrorHandler
    {
        /// <summary>
        /// Handle and log animation errors
        /// </summary>
        public static void HandleAnimationError(Exception ex, string context = "Animation")
        {
            Debug.WriteLine($"{context} Error: {ex.Message}");
            Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
            
            // Don't show error dialogs for animation issues to avoid disrupting gameplay
            // Just log and continue
        }

        /// <summary>
        /// Handle critical errors that should be shown to user
        /// </summary>
        public static void HandleCriticalError(Exception ex, string context = "Application", bool showDialog = true)
        {
            Debug.WriteLine($"CRITICAL {context} Error: {ex.Message}");
            Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
            
            if (showDialog)
            {
                string message = $"Loi trong {context}:\n{ex.Message}\n\nVui long thu lai.";
                MessageBox.Show(message, "Loi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Safe invoke for UI thread operations
        /// </summary>
        public static void SafeInvoke(Control control, Action action)
        {
            try
            {
                if (control == null || control.IsDisposed)
                {
                    return;
                }

                if (control.InvokeRequired)
                {
                    control.Invoke(action);
                }
                else
                {
                    action();
                }
            }
            catch (Exception ex)
            {
                HandleAnimationError(ex, "UI Thread Invoke");
            }
        }

        /// <summary>
        /// Safe timer disposal
        /// </summary>
        public static void SafeDisposeTimer(System.Windows.Forms.Timer? timer)
        {
            try
            {
                if (timer != null)
                {
                    timer.Stop();
                    timer.Dispose();
                }
            }
            catch (Exception ex)
            {
                HandleAnimationError(ex, "Timer Disposal");
            }
        }

        /// <summary>
        /// Safe control disposal
        /// </summary>
        public static void SafeDisposeControl(Control? control)
        {
            try
            {
                if (control != null && !control.IsDisposed)
                {
                    control.Dispose();
                }
            }
            catch (Exception ex)
            {
                HandleAnimationError(ex, "Control Disposal");
            }
        }

        /// <summary>
        /// Safe font creation with fallbacks
        /// </summary>
        public static Font CreateSafeFont(string fontName, float size, FontStyle style = FontStyle.Regular)
        {
            try
            {
                return new Font(fontName, size, style);
            }
            catch
            {
                try
                {
                    return new Font("Arial", size, style);
                }
                catch
                {
                    try
                    {
                        return new Font(FontFamily.GenericSansSerif, size, style);
                    }
                    catch
                    {
                        // Ultimate fallback
                        return SystemFonts.DefaultFont;
                    }
                }
            }
        }

        /// <summary>
        /// Validate animation parameters
        /// </summary>
        public static bool ValidateAnimationParameters(string choice, Control container)
        {
            if (string.IsNullOrEmpty(choice))
            {
                Debug.WriteLine("Animation Error: Empty choice provided");
                return false;
            }

            if (container == null || container.IsDisposed)
            {
                Debug.WriteLine("Animation Error: Invalid container provided");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Safe sound playing
        /// </summary>
        public static void SafePlaySound(Action soundAction)
        {
            try
            {
                soundAction?.Invoke();
            }
            catch (Exception ex)
            {
                HandleAnimationError(ex, "Sound");
                // Continue without sound
            }
        }
    }
}