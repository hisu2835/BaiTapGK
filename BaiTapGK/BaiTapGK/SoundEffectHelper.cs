using System;
using System.Drawing;
using System.Windows.Forms;
using System.Media;

namespace BaiTapGK
{
    public static class SoundEffectHelper
    {
        private static SoundPlayer soundPlayer = new SoundPlayer();

        /// <summary>
        /// Phat am thanh click
        /// </summary>
        public static void PlayClickSound()
        {
            try
            {
                // Tao am thanh click don gian bang SystemSounds
                SystemSounds.Asterisk.Play();
            }
            catch
            {
                // Khong lam gi neu khong phat duoc am thanh
            }
        }

        /// <summary>
        /// Phat am thanh win
        /// </summary>
        public static void PlayWinSound()
        {
            try
            {
                SystemSounds.Exclamation.Play();
            }
            catch { }
        }

        /// <summary>
        /// Phat am thanh lose
        /// </summary>
        public static void PlayLoseSound()
        {
            try
            {
                SystemSounds.Hand.Play();
            }
            catch { }
        }

        /// <summary>
        /// Phat am thanh hover
        /// </summary>
        public static void PlayHoverSound()
        {
            try
            {
                // Am thanh nhe hon cho hover
                SystemSounds.Question.Play();
            }
            catch { }
        }

        /// <summary>
        /// Phat am thanh countdown
        /// </summary>
        public static void PlayCountdownSound()
        {
            try
            {
                SystemSounds.Beep.Play();
            }
            catch { }
        }
    }
}