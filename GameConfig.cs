using System;
using System.Drawing;
using System.Windows.Forms;

namespace BaiTapGK
{
    public static class GameConfig
    {
        // Network settings
        public const int DEFAULT_PORT = 7777;
        public const string DEFAULT_HOST = "127.0.0.1";
        
        // Game settings
        public const int GAME_RESET_DELAY = 3000; // 3 seconds
        
        // UI Colors
        public static readonly Color ROCK_COLOR = Color.LightGray;
        public static readonly Color PAPER_COLOR = Color.LightBlue;
        public static readonly Color SCISSORS_COLOR = Color.LightYellow;
        
        public static readonly Color WIN_COLOR = Color.Green;
        public static readonly Color LOSE_COLOR = Color.Red;
        public static readonly Color TIE_COLOR = Color.Orange;
        
        // Game symbols - Hinh ban tay thay vi bua keo bao
        public const string ROCK_SYMBOL = "?";
        public const string PAPER_SYMBOL = "???";
        public const string SCISSORS_SYMBOL = "??";
        
        // Game choices
        public static readonly string[] CHOICES = { "Da", "Giay", "Keo" };
        
        // Messages
        public static class Messages
        {
            public const string WIN = "Ban thang!";
            public const string LOSE = "Ban thua!";
            public const string TIE = "Hoa!";
            public const string WAITING = "Dang cho doi thu...";
            public const string CHOOSE = "Chon lua chon cua ban";
            public const string LOGIN_REQUIRED = "Vui long dang nhap truoc!";
            public const string ENTER_NAME = "Vui long nhap ten nguoi choi!";
            public const string ENTER_ROOM_ID = "Vui long nhap Room ID!";
            public const string ENTER_SERVER_INFO = "Vui long nhap thong tin server!";
            public const string CONNECTION_SUCCESS = "Ket noi thanh cong!";
            public const string CONNECTION_FAILED = "Ket noi that bai!";
        }

        // Wireshark integration
        public static class Wireshark
        {
            public static readonly string[] COMMON_PATHS = {
                @"C:\Program Files\Wireshark\Wireshark.exe",
                @"C:\Program Files (x86)\Wireshark\Wireshark.exe",
                "wireshark.exe"
            };
            
            public const string DOWNLOAD_URL = "https://www.wireshark.org/";
            public const string FILTER_TEMPLATE = "tcp port {0}";
            public const string CAPTURE_ARGS = "-k -f \"{0}\""; // -k: start, -f: filter
        }
    }
}