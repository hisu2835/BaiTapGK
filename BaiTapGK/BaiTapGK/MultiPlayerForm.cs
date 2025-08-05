using System;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace BaiTapGK
{
    public partial class MultiPlayerForm : Form
    {
        private string playerName;
        private bool isHost = false;
        private TcpListener tcpListener;
        private TcpClient tcpClient;
        private NetworkStream stream;
        private Thread tcpListenerThread;
        private Thread tcpClientThread;
        private string roomId;
        private bool gameStarted = false;
        private string playerChoice = "";
        private string opponentChoice = "";
        private int playerScore = 0;
        private int opponentScore = 0;

        public MultiPlayerForm(string playerName)
        {
            InitializeComponent();
            this.playerName = playerName;
            lblPlayerName.Text = $"Nguoi choi: {playerName}";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            
            // Tao Room ID ngau nhien
            roomId = GenerateRoomId();
            lblRoomId.Text = $"Room ID: {roomId}";
            
            EnableGameButtons(false);
        }

        private string GenerateRoomId()
        {
            return NetworkUtils.GenerateRoomId();
        }

        private void btnCreateRoom_Click(object sender, EventArgs e)
        {
            try
            {
                if (!NetworkUtils.IsPortAvailable(GameConfig.DEFAULT_PORT))
                {
                    MessageBox.Show($"Port {GameConfig.DEFAULT_PORT} dang duoc su dung!", "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                isHost = true;
                tcpListener = new TcpListener(IPAddress.Any, GameConfig.DEFAULT_PORT);
                tcpListener.Start();
                
                tcpListenerThread = new Thread(new ThreadStart(ListenForClients));
                tcpListenerThread.IsBackground = true;
                tcpListenerThread.Start();
                
                lblStatus.Text = "Dang cho nguoi choi khac tham gia...";
                lblStatus.ForeColor = GameConfig.TIE_COLOR;
                btnCreateRoom.Enabled = false;
                btnJoinRoom.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Loi khi tao phong: {ex.Message}", "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnJoinRoom_Click(object sender, EventArgs e)
        {
            string inputRoomId = txtRoomId.Text.Trim();
            if (!NetworkUtils.IsValidRoomId(inputRoomId))
            {
                MessageBox.Show(GameConfig.Messages.ENTER_ROOM_ID, "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                isHost = false;
                tcpClient = new TcpClient();
                tcpClient.Connect(GameConfig.DEFAULT_HOST, GameConfig.DEFAULT_PORT);
                stream = tcpClient.GetStream();
                
                tcpClientThread = new Thread(new ThreadStart(ListenForData));
                tcpClientThread.IsBackground = true;
                tcpClientThread.Start();
                
                // Gui ten nguoi choi
                SendMessage($"PLAYER:{playerName}");
                
                lblStatus.Text = "Da ket noi! San sang choi.";
                lblStatus.ForeColor = GameConfig.WIN_COLOR;
                btnCreateRoom.Enabled = false;
                btnJoinRoom.Enabled = false;
                EnableGameButtons(true);
                gameStarted = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Khong the ket noi den phong: {ex.Message}", "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ListenForClients()
        {
            try
            {
                while (true)
                {
                    using (TcpClient client = tcpListener.AcceptTcpClient())
                    {
                        tcpClient = client;
                        stream = client.GetStream();
                        
                        this.Invoke((MethodInvoker)delegate
                        {
                            lblStatus.Text = "Nguoi choi da tham gia! San sang choi.";
                            lblStatus.ForeColor = GameConfig.WIN_COLOR;
                            EnableGameButtons(true);
                            gameStarted = true;
                        });

                        ListenForData();
                    }
                }
            }
            catch (Exception ex)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    lblStatus.Text = $"Loi server: {ex.Message}";
                    lblStatus.ForeColor = Color.Red;
                });
            }
        }

        private void ListenForData()
        {
            try
            {
                byte[] buffer = new byte[4096];
                while (true)
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;
                    
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    ProcessMessage(message);
                }
            }
            catch (Exception ex)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    lblStatus.Text = $"Mat ket noi: {ex.Message}";
                    lblStatus.ForeColor = GameConfig.LOSE_COLOR;
                    EnableGameButtons(false);
                });
            }
        }

        private void ProcessMessage(string message)
        {
            this.Invoke((MethodInvoker)delegate
            {
                if (message.StartsWith("PLAYER:"))
                {
                    string opponentName = message.Substring(7);
                    lblOpponent.Text = $"Doi thu: {opponentName}";
                }
                else if (message.StartsWith("CHOICE:"))
                {
                    opponentChoice = message.Substring(7);
                    CheckGameResult();
                }
                else if (message.StartsWith("RESULT:"))
                {
                    lblGameResult.Text = message.Substring(7);
                }
            });
        }

        private void SendMessage(string message)
        {
            try
            {
                if (stream != null)
                {
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Loi gui tin nhan: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
            }
        }

        private void btnRock_Click(object sender, EventArgs e)
        {
            MakeChoice("Da");
        }

        private void btnPaper_Click(object sender, EventArgs e)
        {
            MakeChoice("Giay");
        }

        private void btnScissors_Click(object sender, EventArgs e)
        {
            MakeChoice("Keo");
        }

        private void MakeChoice(string choice)
        {
            playerChoice = choice;
            lblPlayerChoice.Text = $"Ban chon: {choice}";
            SendMessage($"CHOICE:{choice}");
            
            EnableGameButtons(false);
            lblGameResult.Text = "Dang cho doi thu...";
            
            CheckGameResult();
        }

        private void CheckGameResult()
        {
            if (!string.IsNullOrEmpty(playerChoice) && !string.IsNullOrEmpty(opponentChoice))
            {
                lblOpponentChoice.Text = $"Doi thu chon: {opponentChoice}";
                
                string result = GetGameResult(playerChoice, opponentChoice);
                lblGameResult.Text = result;
                
                if (result.Contains("Ban thang"))
                {
                    playerScore++;
                    lblGameResult.ForeColor = GameConfig.WIN_COLOR;
                }
                else if (result.Contains("Ban thua"))
                {
                    opponentScore++;
                    lblGameResult.ForeColor = GameConfig.LOSE_COLOR;
                }
                else
                {
                    lblGameResult.ForeColor = GameConfig.TIE_COLOR;
                }
                
                UpdateScore();
                
                // Reset cho van tiep theo
                System.Windows.Forms.Timer resetTimer = new System.Windows.Forms.Timer();
                resetTimer.Interval = 3000; // 3 giay
                resetTimer.Tick += (s, e) =>
                {
                    playerChoice = "";
                    opponentChoice = "";
                    lblPlayerChoice.Text = "Ban chon: ---";
                    lblOpponentChoice.Text = "Doi thu chon: ---";
                    lblGameResult.Text = "Chon lua chon cua ban";
                    lblGameResult.ForeColor = Color.Black;
                    EnableGameButtons(true);
                    resetTimer.Stop();
                    resetTimer.Dispose();
                };
                resetTimer.Start();
            }
        }

        private string GetGameResult(string player, string opponent)
        {
            if (player == opponent)
                return "Hoa!";

            if ((player == "Da" && opponent == "Keo") ||
                (player == "Giay" && opponent == "Da") ||
                (player == "Keo" && opponent == "Giay"))
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
            lblScore.Text = $"Ty so - Ban: {playerScore} | Doi thu: {opponentScore}";
        }

        private void EnableGameButtons(bool enabled)
        {
            btnRock.Enabled = enabled;
            btnPaper.Enabled = enabled;
            btnScissors.Enabled = enabled;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            // Dong ket noi
            try
            {
                stream?.Close();
                tcpClient?.Close();
                tcpListener?.Stop();
                tcpListenerThread?.Abort();
                tcpClientThread?.Abort();
            }
            catch { }
            
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Dong ket noi khi dong form
            try
            {
                stream?.Close();
                tcpClient?.Close();
                tcpListener?.Stop();
                tcpListenerThread?.Abort();
                tcpClientThread?.Abort();
            }
            catch { }
            
            base.OnFormClosing(e);
        }
    }
}