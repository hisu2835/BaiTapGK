using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace BaiTapGK
{
    public static class WiresharkIntegration
    {
        /// <summary>
        /// Tim duong dan Wireshark tren he thong
        /// </summary>
        public static string FindWiresharkPath()
        {
            foreach (string path in GameConfig.Wireshark.COMMON_PATHS)
            {
                if (path == "wireshark.exe")
                {
                    // Kiem tra trong PATH
                    try
                    {
                        ProcessStartInfo testInfo = new ProcessStartInfo
                        {
                            FileName = "where",
                            Arguments = "wireshark.exe",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            CreateNoWindow = true
                        };
                        
                        using (Process process = Process.Start(testInfo))
                        {
                            if (process != null)
                            {
                                process.WaitForExit();
                                if (process.ExitCode == 0)
                                    return "wireshark.exe";
                            }
                        }
                    }
                    catch { }
                }
                else if (File.Exists(path))
                {
                    return path;
                }
            }
            return "";
        }

        /// <summary>
        /// Mo Wireshark voi filter danh rieng
        /// </summary>
        public static bool LaunchWireshark(string port, out string errorMessage)
        {
            errorMessage = "";
            
            try
            {
                string wiresharkPath = FindWiresharkPath();
                
                if (string.IsNullOrEmpty(wiresharkPath))
                {
                    errorMessage = "Khong tim thay Wireshark. Vui long cai dat tai: " + GameConfig.Wireshark.DOWNLOAD_URL;
                    return false;
                }

                string filter = string.Format(GameConfig.Wireshark.FILTER_TEMPLATE, port);
                string arguments = string.Format(GameConfig.Wireshark.CAPTURE_ARGS, filter);
                
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = wiresharkPath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow = false
                };

                Process.Start(startInfo);
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = $"Loi khi khoi dong Wireshark: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Hien thi huong dan su dung Wireshark
        /// </summary>
        public static void ShowInstructions(string port, string serverIP = "")
        {
            if (string.IsNullOrEmpty(serverIP))
                serverIP = NetworkUtils.GetLocalIPAddress();

            string instructions = GenerateInstructions(port, serverIP);
            
            MessageBox.Show(instructions, "Huong dan Wireshark", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Tao huong dan chi tiet
        /// </summary>
        private static string GenerateInstructions(string port, string serverIP)
        {
            return $@"HUONG DAN SU DUNG WIRESHARK CHO GAME:

=== CAI DAT ===
1. Tai Wireshark: {GameConfig.Wireshark.DOWNLOAD_URL}
2. Cai dat va khoi dong lai may

=== SU DUNG ===
1. Mo Wireshark
2. Chon network interface:
   - Ethernet (cho LAN cable)
   - Wi-Fi (cho wireless)
   - Loopback (cho localhost)

3. Ap dung filter: tcp port {port}
4. Nhan nut Start (shark fin icon)
5. Choi game de thay traffic!

=== THONG TIN KET NOI ===
Server IP: {serverIP}
Port: {port}
Filter: tcp port {port}

=== GAME PROTOCOL ===
Cac message ban se thay:
- PLAYER:[ten] ? Nguoi choi tham gia
- CHOICE:[Da/Giay/Keo] ? Lua chon game
- RESULT:[ket qua] ? Ket qua tran

=== TIPS ===
• Right-click packet ? Follow TCP Stream
• Statistics ? Conversations (TCP tab)
• View ? Time Display Format ? Seconds Since Beginning
• Apply display filter: tcp.stream eq 0 (cho stream dau tien)

=== TROUBLESHOOTING ===
• Neu khong thay packets: Kiem tra interface
• Neu filter loi: Xoa filter va thu lai  
• Neu Wireshark crash: Chay as Administrator";
        }

        /// <summary>
        /// Tao file export filter cho Wireshark
        /// </summary>
        public static void CreateFilterFile(string port)
        {
            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string filterFile = Path.Combine(desktopPath, $"game_filter_port_{port}.txt");
                
                string filterContent = $@"# Wireshark Filter cho Game Oan Tu Ti
# Port: {port}
# Copy filter ben duoi va paste vao Wireshark

# Filter chinh - Tat ca traffic game
tcp port {port}

# Filter chi packets co data game  
tcp port {port} and tcp.len > 0

# Filter theo message type
tcp port {port} and tcp contains ""PLAYER""
tcp port {port} and tcp contains ""CHOICE""
tcp port {port} and tcp contains ""RESULT""

# Filter theo IP cu the (thay IP_ADDRESS)
tcp port {port} and ip.addr == IP_ADDRESS

# Filter theo huong traffic
tcp port {port} and tcp.srcport == {port}  # Tu server
tcp port {port} and tcp.dstport == {port}  # Den server

# Advanced filters
tcp.stream eq 0                           # Chi stream dau tien
tcp.flags.syn == 1                        # Chi SYN packets
tcp.flags.fin == 1                        # Chi FIN packets
";

                File.WriteAllText(filterFile, filterContent);
                
                MessageBox.Show($"Da tao file filter tai Desktop:\n{filterFile}\n\n" +
                               "Su dung file nay de copy/paste filters vao Wireshark!",
                               "Filter File", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Khong the tao filter file: {ex.Message}",
                               "Loi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Kiem tra Wireshark co duoc cai dat khong
        /// </summary>
        public static bool IsWiresharkInstalled()
        {
            return !string.IsNullOrEmpty(FindWiresharkPath());
        }

        /// <summary>
        /// Lay phien ban Wireshark
        /// </summary>
        public static string GetWiresharkVersion()
        {
            try
            {
                string wiresharkPath = FindWiresharkPath();
                if (string.IsNullOrEmpty(wiresharkPath))
                    return "Khong cai dat";

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = wiresharkPath,
                    Arguments = "--version",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(startInfo))
                {
                    if (process != null)
                    {
                        string output = process.StandardOutput.ReadToEnd();
                        process.WaitForExit();
                        
                        // Parse version from output
                        var lines = output.Split('\n');
                        if (lines.Length > 0)
                        {
                            return lines[0].Trim();
                        }
                    }
                }
            }
            catch { }
            
            return "Khong xac dinh";
        }
    }
}