using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace BaiTapGK
{
    /// <summary>
    /// Advanced layout manager cho fullscreen responsive design
    /// </summary>
    public static class ResponsiveLayoutManager
    {
        private static readonly Dictionary<Control, OriginalControlState> originalStates = new();
        
        /// <summary>
        /// Luu trang thai ban dau cua control
        /// </summary>
        public static void SaveOriginalState(Control control)
        {
            if (control == null || originalStates.ContainsKey(control)) return;
            
            originalStates[control] = new OriginalControlState
            {
                Location = control.Location,
                Size = control.Size,
                Font = control.Font?.Clone() as Font,
                Margin = control.Margin,
                Padding = control.Padding
            };
        }
        
        /// <summary>
        /// Khoi phuc trang thai ban dau cua control
        /// </summary>
        public static void RestoreOriginalState(Control control)
        {
            if (control == null || !originalStates.ContainsKey(control)) return;
            
            var state = originalStates[control];
            control.Location = state.Location;
            control.Size = state.Size;
            if (state.Font != null)
                control.Font = state.Font;
            control.Margin = state.Margin;
            control.Padding = state.Padding;
        }
        
        /// <summary>
        /// Xoa trang thai da luu
        /// </summary>
        public static void ClearSavedStates()
        {
            foreach (var state in originalStates.Values)
            {
                state.Font?.Dispose();
            }
            originalStates.Clear();
        }
        
        /// <summary>
        /// Tinh toan scale factor toi uu cho form
        /// </summary>
        public static float CalculateOptimalScaleFactor(Size formSize, Size baseSize)
        {
            float widthScale = (float)formSize.Width / baseSize.Width;
            float heightScale = (float)formSize.Height / baseSize.Height;
            
            // Su dung scale factor nho hon de dam bao tat ca fit
            float scaleFactor = Math.Min(widthScale, heightScale);
            
            // Gioi han scale factor trong khoang hop ly
            return Math.Max(0.5f, Math.Min(scaleFactor, 3.0f));
        }
        
        /// <summary>
        /// Ap dung responsive layout cho container
        /// </summary>
        public static void ApplyResponsiveLayout(Control container, ResponsiveLayoutConfig config)
        {
            if (container == null) return;
            
            container.SuspendLayout();
            
            try
            {
                var scaleFactor = CalculateOptimalScaleFactor(container.ClientSize, config.BaseSize);
                var centerPoint = new Point(container.ClientSize.Width / 2, container.ClientSize.Height / 2);
                
                // Ap dung layout cho tung section (with null checks)
                if (config.HeaderSection != null)
                    ApplyHeaderSection(container, config.HeaderSection, scaleFactor, centerPoint);
                
                if (config.ControlSection != null)
                    ApplyControlSection(container, config.ControlSection, scaleFactor, centerPoint);
                
                if (config.GameSection != null)
                    ApplyGameSection(container, config.GameSection, scaleFactor, centerPoint);
                
                if (config.FooterSection != null)
                    ApplyFooterSection(container, config.FooterSection, scaleFactor, centerPoint);
            }
            finally
            {
                container.ResumeLayout(true);
            }
        }
        
        private static void ApplyHeaderSection(Control container, LayoutSection section, float scaleFactor, Point center)
        {
            if (section?.Controls == null) return;
            
            int yOffset = (int)(section.TopMargin * scaleFactor);
            
            foreach (var controlName in section.Controls)
            {
                var control = FindControlByName(container, controlName);
                if (control == null) continue;
                
                SaveOriginalState(control);
                
                // Scale font
                if (control.Font != null)
                {
                    control.Font = new Font(control.Font.FontFamily, 
                        Math.Max(control.Font.Size * scaleFactor, 8), control.Font.Style);
                }
                
                // Center horizontally
                control.AutoSize = true;
                control.Location = new Point(center.X - control.PreferredSize.Width / 2, yOffset);
                
                yOffset += control.Height + (int)(section.Spacing * scaleFactor);
            }
        }
        
        private static void ApplyControlSection(Control container, LayoutSection section, float scaleFactor, Point center)
        {
            if (section?.Controls == null) return;
            
            int yPos = (int)(section.TopMargin * scaleFactor);
            int controlWidth = (int)(section.ControlSize.Width * scaleFactor);
            int controlHeight = (int)(section.ControlSize.Height * scaleFactor);
            int spacing = (int)(section.Spacing * scaleFactor);
            
            // Calculate total width needed
            int totalWidth = (section.Controls.Count * controlWidth) + ((section.Controls.Count - 1) * spacing);
            int startX = center.X - totalWidth / 2;
            
            for (int i = 0; i < section.Controls.Count; i++)
            {
                var control = FindControlByName(container, section.Controls[i]);
                if (control == null) continue;
                
                SaveOriginalState(control);
                
                control.Size = new Size(controlWidth, controlHeight);
                control.Location = new Point(startX + (i * (controlWidth + spacing)), yPos);
                
                // Scale font if applicable
                if (control.Font != null)
                {
                    control.Font = new Font(control.Font.FontFamily,
                        Math.Max(control.Font.Size * scaleFactor, 8), control.Font.Style);
                }
            }
        }
        
        private static void ApplyGameSection(Control container, LayoutSection section, float scaleFactor, Point center)
        {
            if (section?.Controls == null) return;
            
            int yPos = (int)(section.TopMargin * scaleFactor);
            int buttonSize = (int)(section.ControlSize.Width * scaleFactor);
            int spacing = (int)(section.Spacing * scaleFactor);
            
            // For game buttons (Rock, Paper, Scissors) - arrange horizontally centered
            int totalWidth = (section.Controls.Count * buttonSize) + ((section.Controls.Count - 1) * spacing);
            int startX = center.X - totalWidth / 2;
            
            for (int i = 0; i < section.Controls.Count; i++)
            {
                var control = FindControlByName(container, section.Controls[i]);
                if (control == null) continue;
                
                SaveOriginalState(control);
                
                control.Size = new Size(buttonSize, buttonSize);
                control.Location = new Point(startX + (i * (buttonSize + spacing)), yPos);
                
                // Scale font
                if (control.Font != null)
                {
                    control.Font = new Font(control.Font.FontFamily,
                        Math.Max(control.Font.Size * scaleFactor, 9), control.Font.Style);
                }
            }
        }
        
        private static void ApplyFooterSection(Control container, LayoutSection section, float scaleFactor, Point center)
        {
            if (section?.Controls == null) return;
            
            int yPos = container.ClientSize.Height - (int)(section.TopMargin * scaleFactor);
            
            foreach (var controlName in section.Controls)
            {
                var control = FindControlByName(container, controlName);
                if (control == null) continue;
                
                SaveOriginalState(control);
                
                // Scale font và size
                if (control.Font != null)
                {
                    control.Font = new Font(control.Font.FontFamily,
                        Math.Max(control.Font.Size * scaleFactor, 8), control.Font.Style);
                }
                
                if (control is Button button)
                {
                    button.Size = new Size(
                        (int)(section.ControlSize.Width * scaleFactor),
                        (int)(section.ControlSize.Height * scaleFactor)
                    );
                }
                
                // Center horizontally
                control.Location = new Point(center.X - control.Width / 2, yPos - control.Height);
            }
        }
        
        /// <summary>
        /// Tim control theo ten trong container
        /// </summary>
        private static Control? FindControlByName(Control container, string name)
        {
            return container.Controls.Find(name, true).FirstOrDefault();
        }
    }
    
    /// <summary>
    /// Luu trang thai ban dau cua control
    /// </summary>
    public class OriginalControlState
    {
        public Point Location { get; set; }
        public Size Size { get; set; }
        public Font? Font { get; set; }
        public Padding Margin { get; set; }
        public Padding Padding { get; set; }
    }
    
    /// <summary>
    /// Cau hinh layout cho responsive design
    /// </summary>
    public class ResponsiveLayoutConfig
    {
        public Size BaseSize { get; set; } = new Size(540, 600);
        public LayoutSection? HeaderSection { get; set; }
        public LayoutSection? ControlSection { get; set; }
        public LayoutSection? GameSection { get; set; }
        public LayoutSection? FooterSection { get; set; }
    }
    
    /// <summary>
    /// Cau hinh cho mot section trong layout
    /// </summary>
    public class LayoutSection
    {
        public List<string> Controls { get; set; } = new();
        public int TopMargin { get; set; }
        public int Spacing { get; set; }
        public Size ControlSize { get; set; } = new Size(100, 30);
    }
}