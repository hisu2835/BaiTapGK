using System;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace BaiTapGK
{
    public partial class MultiPlayerForm : FullscreenSupportForm
    {
        private string playerName;
        private bool isHost = false;
        private TcpListener? tcpListener;
        private TcpClient? tcpClient;
        private NetworkStream? stream;
        private Thread? tcpListenerThread;
        private Thread? tcpClientThread;
        private string roomId;
        private bool gameStarted = false;
        private string playerChoice = "";
        private string opponentChoice = "";
        private int playerScore = 0;
        private int opponentScore = 0;
        private HandGestureAnimationControl? playerGestureControl;
        private HandGestureAnimationControl? opponentGestureControl;
        private BattleResultControl? battleResultControl;

        public MultiPlayerForm(string playerName)
        {
            InitializeComponent();
            this.playerName = playerName;
            
            // Apply current language first
            ApplyCurrentLanguage();
            
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable; // Cho phep resize
            this.MaximizeBox = true; // Cho phep maximize
            this.MinimizeBox = true;
            
            // Set minimum size
            this.MinimumSize = new Size(540, 600);
            
            // Tao Room ID ngau nhien
            roomId = GenerateRoomId();
            txtRoomId.Text = roomId;
            
            EnableGameButtons(false);
            
            // Hien thi thong tin IP local
            ShowLocalIPInfo();
            
            // Thiet lap hover effects cho cac button
            SetupButtonEffects();
            
            // Subscribe to language change events
            LanguageManager.OnLanguageChanged += OnLanguageChanged;
        }

        private void OnLanguageChanged(string newLanguage)
        {
            ApplyCurrentLanguage();
        }

        private void ApplyCurrentLanguage()
        {
            try
            {
                // Update form title
                this.Text = LanguageManager.GetText("MultiPlayerTitle");
                
                // Update player name label
                lblPlayerName.Text = $"{LanguageManager.GetText("Player")}: {playerName}";
                
                // Update other labels
                if (lblOpponent != null)
                {
                    string opponentName = lblOpponent.Text.Replace("Doi thu: ", "").Replace("Opponent: ", "");
                    if (opponentName == "---" || string.IsNullOrEmpty(opponentName))
                    {
                        lblOpponent.Text = $"{LanguageManager.GetText("Opponent")}: ---";
                    }
                    else
                    {
                        lblOpponent.Text = $"{LanguageManager.GetText("Opponent")}: {opponentName}";
                    }
                }
                
                // Update choice labels
                UpdateChoiceLabels();
                
                // Update status and result labels
                UpdateStatusLabels();
                
                // Update button texts using helper
                LanguageHelper.ApplyLanguage(this);
                
                // Update score display
                UpdateScoreDisplay();
                
                // Update local IP info
                ShowLocalIPInfo();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error applying language to MultiPlayerForm: {ex.Message}");
            }
        }

        private void UpdateChoiceLabels()
        {
            if (lblPlayerChoice != null)
            {
                if (string.IsNullOrEmpty(playerChoice))
                {
                    lblPlayerChoice.Text = $"{LanguageManager.GetText("YourChoice")}: ---";
                }
                else
                {
                    string translatedChoice = LanguageHelper.GetChoiceText(playerChoice);
                    lblPlayerChoice.Text = $"{LanguageManager.GetText("YourChoice")}: {translatedChoice}";
                }
            }

            if (lblOpponentChoice != null)
            {
                string opponentText = LanguageManager.GetText("Opponent");
                if (string.IsNullOrEmpty(opponentChoice))
                {
                    lblOpponentChoice.Text = $"{opponentText}: ---";
                }
                else
                {
                    string translatedChoice = LanguageHelper.GetChoiceText(opponentChoice);
                    lblOpponentChoice.Text = $"{opponentText}: {translatedChoice}";
                }
            }
        }

        private void UpdateStatusLabels()
        {
            if (lblGameResult != null)
            {
                if (lblGameResult.Text.Contains("Chon lua chon") || lblGameResult.Text.Contains("Choose your move"))
                {
                    lblGameResult.Text = LanguageManager.GetText("ChooseYourMove");
                }
                else if (lblGameResult.Text.Contains("Dang cho") || lblGameResult.Text.Contains("Waiting"))
                {
                    lblGameResult.Text = LanguageManager.GetText("WaitingForOpponent");
                }
            }

            if (lblStatus != null)
            {
                string currentStatus = lblStatus.Text.ToLower();
                if (currentStatus.Contains("nhap thong tin") || currentStatus.Contains("enter"))
                {
                    lblStatus.Text = LanguageManager.GetText("EnterServerInfo");
                }
                else if (currentStatus.Contains("dang cho") || currentStatus.Contains("waiting"))
                {
                    lblStatus.Text = LanguageManager.GetText("WaitingForConnection");
                }
                else if (currentStatus.Contains("da ket noi") || currentStatus.Contains("connected"))
                {
                    lblStatus.Text = LanguageManager.GetText("Connected");
                }
            }
        }

        private void UpdateScoreDisplay()
        {
            if (lblScore != null)
            {
                string playerText = LanguageManager.GetText("Player");
                string opponentText = LanguageManager.GetText("Opponent");
                lblScore.Text = $"{LanguageManager.GetText("Score")} - {playerText}: {playerScore} | {opponentText}: {opponentScore}";
            }
        }

        private void SetupButtonEffects()
        {
            // Thiet lap hover effects
            ButtonAnimationHelper.SetupButtonHoverEffects(btnRock);
            ButtonAnimationHelper.SetupButtonHoverEffects(btnPaper);
            ButtonAnimationHelper.SetupButtonHoverEffects(btnScissors);
            
            // Thiet lap mau sac ban dau
            btnRock.BackColor = GameConfig.ROCK_COLOR;
            btnPaper.BackColor = GameConfig.PAPER_COLOR;
            btnScissors.BackColor = GameConfig.SCISSORS_COLOR;
            
            // Thiet lap gesture controls
            SetupGestureControls();
        }

        private void SetupGestureControls()
        {
            // Player gesture control
            playerGestureControl = new HandGestureAnimationControl();
            playerGestureControl.Location = new Point(50, 360);
            playerGestureControl.Size = new Size(100, 100);
            this.Controls.Add(playerGestureControl);

            // Opponent gesture control  
            opponentGestureControl = new HandGestureAnimationControl();
            opponentGestureControl.Location = new Point(370, 360);
            opponentGestureControl.Size = new Size(100, 100);
            this.Controls.Add(opponentGestureControl);

            // Battle result control
            battleResultControl = new BattleResultControl();
            this.Controls.Add(battleResultControl);
            battleResultControl.CenterOnParent();

            // Bring gesture controls to front
            playerGestureControl.BringToFront();
            opponentGestureControl.BringToFront();
            
            // Battle result should be on top of everything
            battleResultControl.BringToFront();
        }

        protected override void AdjustLayoutForFullscreen(bool isFullscreen)
        {
            // Dieu chinh layout khi fullscreen cho multiplayer
            if (isFullscreen)
            {
                ApplyAdvancedFullscreenLayout();
            }
            else
            {
                RestoreWindowedLayout();
            }
        }

        private void ApplyAdvancedFullscreenLayout()
        {
            // Tao cau hinh responsive layout cho multiplayer
            var config = new ResponsiveLayoutConfig
            {
                BaseSize = new Size(540, 600),
                
                // Header section - thong tin player va status
                HeaderSection = new LayoutSection
                {
                    Controls = new List<string> { "lblPlayerName", "lblOpponent", "lblConnectionInfo", "lblStatus" },
                    TopMargin = 30,
                    Spacing = 15
                },
                
                // Control section - network controls
                ControlSection = new LayoutSection
                {
                    Controls = new List<string> { "txtServerIP", "txtPort", "txtRoomId" },
                    TopMargin = 160,
                    Spacing = 20,
                    ControlSize = new Size(120, 30)
                },
                
                // Game section - buttons
                GameSection = new LayoutSection
                {
                    Controls = new List<string> { "btnRock", "btnPaper", "btnScissors" },
                    TopMargin = 320,
                    Spacing = 40,
                    ControlSize = new Size(80, 80)
                },
                
                // Footer section
                FooterSection = new LayoutSection
                {
                    Controls = new List<string> { "btnBack" },
                    TopMargin = 60,
                    ControlSize = new Size(120, 40)
                }
            };
            
            // Ap dung responsive layout
            ResponsiveLayoutManager.ApplyResponsiveLayout(this, config);
            
            // Xu ly cac control dac biet
            ApplySpecialFullscreenControls();
        }

        private void ApplySpecialFullscreenControls()
        {
            var scaleFactor = ResponsiveLayoutManager.CalculateOptimalScaleFactor(
                this.ClientSize, new Size(540, 600));
            var centerX = this.ClientSize.Width / 2;
            
            // Network buttons section - dat ben duoi text boxes
            var networkButtons = new[] { btnCreateRoom, btnJoinRoom };
            var networkButtonY = (int)(220 * scaleFactor);
            var networkButtonWidth = (int)(140 * scaleFactor);
            var networkButtonHeight = (int)(35 * scaleFactor);
            var networkButtonSpacing = (int)(30 * scaleFactor);
            
            var totalNetworkWidth = (networkButtons.Length * networkButtonWidth) + 
                                   ((networkButtons.Length - 1) * networkButtonSpacing);
            var networkStartX = centerX - totalNetworkWidth / 2;
            
            for (int i = 0; i < networkButtons.Length; i++)
            {
                if (networkButtons[i] != null)
                {
                    ResponsiveLayoutManager.SaveOriginalState(networkButtons[i]);
                    networkButtons[i].Size = new Size(networkButtonWidth, networkButtonHeight);
                    networkButtons[i].Location = new Point(
                        networkStartX + (i * (networkButtonWidth + networkButtonSpacing)),
                        networkButtonY
                    );
                    networkButtons[i].Font = new Font(networkButtons[i].Font.FontFamily,
                        Math.Max(networkButtons[i].Font.Size * scaleFactor, 9), FontStyle.Bold);
                }
            }
            
            // Wireshark buttons - smaller, positioned below network buttons
            var wiresharkButtons = new[] { btnOpenWireshark, btnWiresharkHelp };
            var wiresharkButtonY = networkButtonY + networkButtonHeight + (int)(15 * scaleFactor);
            var wiresharkButtonWidth = (int)(100 * scaleFactor);
            var wiresharkButtonHeight = (int)(28 * scaleFactor);
            var wiresharkButtonSpacing = (int)(20 * scaleFactor);
            
            var totalWiresharkWidth = (wiresharkButtons.Length * wiresharkButtonWidth) + 
                                     ((wiresharkButtons.Length - 1) * wiresharkButtonSpacing);
            var wiresharkStartX = centerX - totalWiresharkWidth / 2;
            
            for (int i = 0; i < wiresharkButtons.Length; i++)
            {
                if (wiresharkButtons[i] != null)
                {
                    ResponsiveLayoutManager.SaveOriginalState(wiresharkButtons[i]);
                    wiresharkButtons[i].Size = new Size(wiresharkButtonWidth, wiresharkButtonHeight);
                    wiresharkButtons[i].Location = new Point(
                        wiresharkStartX + (i * (wiresharkButtonWidth + wiresharkButtonSpacing)),
                        wiresharkButtonY
                    );
                    wiresharkButtons[i].Font = new Font(wiresharkButtons[i].Font.FontFamily,
                        Math.Max(wiresharkButtons[i].Font.Size * scaleFactor, 8));
                }
            }
            
            // Gesture controls - symmetric positioning with better spacing
            var gestureY = (int)(420 * scaleFactor);
            var gestureSize = (int)(120 * scaleFactor);
            var gestureSpacing = this.ClientSize.Width / 3; // More dynamic spacing
            
            if (playerGestureControl != null)
            {
                ResponsiveLayoutManager.SaveOriginalState(playerGestureControl);
                playerGestureControl.Size = new Size(gestureSize, gestureSize);
                playerGestureControl.Location = new Point(
                    gestureSpacing - gestureSize / 2,
                    gestureY
                );
            }
            
            if (opponentGestureControl != null)
            {
                ResponsiveLayoutManager.SaveOriginalState(opponentGestureControl);
                opponentGestureControl.Size = new Size(gestureSize, gestureSize);
                opponentGestureControl.Location = new Point(
                    this.ClientSize.Width - gestureSpacing - gestureSize / 2,
                    gestureY
                );
            }
            
            // Choice labels - positioned below gesture controls
            var choiceLabelY = gestureY + gestureSize + (int)(15 * scaleFactor);
            
            if (lblPlayerChoice != null)
            {
                ResponsiveLayoutManager.SaveOriginalState(lblPlayerChoice);
                lblPlayerChoice.Font = new Font(lblPlayerChoice.Font.FontFamily,
                    Math.Max(lblPlayerChoice.Font.Size * scaleFactor, 10));
                lblPlayerChoice.AutoSize = true;
                lblPlayerChoice.Location = new Point(
                    gestureSpacing - lblPlayerChoice.PreferredSize.Width / 2,
                    choiceLabelY
                );
            }
            
            if (lblOpponentChoice != null)
            {
                ResponsiveLayoutManager.SaveOriginalState(lblOpponentChoice);
                lblOpponentChoice.Font = new Font(lblOpponentChoice.Font.FontFamily,
                    Math.Max(lblOpponentChoice.Font.Size * scaleFactor, 10));
                lblOpponentChoice.AutoSize = true;
                lblOpponentChoice.Location = new Point(
                    this.ClientSize.Width - gestureSpacing - lblOpponentChoice.PreferredSize.Width / 2,
                    choiceLabelY
                );
            }
            
            // Game result và score - centered between gestures
            var resultY = choiceLabelY + (int)(40 * scaleFactor);
            
            if (lblGameResult != null)
            {
                ResponsiveLayoutManager.SaveOriginalState(lblGameResult);
                lblGameResult.Font = new Font(lblGameResult.Font.FontFamily,
                    Math.Max(lblGameResult.Font.Size * scaleFactor, 12), FontStyle.Bold);
                lblGameResult.AutoSize = true;
                lblGameResult.Location = new Point(
                    centerX - lblGameResult.PreferredSize.Width / 2,
                    resultY
                );
            }
            
            if (lblScore != null)
            {
                ResponsiveLayoutManager.SaveOriginalState(lblScore);
                lblScore.Font = new Font(lblScore.Font.FontFamily,
                    Math.Max(lblScore.Font.Size * scaleFactor, 11), FontStyle.Bold);
                lblScore.AutoSize = true;
                lblScore.Location = new Point(
                    centerX - lblScore.PreferredSize.Width / 2,
                    resultY + (int)(30 * scaleFactor)
                );
            }
            
            // Scale battle result control
            if (battleResultControl != null)
            {
                battleResultControl.ScaleForFullscreen(scaleFactor);
            }
        }

        private void RestoreWindowedLayout()
        {
            // Khoi phuc layout windowed mode bang cach su dung ResponsiveLayoutManager
            var allControls = this.Controls.Cast<Control>().ToList();
            
            foreach (var control in allControls)
            {
                ResponsiveLayoutManager.RestoreOriginalState(control);
            }
            
            // Restore special controls that might not be in main Controls collection
            if (playerGestureControl != null)
                ResponsiveLayoutManager.RestoreOriginalState(playerGestureControl);
            
            if (opponentGestureControl != null)
                ResponsiveLayoutManager.RestoreOriginalState(opponentGestureControl);
            
            if (battleResultControl != null)
                battleResultControl.ScaleForFullscreen(1.0f);
        }

        private void AdjustGestureControlsForFullscreen(float scaleFactor)
        {
            if (playerGestureControl != null)
            {
                playerGestureControl.Size = new Size(
                    (int)(100 * scaleFactor),
                    (int)(100 * scaleFactor)
                );
                
                if (scaleFactor > 1.0f)
                {
                    playerGestureControl.Location = new Point(
                        (int)(this.ClientSize.Width * 0.25f - playerGestureControl.Width / 2),
                        (int)(this.ClientSize.Height * 0.75f)
                    );
                }
                else
                {
                    playerGestureControl.Location = new Point(50, 360);
                }
            }

            if (opponentGestureControl != null)
            {
                opponentGestureControl.Size = new Size(
                    (int)(100 * scaleFactor),
                    (int)(100 * scaleFactor)
                );
                
                if (scaleFactor > 1.0f)
                {
                    opponentGestureControl.Location = new Point(
                        (int)(this.ClientSize.Width * 0.75f - opponentGestureControl.Width / 2),
                        (int)(this.ClientSize.Height * 0.75f)
                    );
                }
                else
                {
                    opponentGestureControl.Location = new Point(370, 360);
                }
            }
        }

        private void AdjustGameButtonsForFullscreen(float scaleFactor)
        {
            AdjustButtonForFullscreen(btnRock, scaleFactor);
            AdjustButtonForFullscreen(btnPaper, scaleFactor);
            AdjustButtonForFullscreen(btnScissors, scaleFactor);
            AdjustButtonForFullscreen(btnBack, scaleFactor);
        }

        private void AdjustNetworkControlsForFullscreen(float scaleFactor)
        {
            AdjustButtonForFullscreen(btnCreateRoom, scaleFactor);
            AdjustButtonForFullscreen(btnJoinRoom, scaleFactor);
            AdjustButtonForFullscreen(btnOpenWireshark, scaleFactor);
            AdjustButtonForFullscreen(btnWiresharkHelp, scaleFactor);
            
            // Adjust text boxes if needed
            if (scaleFactor > 1.0f)
            {
                AdjustTextBoxForFullscreen(txtServerIP, scaleFactor);
                AdjustTextBoxForFullscreen(txtPort, scaleFactor);
                AdjustTextBoxForFullscreen(txtRoomId, scaleFactor);
            }
        }

        private void AdjustButtonForFullscreen(Button? button, float scaleFactor)
        {
            if (button == null) return;
            
            // Store original size in Tag if not already stored
            if (button.Tag == null)
            {
                button.Tag = new Size(button.Width, button.Height);
            }
            
            Size originalSize = (Size)button.Tag;
            button.Size = new Size(
                (int)(originalSize.Width * scaleFactor),
                (int)(originalSize.Height * scaleFactor)
            );
            
            // Adjust font
            float fontSize = button.Font.Size * scaleFactor;
            button.Font = new Font(button.Font.FontFamily, fontSize, button.Font.Style);
        }

        private void AdjustTextBoxForFullscreen(TextBox? textBox, float scaleFactor)
        {
            if (textBox == null) return;
            
            // Store original size in Tag if not already stored
            if (textBox.Tag == null)
            {
                textBox.Tag = new Size(textBox.Width, textBox.Height);
            }
            
            Size originalSize = (Size)textBox.Tag;
            textBox.Size = new Size(
                (int)(originalSize.Width * scaleFactor),
                (int)(originalSize.Height * scaleFactor)
            );
            
            // Adjust font
            float fontSize = textBox.Font.Size * scaleFactor;
            textBox.Font = new Font(textBox.Font.FontFamily, fontSize, textBox.Font.Style);
        }

        private void ShowLocalIPInfo()
        {
            try
            {
                string localIP = NetworkUtils.GetLocalIPAddress();
                lblConnectionInfo.Text = $"IP Local: {localIP} | Room: {roomId}";
            }
            catch
            {
                lblConnectionInfo.Text = $"Room: {roomId}";
            }
        }

        private string GenerateRoomId()
        {
            return NetworkUtils.GenerateRoomId();
        }

        private void btnCreateRoom_Click(object sender, EventArgs e)
        {
            try
            {
                int port = int.Parse(txtPort.Text);
                
                if (!NetworkUtils.IsPortAvailable(port))
                {
                    string message = LanguageManager.GetFormattedText("PortInUse", port);
                    string title = LanguageManager.GetText("Error");
                    MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                isHost = true;
                tcpListener = new TcpListener(IPAddress.Any, port);
                tcpListener.Start();
                
                tcpListenerThread = new Thread(new ThreadStart(ListenForClients));
                tcpListenerThread.IsBackground = true;
                tcpListenerThread.Start();
                
                lblStatus.Text = LanguageManager.GetText("WaitingForConnection") + $" {NetworkUtils.GetLocalIPAddress()}:{port}";
                lblStatus.ForeColor = Color.Orange;
                btnCreateRoom.Enabled = false;
                btnJoinRoom.Enabled = false;
                
                // Hien thi thong tin cho ban be
                ShowConnectionInfo(port);
            }
            catch (Exception ex)
            {
                string message = LanguageManager.GetText("Error") + $": {ex.Message}";
                string title = LanguageManager.GetText("Error");
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowConnectionInfo(int port)
        {
            string localIP = NetworkUtils.GetLocalIPAddress();
            string title = LanguageManager.GetText("ConnectionInfo");
            string message = $"{LanguageManager.GetText("ConnectionInfo")}:\n\n" +
                           $"Server IP: {localIP}\n" +
                           $"Port: {port}\n" +
                           $"Room ID: {roomId}\n\n" +
                           $"{GetConnectionInstructions()}\n\n" +
                           $"{GetWiresharkInstructions(port)}";
            
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string GetConnectionInstructions()
        {
            return LanguageManager.CurrentLanguage switch
            {
                "vi" => "G?i thông tin này cho b?n bè ?? h? k?t n?i!",
                "en" => "Send this information to friends so they can connect!",
                "zh" => "????????????????",
                "ja" => "????????????????????????",
                "ko" => "???? ??? ? ??? ? ??? ?????!",
                "es" => "¡Envía esta información a tus amigos para que puedan conectarse!",
                "fr" => "Envoyez ces informations à vos amis pour qu'ils puissent se connecter !",
                "de" => "Senden Sie diese Informationen an Freunde, damit sie sich verbinden können!",
                "pt" => "Envie esta informação para amigos para que eles possam se conectar!",
                "ru" => "????????? ??? ?????????? ???????, ????? ??? ????? ????????????!",
                _ => "Send this information to friends so they can connect!"
            };
        }

        private string GetWiresharkInstructions(int port)
        {
            string filterText = LanguageManager.CurrentLanguage switch
            {
                "vi" => "S? d?ng Wireshark ?? monitor traffic:\nFilter: tcp port",
                "en" => "Use Wireshark to monitor traffic:\nFilter: tcp port",
                "zh" => "??Wireshark?????\n???: tcp port",
                "ja" => "Wireshark???????????????\n?????: tcp port",
                "ko" => "Wireshark? ???? ??? ?????\n??: tcp port",
                "es" => "Usar Wireshark para monitorear tráfico:\nFiltro: tcp port",
                "fr" => "Utiliser Wireshark pour surveiller le trafic :\nFiltre : tcp port",
                "de" => "Wireshark zum Überwachen des Verkehrs verwenden:\nFilter: tcp port",
                "pt" => "Use Wireshark para monitorar tráfego:\nFiltro: tcp port",
                "ru" => "??????????? Wireshark ??? ??????????? ???????:\n??????: tcp port",
                _ => "Use Wireshark to monitor traffic:\nFilter: tcp port"
            };

            return $"{filterText} {port}";
        }

        private void btnJoinRoom_Click(object sender, EventArgs e)
        {
            string serverIP = txtServerIP.Text.Trim();
            string portText = txtPort.Text.Trim();
            string inputRoomId = txtRoomId.Text.Trim();
            
            if (string.IsNullOrEmpty(serverIP) || string.IsNullOrEmpty(portText))
            {
                string message = LanguageManager.GetText("EnterServerInfo");
                string title = LanguageManager.GetText("Warning");
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(portText, out int port))
            {
                string message = LanguageManager.GetText("InvalidPort");
                string title = LanguageManager.GetText("Error");
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                isHost = false;
                tcpClient = new TcpClient();
                tcpClient.Connect(serverIP, port);
                stream = tcpClient.GetStream();
                
                tcpClientThread = new Thread(new ThreadStart(ListenForData));
                tcpClientThread.IsBackground = true;
                tcpClientThread.Start();
                
                // Gui ten nguoi choi
                SendMessage($"PLAYER:{playerName}");
                
                lblStatus.Text = LanguageManager.GetText("Connected");
                lblStatus.ForeColor = Color.Green;
                btnCreateRoom.Enabled = false;
                btnJoinRoom.Enabled = false;
                EnableGameButtons(true);
                gameStarted = true;
            }
            catch (Exception ex)
            {
                string message = LanguageManager.GetFormattedText("CannotConnect", serverIP, port) + 
                               $"\n\n{LanguageManager.GetText("Error")}: {ex.Message}";
                string title = LanguageManager.GetText("Error");
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOpenWireshark_Click(object sender, EventArgs e)
        {
            try
            {
                string port = txtPort.Text;
                
                if (WiresharkIntegration.LaunchWireshark(port, out string errorMessage))
                {
                    MessageBox.Show($"Da mo Wireshark voi filter: tcp port {port}\n\n" +
                                   "Bat dau choi game de xem network traffic!\n\n" +
                                   "Tips:\n" +
                                   "- Right-click packet ? Follow TCP Stream\n" +
                                   "- Statistics ? Conversations", 
                                   "Wireshark Started", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(errorMessage, "Loi Wireshark", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    WiresharkIntegration.ShowInstructions(port, txtServerIP.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Loi: {ex.Message}", "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenWiresharkWithFilter(string port)
        {
            try
            {
                // Thu cac duong dan Wireshark thuong gap
                string[] wiresharkPaths = {
                    @"C:\Program Files\Wireshark\Wireshark.exe",
                    @"C:\Program Files (x86)\Wireshark\Wireshark.exe",
                    "wireshark.exe" // Neu co trong PATH
                };

                string wiresharkPath = "";
                foreach (string path in wiresharkPaths)
                {
                    if (File.Exists(path) || path == "wireshark.exe")
                    {
                        wiresharkPath = path;
                        break;
                    }
                }

                if (string.IsNullOrEmpty(wiresharkPath))
                {
                    // Neu khong tim thay Wireshark, hien thi huong dan
                    ShowWiresharkInstructions(port);
                    return;
                }

                // Tao filter cho game traffic
                string filter = $"tcp port {port}";
                
                // Khoi dong Wireshark voi filter
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = wiresharkPath,
                    Arguments = $"-k -f \"{filter}\"", // -k: start capturing, -f: filter
                    UseShellExecute = false
                };

                Process.Start(startInfo);
                
                MessageBox.Show($"Da mo Wireshark voi filter: {filter}\n\n" +
                               "Bat dau choi game de xem network traffic!", 
                               "Wireshark", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                ShowWiresharkInstructions(port);
            }
        }

        private void ShowWiresharkInstructions(string port)
        {
            string instructions = $"HUONG DAN SU DUNG WIRESHARK:\n\n" +
                                 $"1. Mo Wireshark (tai tai: https://www.wireshark.org/)\n" +
                                 $"2. Chon network interface (thuong la Ethernet hoac WiFi)\n" +
                                 $"3. Ap dung filter: tcp port {port}\n" +
                                 $"4. Nhan Start de bat dau capture\n" +
                                 $"5. Choi game de xem traffic!\n\n" +
                                 $"CAC MESSAGE GAME BAN CO THE THAY:\n" +
                                 $"- PLAYER:[ten] - Khi player tham gia\n" +
                                 $"- CHOICE:[Da/Giay/Keo] - Khi player chon\n" +
                                 $"- RESULT:[ket qua] - Ket qua tran dau\n\n" +
                                 $"IP Server: {NetworkUtils.GetLocalIPAddress()}\n" +
                                 $"Port: {port}";

            MessageBox.Show(instructions, "Huong dan Wireshark", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                            lblStatus.Text = LanguageManager.GetText("PlayerJoined");
                            lblStatus.ForeColor = Color.Green;
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
                    lblStatus.Text = $"{LanguageManager.GetText("ServerError")}: {ex.Message}";
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
                    lblStatus.ForeColor = Color.Red;
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
                lblStatus.Text = $"{LanguageManager.GetText("SendMessageError")}: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
            }
        }

        private void btnRock_Click(object sender, EventArgs e)
        {
            // Animation cho button click
            ButtonAnimationHelper.AnimateButtonClick(btnRock, () => {
                MakeChoiceWithAnimation("Da");
            });
        }

        private void btnPaper_Click(object sender, EventArgs e)
        {
            // Animation cho button click
            ButtonAnimationHelper.AnimateButtonClick(btnPaper, () => {
                MakeChoiceWithAnimation("Giay");
            });
        }

        private void btnScissors_Click(object sender, EventArgs e)
        {
            // Animation cho button click
            ButtonAnimationHelper.AnimateButtonClick(btnScissors, () => {
                MakeChoiceWithAnimation("Keo");
            });
        }

        private void MakeChoiceWithAnimation(string choice)
        {
            // Disable buttons va animate
            EnableGameButtonsWithAnimation(false);
            
            // Hien thi choice confirmation
            Button? chosenButton = GetButtonByChoice(choice);
            if (chosenButton != null)
            {
                ButtonAnimationHelper.AnimateChoiceConfirmation(chosenButton);
            }
            
            // Reset gesture controls safely
            playerGestureControl?.Reset();
            opponentGestureControl?.Reset();
            
            // Start countdown animation for multiplayer
            CountdownAnimationHelper.StartCountdown(this, () =>
            {
                // Start player gesture animation
                playerGestureControl?.StartShakeAnimation(choice, (playerChoice) =>
                {
                    lblPlayerChoice.Text = $"Ban chon: {playerChoice}";
                    
                    // Send choice to opponent after animation
                    MakeChoice(choice);
                });
            });
        }

        private void MakeChoice(string choice)
        {
            playerChoice = choice;
            string translatedChoice = LanguageHelper.GetChoiceText(choice);
            lblPlayerChoice.Text = $"{LanguageManager.GetText("YourChoice")}: {translatedChoice}";
            SendMessage($"CHOICE:{choice}");
            
            EnableGameButtons(false);
            lblGameResult.Text = LanguageManager.GetText("WaitingForOpponent");
            
            CheckGameResult();
        }

        private void CheckGameResult()
        {
            if (!string.IsNullOrEmpty(playerChoice) && !string.IsNullOrEmpty(opponentChoice))
            {
                // Start opponent gesture animation when we receive their choice
                opponentGestureControl?.StartShakeAnimation(opponentChoice, (oppChoice) =>
                {
                    lblOpponentChoice.Text = $"Doi thu chon: {oppChoice}";
                    
                    // Show final result after opponent animation
                    ShowFinalMultiplayerResult();
                });
            }
        }

        private void ShowFinalMultiplayerResult()
        {
            string result = GetGameResult(playerChoice, opponentChoice);
            
            // Get opponent name from label
            string opponentName = lblOpponent.Text.Replace("Doi thu: ", "").Trim();
            if (string.IsNullOrEmpty(opponentName) || opponentName == "---")
            {
                opponentName = "DOI THU";
            }
            
            // Show battle result with new control
            battleResultControl?.ShowBattleResult(playerChoice, opponentChoice, result, playerName, opponentName);
            
            if (result.Contains("Ban thang"))
            {
                playerScore++;
                // Win celebration is handled by BattleResultControl
            }
            else if (result.Contains("Ban thua"))
            {
                opponentScore++;
                // Lose sound is handled by BattleResultControl
            }
            
            UpdateScoreWithAnimation();
            
            // Reset cho van tiep theo sau 4 giay
            System.Windows.Forms.Timer resetTimer = new System.Windows.Forms.Timer();
            resetTimer.Interval = 4000; // Longer to see battle result
            resetTimer.Tick += (s, e) =>
            {
                ResetRoundWithAnimation();
                resetTimer.Stop();
                resetTimer.Dispose();
            };
            resetTimer.Start();
        }

        private void ShowResultWithAnimation()
        {
            // Tao hieu ung fade-in cho opponent choice
            lblOpponentChoice.ForeColor = Color.Transparent;
            
            System.Windows.Forms.Timer fadeTimer = new System.Windows.Forms.Timer();
            fadeTimer.Interval = 50;
            int fadeStep = 0;
            
            fadeTimer.Tick += (sender, e) =>
            {
                fadeStep++;
                double progress = Math.Min(fadeStep / 10.0, 1.0);
                
                int alpha = (int)(255 * progress);
                lblOpponentChoice.ForeColor = Color.FromArgb(alpha, Color.Black);
                
                if (fadeStep >= 10)
                {
                    lblOpponentChoice.ForeColor = Color.Black;
                    fadeTimer.Stop();
                    fadeTimer.Dispose();
                }
            };
            fadeTimer.Start();
        }

        private void UpdateScoreWithAnimation()
        {
            UpdateScore();
            
            // Animate score update
            System.Windows.Forms.Timer pulseTimer = new System.Windows.Forms.Timer();
            pulseTimer.Interval = 100;
            int pulseStep = 0;
            Font originalFont = lblScore.Font;
            
            pulseTimer.Tick += (sender, e) =>
            {
                pulseStep++;
                
                if (pulseStep <= 3)
                {
                    // Phong to
                    lblScore.Font = new Font(originalFont.FontFamily, originalFont.Size + 2, FontStyle.Bold);
                    lblScore.ForeColor = Color.DarkBlue;
                }
                else
                {
                    // Thu nho ve ban dau
                    lblScore.Font = originalFont;
                    lblScore.ForeColor = Color.DarkGreen;
                }
                
                if (pulseStep >= 6)
                {
                    pulseTimer.Stop();
                    pulseTimer.Dispose();
                }
            };
            pulseTimer.Start();
        }

        private void ResetRoundWithAnimation()
        {
            // Reset gesture controls safely
            playerGestureControl?.Reset();
            opponentGestureControl?.Reset();
            
            // Animate reset process
            System.Windows.Forms.Timer resetTimer = new System.Windows.Forms.Timer();
            resetTimer.Interval = 150;
            int resetStep = 0;
            
            resetTimer.Tick += (sender, e) =>
            {
                resetStep++;
                
                switch (resetStep)
                {
                    case 1:
                        playerChoice = "";
                        lblPlayerChoice.Text = "Ban chon: ---";
                        lblPlayerChoice.ForeColor = Color.Gray;
                        break;
                    case 2:
                        opponentChoice = "";
                        lblOpponentChoice.Text = "Doi thu chon: ---";
                        lblOpponentChoice.ForeColor = Color.Gray;
                        break;
                    case 3:
                        lblGameResult.Text = "Chon lua chon cua ban";
                        lblGameResult.ForeColor = Color.Black;
                        break;
                    case 4:
                        // Khoi phuc mau sac ban dau
                        lblPlayerChoice.ForeColor = Color.Black;
                        lblOpponentChoice.ForeColor = Color.Black;
                        EnableGameButtonsWithAnimation(true);
                        resetTimer.Stop();
                        resetTimer.Dispose();
                        break;
                }
            };
            resetTimer.Start();
        }

        private string GetGameResult(string player, string opponent)
        {
            if (player == opponent)
                return LanguageManager.GetText("Draw");

            if ((player == "Da" && opponent == "Keo") ||
                (player == "Giay" && opponent == "Da") ||
                (player == "Keo" && opponent == "Giay"))
            {
                return LanguageManager.GetText("YouWin");
            }
            else
            {
                return LanguageManager.GetText("YouLose");
            }
        }

        private void UpdateScore()
        {
            UpdateScoreDisplay();
        }

        private void EnableGameButtons(bool enabled)
        {
            EnableGameButtonsWithAnimation(enabled);
        }

        private void EnableGameButtonsWithAnimation(bool enabled)
        {
            btnRock.Enabled = enabled;
            btnPaper.Enabled = enabled;
            btnScissors.Enabled = enabled;
            
            if (enabled)
            {
                ButtonAnimationHelper.AnimateButtonEnable(btnRock);
                ButtonAnimationHelper.AnimateButtonEnable(btnPaper);
                ButtonAnimationHelper.AnimateButtonEnable(btnScissors);
            }
            else
            {
                ButtonAnimationHelper.AnimateButtonDisable(btnRock);
                ButtonAnimationHelper.AnimateButtonDisable(btnPaper);
                ButtonAnimationHelper.AnimateButtonDisable(btnScissors);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            // Stop animations before closing
            ButtonAnimationHelper.StopCurrentAnimation();
            
            // Stop gesture animations
            playerGestureControl?.StopAnimation();
            opponentGestureControl?.StopAnimation();
            
            // Hide battle result
            battleResultControl?.HideBattleResult();
            
            // Dong ket noi
            try
            {
                stream?.Close();
                tcpClient?.Close();
                tcpListener?.Stop();
                
                // Su dung CancellationToken thay vi Abort()
                tcpListenerThread = null;
                tcpClientThread = null;
            }
            catch { }
            
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Unsubscribe from language change events
            LanguageManager.OnLanguageChanged -= OnLanguageChanged;
            
            // Stop animations before closing
            ButtonAnimationHelper.StopCurrentAnimation();
            
            // Stop gesture animations
            playerGestureControl?.StopAnimation();
            opponentGestureControl?.StopAnimation();
            
            // Hide battle result
            battleResultControl?.HideBattleResult();
            
            // Dong ket noi khi dong form
            try
            {
                stream?.Close();
                tcpClient?.Close();
                tcpListener?.Stop();
                
                tcpListenerThread = null;
                tcpClientThread = null;
            }
            catch { }
            
            base.OnFormClosing(e);
        }

        private void btnWiresharkHelp_Click(object sender, EventArgs e)
        {
            try
            {
                string port = txtPort.Text;
                string serverIP = string.IsNullOrEmpty(txtServerIP.Text) ? NetworkUtils.GetLocalIPAddress() : txtServerIP.Text;
                
                // Hien thi thong tin Wireshark status
                string status = WiresharkIntegration.IsWiresharkInstalled() ? 
                    "Da cai dat" : "Chua cai dat";
                string version = WiresharkIntegration.GetWiresharkVersion();
                
                string info = $"WIRESHARK STATUS:\n" +
                             $"Trang thai: {status}\n" +
                             $"Phien ban: {version}\n\n" +
                             $"Chon hanh dong:";
                
                DialogResult result = MessageBox.Show(info + "\n\nYes = Xem huong dan\nNo = Tao filter file\nCancel = Dong",
                    "Wireshark Help", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                
                switch (result)
                {
                    case DialogResult.Yes:
                        WiresharkIntegration.ShowInstructions(port, serverIP);
                        break;
                    case DialogResult.No:
                        WiresharkIntegration.CreateFilterFile(port);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Loi: {ex.Message}", "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Button? GetButtonByChoice(string choice)
        {
            return choice switch
            {
                "Da" => btnRock,
                "Giay" => btnPaper,
                "Keo" => btnScissors,
                _ => null
            };
        }
    }
}