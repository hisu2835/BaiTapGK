namespace BaiTapGK
{
    partial class GameIntroForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitle;
        private RichTextBox rtbGameRules;
        private Button btnClose;

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
            this.rtbGameRules = new RichTextBox();
            this.btnClose = new Button();
            this.SuspendLayout();
            
            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Bold, GraphicsUnit.Point);
            this.lblTitle.ForeColor = Color.DarkBlue;
            this.lblTitle.Location = new Point(130, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(240, 26);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "GIOI THIEU VA CACH CHOI";
            
            // rtbGameRules
            this.rtbGameRules.BackColor = Color.White;
            this.rtbGameRules.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.rtbGameRules.Location = new Point(20, 60);
            this.rtbGameRules.Name = "rtbGameRules";
            this.rtbGameRules.ReadOnly = true;
            this.rtbGameRules.Size = new Size(460, 300);
            this.rtbGameRules.TabIndex = 1;
            this.rtbGameRules.Text = @"TRO CHOI OAN TU TI (ROCK PAPER SCISSORS)

CACH CHOI:
• Ban va doi thu dong thoi chon mot trong ba lua chon:
  - ? DA (Rock)
  - ??? GIAY (Paper)  
  - ?? KEO (Scissors)

LUAT CHOI:
• DA thang KEO (da nghien nat keo)
• KEO thang GIAY (keo cat giay)
• GIAY thang DA (giay bao da)
• Neu cung chon giong nhau = HOA

CHE DO CHOI:
1. CHOI DON: Choi voi may tinh (AI ngau nhien)
2. CHOI DOI: Choi online voi nguoi khac
   - Tao phong hoac tham gia phong bang ID
   - Can ket noi Internet de choi

MEO:
• Khong co chien thuat co dinh vi game hoan toan dua vao may man
• Hay du doan tam ly doi thu trong che do choi doi
• Thuong thuc va vui choi!

CHUC BAN CHOI VUI VE!";
            
            // btnClose
            this.btnClose.BackColor = Color.LightCoral;
            this.btnClose.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point);
            this.btnClose.Location = new Point(200, 380);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(100, 40);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "DONG";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new EventHandler(this.btnClose_Click);
            
            // GameIntroForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.LightYellow;
            this.ClientSize = new Size(500, 440);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.rtbGameRules);
            this.Controls.Add(this.lblTitle);
            this.Name = "GameIntroForm";
            this.Text = "Gioi thieu Game - Rock Paper Scissors";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}