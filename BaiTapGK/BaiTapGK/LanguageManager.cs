using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace BaiTapGK
{
    /// <summary>
    /// Quan ly ngon ngu cho ung dung Rock Paper Scissors
    /// Supports multiple languages with easy switching and persistent settings
    /// </summary>
    public static class LanguageManager
    {
        // Supported languages - ordered by usage popularity
        public static readonly Dictionary<string, LanguageInfo> SupportedLanguages = new()
        {
            { "vi", new LanguageInfo("vi", "Ti?ng Vi?t", "Vietnamese", "????") },
            { "en", new LanguageInfo("en", "English", "English", "????") },
            { "zh", new LanguageInfo("zh", "??", "Chinese (Simplified)", "????") },
            { "ja", new LanguageInfo("ja", "???", "Japanese", "????") },
            { "ko", new LanguageInfo("ko", "???", "Korean", "????") },
            { "es", new LanguageInfo("es", "Español", "Spanish", "????") },
            { "fr", new LanguageInfo("fr", "Français", "French", "????") },
            { "de", new LanguageInfo("de", "Deutsch", "German", "????") },
            { "pt", new LanguageInfo("pt", "Português", "Portuguese", "????") },
            { "ru", new LanguageInfo("ru", "???????", "Russian", "????") }
        };

        private static string currentLanguage = "vi"; // Default to Vietnamese
        private static Dictionary<string, Dictionary<string, string>> translations = new();
        private static readonly string settingsFile = "language_settings.json";

        /// <summary>
        /// Current selected language code
        /// </summary>
        public static string CurrentLanguage 
        { 
            get => currentLanguage;
            private set 
            {
                if (currentLanguage != value)
                {
                    currentLanguage = value;
                    OnLanguageChanged?.Invoke(value);
                }
            }
        }

        /// <summary>
        /// Event fired when language changes
        /// </summary>
        public static event Action<string>? OnLanguageChanged;

        static LanguageManager()
        {
            InitializeTranslations();
            LoadLanguageSettings();
        }

        /// <summary>
        /// Get translated text for a key
        /// </summary>
        public static string GetText(string key)
        {
            if (translations.ContainsKey(CurrentLanguage) && 
                translations[CurrentLanguage].ContainsKey(key))
            {
                return translations[CurrentLanguage][key];
            }

            // Fallback to English if available
            if (CurrentLanguage != "en" && 
                translations.ContainsKey("en") && 
                translations["en"].ContainsKey(key))
            {
                return translations["en"][key];
            }

            // Fallback to Vietnamese if available
            if (CurrentLanguage != "vi" && 
                translations.ContainsKey("vi") && 
                translations["vi"].ContainsKey(key))
            {
                return translations["vi"][key];
            }

            // Return key if no translation found
            return $"[{key}]";
        }

        /// <summary>
        /// Set current language
        /// </summary>
        public static bool SetLanguage(string languageCode)
        {
            if (SupportedLanguages.ContainsKey(languageCode))
            {
                CurrentLanguage = languageCode;
                SaveLanguageSettings();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get current language info
        /// </summary>
        public static LanguageInfo GetCurrentLanguageInfo()
        {
            return SupportedLanguages.TryGetValue(CurrentLanguage, out var info) ? 
                   info : SupportedLanguages["vi"];
        }

        /// <summary>
        /// Initialize all translation dictionaries
        /// </summary>
        private static void InitializeTranslations()
        {
            // Vietnamese (Default)
            translations["vi"] = new Dictionary<string, string>
            {
                // Common
                {"AppTitle", "Rock Paper Scissors"},
                {"Player", "Ng??i ch?i"},
                {"Computer", "Máy tính"},
                {"Opponent", "??i th?"},
                {"Score", "T? s?"},
                {"Back", "Quay l?i"},
                {"Close", "?óng"},
                {"Cancel", "H?y"},
                {"OK", "??ng ý"},
                {"Yes", "Có"},
                {"No", "Không"},
                {"Language", "Ngôn ng?"},
                {"Settings", "Cài ??t"},

                // Login Form
                {"LoginTitle", "??NG NH?P"},
                {"PlayerNameLabel", "Tên ng??i ch?i:"},
                {"EnterPlayerName", "Vui lòng nh?p tên ng??i ch?i!"},
                {"Login", "??ng nh?p"},

                // Main Menu
                {"MainMenuTitle", "ROCK PAPER SCISSORS"},
                {"SinglePlayer", "Ch?i ??n"},
                {"MultiPlayer", "Ch?i ?ôi"},
                {"GameRules", "Lu?t ch?i"},
                {"Exit", "Thoát"},

                // Game Choices
                {"Rock", "?á"},
                {"Paper", "Gi?y"},
                {"Scissors", "Kéo"},
                {"RockEmoji", "?"},
                {"PaperEmoji", "?"},
                {"ScissorsEmoji", "??"},

                // Single Player
                {"SinglePlayerTitle", "CH?I ??N"},
                {"YourChoice", "B?n ch?n"},
                {"ComputerChoice", "Máy ch?n"},
                {"GameResult", "K?t qu?"},
                {"Reset", "??t l?i"},
                {"YouWin", "B?n th?ng!"},
                {"YouLose", "B?n thua!"},
                {"Draw", "Hòa!"},

                // Multi Player
                {"MultiPlayerTitle", "CH?I ?ÔI ONLINE"},
                {"ConnectionInfo", "Thông tin k?t n?i:"},
                {"ServerIPPlaceholder", "IP Server (vd: 192.168.1.100)"},
                {"PortPlaceholder", "Port"},
                {"RoomIDPlaceholder", "Room ID"},
                {"CreateRoom", "T?o phòng"},
                {"JoinRoom", "Vào phòng"},
                {"OpenWireshark", "M? Wireshark"},
                {"WiresharkHelp", "Wireshark Help"},
                {"WaitingForConnection", "?ang ch? ng??i ch?i k?t n?i"},
                {"Connected", "?ã k?t n?i! S?n sàng ch?i."},
                {"ConnectionLost", "M?t k?t n?i"},
                {"WaitingForOpponent", "?ang ch? ??i th?..."},
                {"ChooseYourMove", "Ch?n l?a ch?n c?a b?n"},

                // Game Rules
                {"GameRulesTitle", "LU?T CH?I ROCK PAPER SCISSORS"},
                {"GameRulesContent", @"?? CÁCH CH?I:
• ?á (?) th?ng Kéo (??)
• Kéo (??) th?ng Gi?y (?)  
• Gi?y (?) th?ng ?á (?)
• N?u cùng ch?n m?t lo?i thì hòa

?? CH?I ??N:
• Ch?i v?i máy tính
• Ch?n ?á, Gi?y ho?c Kéo
• Xem k?t qu? và t? s?

?? CH?I ONLINE:
• T?o phòng ho?c tham gia phòng
• Ch?i v?i b?n bè qua m?ng
• ??ng b? animation và fullscreen

?? WIRESHARK:
• Theo dõi network traffic
• Phân tích gói tin game
• Filter: tcp port 7777

?? PHÍM T?T:
• F11: Fullscreen
• ESC: Thoát fullscreen"},

                // Error Messages
                {"Error", "L?i"},
                {"Warning", "C?nh báo"},
                {"Information", "Thông tin"},
                {"PortInUse", "Port {0} ?ang ???c s? d?ng!"},
                {"CannotConnect", "Không th? k?t n?i ??n server {0}:{1}"},
                {"InvalidPort", "Port không h?p l?!"},
                {"EnterServerInfo", "Vui lòng nh?p ??y ?? thông tin server!"},
                {"WiresharkNotFound", "Không tìm th?y Wireshark. Vui lòng cài ??t t?i: {0}"},

                // Network Messages
                {"LocalIP", "IP Local"},
                {"Room", "Room"},
                {"PlayerJoined", "Ng??i ch?i ?ã tham gia! S?n sàng ch?i."},
                {"ServerError", "L?i server"},
                {"SendMessageError", "L?i g?i tin nh?n"},

                // Language Selection
                {"SelectLanguage", "Ch?n ngôn ng?"},
                {"LanguageChanged", "?ã thay ??i ngôn ng? thành {0}"},
                {"RestartRequired", "Kh?i ??ng l?i ?ng d?ng ?? áp d?ng hoàn toàn"}
            };

            // English
            translations["en"] = new Dictionary<string, string>
            {
                // Common
                {"AppTitle", "Rock Paper Scissors"},
                {"Player", "Player"},
                {"Computer", "Computer"},
                {"Opponent", "Opponent"},
                {"Score", "Score"},
                {"Back", "Back"},
                {"Close", "Close"},
                {"Cancel", "Cancel"},
                {"OK", "OK"},
                {"Yes", "Yes"},
                {"No", "No"},
                {"Language", "Language"},
                {"Settings", "Settings"},

                // Login Form
                {"LoginTitle", "LOGIN"},
                {"PlayerNameLabel", "Player name:"},
                {"EnterPlayerName", "Please enter player name!"},
                {"Login", "Login"},

                // Main Menu
                {"MainMenuTitle", "ROCK PAPER SCISSORS"},
                {"SinglePlayer", "Single Player"},
                {"MultiPlayer", "Multiplayer"},
                {"GameRules", "Game Rules"},
                {"Exit", "Exit"},

                // Game Choices
                {"Rock", "Rock"},
                {"Paper", "Paper"},
                {"Scissors", "Scissors"},
                {"RockEmoji", "?"},
                {"PaperEmoji", "?"},
                {"ScissorsEmoji", "??"},

                // Single Player
                {"SinglePlayerTitle", "SINGLE PLAYER"},
                {"YourChoice", "Your choice"},
                {"ComputerChoice", "Computer choice"},
                {"GameResult", "Result"},
                {"Reset", "Reset"},
                {"YouWin", "You win!"},
                {"YouLose", "You lose!"},
                {"Draw", "Draw!"},

                // Multi Player
                {"MultiPlayerTitle", "ONLINE MULTIPLAYER"},
                {"ConnectionInfo", "Connection info:"},
                {"ServerIPPlaceholder", "Server IP (e.g: 192.168.1.100)"},
                {"PortPlaceholder", "Port"},
                {"RoomIDPlaceholder", "Room ID"},
                {"CreateRoom", "Create Room"},
                {"JoinRoom", "Join Room"},
                {"OpenWireshark", "Open Wireshark"},
                {"WiresharkHelp", "Wireshark Help"},
                {"WaitingForConnection", "Waiting for player to connect"},
                {"Connected", "Connected! Ready to play."},
                {"ConnectionLost", "Connection lost"},
                {"WaitingForOpponent", "Waiting for opponent..."},
                {"ChooseYourMove", "Choose your move"},

                // Game Rules
                {"GameRulesTitle", "ROCK PAPER SCISSORS RULES"},
                {"GameRulesContent", @"?? HOW TO PLAY:
• Rock (?) beats Scissors (??)
• Scissors (??) beats Paper (?)
• Paper (?) beats Rock (?)
• Same choice = Draw

?? SINGLE PLAYER:
• Play against computer
• Choose Rock, Paper or Scissors
• View results and score

?? ONLINE PLAY:
• Create or join a room
• Play with friends online
• Synchronized animations and fullscreen

?? WIRESHARK:
• Monitor network traffic
• Analyze game packets
• Filter: tcp port 7777

?? KEYBOARD SHORTCUTS:
• F11: Fullscreen
• ESC: Exit fullscreen"},

                // Error Messages
                {"Error", "Error"},
                {"Warning", "Warning"},
                {"Information", "Information"},
                {"PortInUse", "Port {0} is already in use!"},
                {"CannotConnect", "Cannot connect to server {0}:{1}"},
                {"InvalidPort", "Invalid port!"},
                {"EnterServerInfo", "Please enter complete server information!"},
                {"WiresharkNotFound", "Wireshark not found. Please install from: {0}"},

                // Network Messages
                {"LocalIP", "Local IP"},
                {"Room", "Room"},
                {"PlayerJoined", "Player joined! Ready to play."},
                {"ServerError", "Server error"},
                {"SendMessageError", "Message send error"},

                // Language Selection
                {"SelectLanguage", "Select Language"},
                {"LanguageChanged", "Language changed to {0}"},
                {"RestartRequired", "Restart application to fully apply changes"}
            };

            // Chinese (Simplified)
            translations["zh"] = new Dictionary<string, string>
            {
                // Common
                {"AppTitle", "?????"},
                {"Player", "??"},
                {"Computer", "??"},
                {"Opponent", "??"},
                {"Score", "??"},
                {"Back", "??"},
                {"Close", "??"},
                {"Cancel", "??"},
                {"OK", "??"},
                {"Yes", "?"},
                {"No", "?"},
                {"Language", "??"},
                {"Settings", "??"},

                // Login Form
                {"LoginTitle", "??"},
                {"PlayerNameLabel", "?????"},
                {"EnterPlayerName", "????????"},
                {"Login", "??"},

                // Main Menu
                {"MainMenuTitle", "?????"},
                {"SinglePlayer", "????"},
                {"MultiPlayer", "????"},
                {"GameRules", "????"},
                {"Exit", "??"},

                // Game Choices
                {"Rock", "??"},
                {"Paper", "?"},
                {"Scissors", "??"},
                {"RockEmoji", "?"},
                {"PaperEmoji", "?"},
                {"ScissorsEmoji", "??"},

                // Single Player
                {"SinglePlayerTitle", "????"},
                {"YourChoice", "????"},
                {"ComputerChoice", "????"},
                {"GameResult", "??"},
                {"Reset", "??"},
                {"YouWin", "????"},
                {"YouLose", "????"},
                {"Draw", "???"},

                // Multi Player
                {"MultiPlayerTitle", "??????"},
                {"ConnectionInfo", "?????"},
                {"ServerIPPlaceholder", "???IP (?: 192.168.1.100)"},
                {"PortPlaceholder", "??"},
                {"RoomIDPlaceholder", "??ID"},
                {"CreateRoom", "????"},
                {"JoinRoom", "????"},
                {"OpenWireshark", "??Wireshark"},
                {"WiresharkHelp", "Wireshark??"},
                {"WaitingForConnection", "??????"},
                {"Connected", "?????????"},
                {"ConnectionLost", "????"},
                {"WaitingForOpponent", "????..."},
                {"ChooseYourMove", "??????"},

                // Game Rules
                {"GameRulesTitle", "???????"},
                {"GameRulesContent", @"?? ?????
• ?? (?) ? ?? (??)
• ?? (??) ? ? (?)
• ? (?) ? ?? (?)
• ???? = ??

?? ?????
• ?????
• ?????????
• ???????

?? ?????
• ???????
• ???????
• ???????

?? WIRESHARK?
• ??????
• ???????
• ???: tcp port 7777

?? ??????
• F11: ??
• ESC: ????"},

                // Language Selection
                {"SelectLanguage", "????"},
                {"LanguageChanged", "?????? {0}"},
                {"RestartRequired", "?????????????"}
            };

            // Japanese
            translations["ja"] = new Dictionary<string, string>
            {
                // Common
                {"AppTitle", "?????"},
                {"Player", "?????"},
                {"Computer", "???????"},
                {"Opponent", "??"},
                {"Score", "???"},
                {"Back", "??"},
                {"Close", "???"},
                {"Cancel", "?????"},
                {"OK", "OK"},
                {"Yes", "??"},
                {"No", "???"},
                {"Language", "??"},
                {"Settings", "??"},

                // Login Form
                {"LoginTitle", "????"},
                {"PlayerNameLabel", "???????"},
                {"EnterPlayerName", "????????????????"},
                {"Login", "????"},

                // Main Menu
                {"MainMenuTitle", "?????"},
                {"SinglePlayer", "?????"},
                {"MultiPlayer", "??????"},
                {"GameRules", "??????"},
                {"Exit", "??"},

                // Game Choices
                {"Rock", "??"},
                {"Paper", "??"},
                {"Scissors", "???"},
                {"RockEmoji", "?"},
                {"PaperEmoji", "?"},
                {"ScissorsEmoji", "??"},

                // Single Player
                {"SinglePlayerTitle", "?????"},
                {"YourChoice", "??????"},
                {"ComputerChoice", "??????????"},
                {"GameResult", "??"},
                {"Reset", "????"},
                {"YouWin", "???????"},
                {"YouLose", "???????"},
                {"Draw", "?????"},

                // Multi Player
                {"MultiPlayerTitle", "???????????"},
                {"ConnectionInfo", "?????"},
                {"ServerIPPlaceholder", "????IP (?: 192.168.1.100)"},
                {"PortPlaceholder", "???"},
                {"RoomIDPlaceholder", "???ID"},
                {"CreateRoom", "?????"},
                {"JoinRoom", "?????"},
                {"OpenWireshark", "Wireshark???"},
                {"WiresharkHelp", "Wireshark???"},
                {"WaitingForConnection", "????????????"},
                {"Connected", "???????????????"},
                {"ConnectionLost", "??????????"},
                {"WaitingForOpponent", "??????..."},
                {"ChooseYourMove", "?????????"},

                // Game Rules
                {"GameRulesTitle", "????????"},
                {"GameRulesContent", @"?? ????
• ?? (?) ???? (??) ???
• ??? (??) ??? (?) ???
• ?? (?) ??? (?) ???
• ??? = ????

?? ??????
• ??????????
• ????????????
• ?????????

?? ?????????
• ???????????
• ??????????
• ????????????????????

?? WIRESHARK?
• ???????????????
• ??????????
• ?????: tcp port 7777

?? ?????????????
• F11: ???????
• ESC: ?????????"},

                // Language Selection
                {"SelectLanguage", "?????"},
                {"LanguageChanged", "??? {0} ????????"},
                {"RestartRequired", "??????????????????????????????"}
            };

            // Korean
            translations["ko"] = new Dictionary<string, string>
            {
                // Common
                {"AppTitle", "?????"},
                {"Player", "????"},
                {"Computer", "???"},
                {"Opponent", "???"},
                {"Score", "??"},
                {"Back", "??"},
                {"Close", "??"},
                {"Cancel", "??"},
                {"OK", "??"},
                {"Yes", "?"},
                {"No", "???"},
                {"Language", "??"},
                {"Settings", "??"},

                // Login Form
                {"LoginTitle", "???"},
                {"PlayerNameLabel", "???? ??:"},
                {"EnterPlayerName", "???? ??? ??????!"},
                {"Login", "???"},

                // Main Menu
                {"MainMenuTitle", "?????"},
                {"SinglePlayer", "?? ???"},
                {"MultiPlayer", "?? ???"},
                {"GameRules", "?? ??"},
                {"Exit", "??"},

                // Game Choices
                {"Rock", "??"},
                {"Paper", "?"},
                {"Scissors", "??"},
                {"RockEmoji", "?"},
                {"PaperEmoji", "?"},
                {"ScissorsEmoji", "??"},

                // Single Player
                {"SinglePlayerTitle", "?? ???"},
                {"YourChoice", "??? ??"},
                {"ComputerChoice", "??? ??"},
                {"GameResult", "??"},
                {"Reset", "???"},
                {"YouWin", "??? ?????!"},
                {"YouLose", "??? ????!"},
                {"Draw", "???!"},

                // Multi Player
                {"MultiPlayerTitle", "??? ?????"},
                {"ConnectionInfo", "?? ??:"},
                {"ServerIPPlaceholder", "?? IP (?: 192.168.1.100)"},
                {"PortPlaceholder", "??"},
                {"RoomIDPlaceholder", "? ID"},
                {"CreateRoom", "? ???"},
                {"JoinRoom", "? ??"},
                {"OpenWireshark", "Wireshark ??"},
                {"WiresharkHelp", "Wireshark ???"},
                {"WaitingForConnection", "???? ?? ???"},
                {"Connected", "???! ?? ?? ??."},
                {"ConnectionLost", "??? ??????"},
                {"WaitingForOpponent", "??? ???..."},
                {"ChooseYourMove", "??? ?? ?????"},

                // Game Rules
                {"GameRulesTitle", "????? ??"},
                {"GameRulesContent", @"?? ?? ??:
• ?? (?) ? ?? (??) ? ????
• ?? (??) ? ? (?) ? ????
• ? (?) ? ?? (?) ? ????
• ?? ?? = ???

?? ?? ???:
• ???? ??
• ??, ?, ?? ? ??
• ??? ?? ??

?? ??? ???:
• ?? ???? ??
• ???? ??? ??
• ???? ?????? ????

?? WIRESHARK:
• ???? ??? ????
• ?? ?? ??
• ??: tcp port 7777

?? ??? ???:
• F11: ????
• ESC: ???? ??"},

                // Language Selection
                {"SelectLanguage", "?? ??"},
                {"LanguageChanged", "??? {0}(?)? ???????"},
                {"RestartRequired", "????? ??? ????? ??????? ??????"}
            };

            // Spanish
            translations["es"] = new Dictionary<string, string>
            {
                // Common
                {"AppTitle", "Piedra Papel Tijera"},
                {"Player", "Jugador"},
                {"Computer", "Computadora"},
                {"Opponent", "Oponente"},
                {"Score", "Puntuación"},
                {"Back", "Atrás"},
                {"Close", "Cerrar"},
                {"Cancel", "Cancelar"},
                {"OK", "Aceptar"},
                {"Yes", "Sí"},
                {"No", "No"},
                {"Language", "Idioma"},
                {"Settings", "Configuración"},

                // Login Form
                {"LoginTitle", "INICIAR SESIÓN"},
                {"PlayerNameLabel", "Nombre del jugador:"},
                {"EnterPlayerName", "¡Por favor ingrese el nombre del jugador!"},
                {"Login", "Iniciar sesión"},

                // Main Menu
                {"MainMenuTitle", "PIEDRA PAPEL TIJERA"},
                {"SinglePlayer", "Un jugador"},
                {"MultiPlayer", "Multijugador"},
                {"GameRules", "Reglas del juego"},
                {"Exit", "Salir"},

                // Game Choices
                {"Rock", "Piedra"},
                {"Paper", "Papel"},
                {"Scissors", "Tijera"},
                {"RockEmoji", "?"},
                {"PaperEmoji", "?"},
                {"ScissorsEmoji", "??"},

                // Single Player
                {"SinglePlayerTitle", "UN JUGADOR"},
                {"YourChoice", "Tu elección"},
                {"ComputerChoice", "Elección de la computadora"},
                {"GameResult", "Resultado"},
                {"Reset", "Reiniciar"},
                {"YouWin", "¡Ganaste!"},
                {"YouLose", "¡Perdiste!"},
                {"Draw", "¡Empate!"},

                // Multi Player
                {"MultiPlayerTitle", "MULTIJUGADOR EN LÍNEA"},
                {"ConnectionInfo", "Información de conexión:"},
                {"ServerIPPlaceholder", "IP del servidor (ej: 192.168.1.100)"},
                {"PortPlaceholder", "Puerto"},
                {"RoomIDPlaceholder", "ID de sala"},
                {"CreateRoom", "Crear sala"},
                {"JoinRoom", "Unirse a sala"},
                {"OpenWireshark", "Abrir Wireshark"},
                {"WiresharkHelp", "Ayuda de Wireshark"},
                {"WaitingForConnection", "Esperando conexión del jugador"},
                {"Connected", "¡Conectado! Listo para jugar."},
                {"ConnectionLost", "Conexión perdida"},
                {"WaitingForOpponent", "Esperando oponente..."},
                {"ChooseYourMove", "Elige tu jugada"},

                // Game Rules
                {"GameRulesTitle", "REGLAS DE PIEDRA PAPEL TIJERA"},
                {"GameRulesContent", @"?? CÓMO JUGAR:
• Piedra (?) vence a Tijera (??)
• Tijera (??) vence a Papel (?)
• Papel (?) vence a Piedra (?)
• Misma elección = Empate

?? UN JUGADOR:
• Jugar contra la computadora
• Elegir Piedra, Papel o Tijera
• Ver resultados y puntuación

?? JUEGO EN LÍNEA:
• Crear o unirse a una sala
• Jugar con amigos en línea
• Animaciones sincronizadas y pantalla completa

?? WIRESHARK:
• Monitorear tráfico de red
• Analizar paquetes del juego
• Filtro: tcp port 7777

?? ATAJOS DE TECLADO:
• F11: Pantalla completa
• ESC: Salir de pantalla completa"},

                // Language Selection
                {"SelectLanguage", "Seleccionar idioma"},
                {"LanguageChanged", "Idioma cambiado a {0}"},
                {"RestartRequired", "Reiniciar aplicación para aplicar completamente los cambios"}
            };

            // French
            translations["fr"] = new Dictionary<string, string>
            {
                // Common
                {"AppTitle", "Pierre Papier Ciseaux"},
                {"Player", "Joueur"},
                {"Computer", "Ordinateur"},
                {"Opponent", "Adversaire"},
                {"Score", "Score"},
                {"Back", "Retour"},
                {"Close", "Fermer"},
                {"Cancel", "Annuler"},
                {"OK", "OK"},
                {"Yes", "Oui"},
                {"No", "Non"},
                {"Language", "Langue"},
                {"Settings", "Paramètres"},

                // Login Form
                {"LoginTitle", "CONNEXION"},
                {"PlayerNameLabel", "Nom du joueur:"},
                {"EnterPlayerName", "Veuillez entrer le nom du joueur!"},
                {"Login", "Se connecter"},

                // Main Menu
                {"MainMenuTitle", "PIERRE PAPIER CISEAUX"},
                {"SinglePlayer", "Solo"},
                {"MultiPlayer", "Multijoueur"},
                {"GameRules", "Règles du jeu"},
                {"Exit", "Quitter"},

                // Game Choices
                {"Rock", "Pierre"},
                {"Paper", "Papier"},
                {"Scissors", "Ciseaux"},
                {"RockEmoji", "?"},
                {"PaperEmoji", "?"},
                {"ScissorsEmoji", "??"},

                // Single Player
                {"SinglePlayerTitle", "SOLO"},
                {"YourChoice", "Votre choix"},
                {"ComputerChoice", "Choix de l'ordinateur"},
                {"GameResult", "Résultat"},
                {"Reset", "Réinitialiser"},
                {"YouWin", "Vous gagnez!"},
                {"YouLose", "Vous perdez!"},
                {"Draw", "Égalité!"},

                // Multi Player
                {"MultiPlayerTitle", "MULTIJOUEUR EN LIGNE"},
                {"ConnectionInfo", "Informations de connexion:"},
                {"ServerIPPlaceholder", "IP du serveur (ex: 192.168.1.100)"},
                {"PortPlaceholder", "Port"},
                {"RoomIDPlaceholder", "ID de salle"},
                {"CreateRoom", "Créer une salle"},
                {"JoinRoom", "Rejoindre une salle"},
                {"OpenWireshark", "Ouvrir Wireshark"},
                {"WiresharkHelp", "Aide Wireshark"},
                {"WaitingForConnection", "En attente de connexion du joueur"},
                {"Connected", "Connecté! Prêt à jouer."},
                {"ConnectionLost", "Connexion perdue"},
                {"WaitingForOpponent", "En attente de l'adversaire..."},
                {"ChooseYourMove", "Choisissez votre coup"},

                // Game Rules
                {"GameRulesTitle", "RÈGLES DE PIERRE PAPIER CISEAUX"},
                {"GameRulesContent", @"?? COMMENT JOUER:
• Pierre (?) bat Ciseaux (??)
• Ciseaux (??) bat Papier (?)
• Papier (?) bat Pierre (?)
• Même choix = Égalité

?? SOLO:
• Jouer contre l'ordinateur
• Choisir Pierre, Papier ou Ciseaux
• Voir les résultats et le score

?? JEU EN LIGNE:
• Créer ou rejoindre une salle
• Jouer avec des amis en ligne
• Animations synchronisées et plein écran

?? WIRESHARK:
• Surveiller le trafic réseau
• Analyser les paquets du jeu
• Filtre: tcp port 7777

?? RACCOURCIS CLAVIER:
• F11: Plein écran
• ESC: Quitter le plein écran"},

                // Language Selection
                {"SelectLanguage", "Sélectionner la langue"},
                {"LanguageChanged", "Langue changée en {0}"},
                {"RestartRequired", "Redémarrer l'application pour appliquer complètement les changements"}
            };

            // German  
            translations["de"] = new Dictionary<string, string>
            {
                // Common
                {"AppTitle", "Schere Stein Papier"},
                {"Player", "Spieler"},
                {"Computer", "Computer"},
                {"Opponent", "Gegner"},
                {"Score", "Punktzahl"},
                {"Back", "Zurück"},
                {"Close", "Schließen"},
                {"Cancel", "Abbrechen"},
                {"OK", "OK"},
                {"Yes", "Ja"},
                {"No", "Nein"},
                {"Language", "Sprache"},
                {"Settings", "Einstellungen"},

                // Login Form
                {"LoginTitle", "ANMELDUNG"},
                {"PlayerNameLabel", "Spielername:"},
                {"EnterPlayerName", "Bitte Spielername eingeben!"},
                {"Login", "Anmelden"},

                // Main Menu
                {"MainMenuTitle", "SCHERE STEIN PAPIER"},
                {"SinglePlayer", "Einzelspieler"},
                {"MultiPlayer", "Mehrspieler"},
                {"GameRules", "Spielregeln"},
                {"Exit", "Beenden"},

                // Game Choices
                {"Rock", "Stein"},
                {"Paper", "Papier"},
                {"Scissors", "Schere"},
                {"RockEmoji", "?"},
                {"PaperEmoji", "?"},
                {"ScissorsEmoji", "??"},

                // Single Player
                {"SinglePlayerTitle", "EINZELSPIELER"},
                {"YourChoice", "Ihre Wahl"},
                {"ComputerChoice", "Computer-Wahl"},
                {"GameResult", "Ergebnis"},
                {"Reset", "Zurücksetzen"},
                {"YouWin", "Sie gewinnen!"},
                {"YouLose", "Sie verlieren!"},
                {"Draw", "Unentschieden!"},

                // Multi Player
                {"MultiPlayerTitle", "ONLINE-MEHRSPIELER"},
                {"ConnectionInfo", "Verbindungsinfo:"},
                {"ServerIPPlaceholder", "Server-IP (z.B: 192.168.1.100)"},
                {"PortPlaceholder", "Port"},
                {"RoomIDPlaceholder", "Raum-ID"},
                {"CreateRoom", "Raum erstellen"},
                {"JoinRoom", "Raum beitreten"},
                {"OpenWireshark", "Wireshark öffnen"},
                {"WiresharkHelp", "Wireshark-Hilfe"},
                {"WaitingForConnection", "Warten auf Spielerverbindung"},
                {"Connected", "Verbunden! Bereit zum Spielen."},
                {"ConnectionLost", "Verbindung verloren"},
                {"WaitingForOpponent", "Warten auf Gegner..."},
                {"ChooseYourMove", "Wählen Sie Ihren Zug"},

                // Game Rules
                {"GameRulesTitle", "SCHERE STEIN PAPIER REGELN"},
                {"GameRulesContent", @"?? SO WIRD GESPIELT:
• Stein (?) schlägt Schere (??)
• Schere (??) schlägt Papier (?)
• Papier (?) schlägt Stein (?)
• Gleiche Wahl = Unentschieden

?? EINZELSPIELER:
• Gegen Computer spielen
• Stein, Papier oder Schere wählen
• Ergebnisse und Punktzahl anzeigen

?? ONLINE-SPIEL:
• Raum erstellen oder beitreten
• Mit Freunden online spielen
• Synchronisierte Animationen und Vollbild

?? WIRESHARK:
• Netzwerkverkehr überwachen
• Spielpakete analysieren
• Filter: tcp port 7777

?? TASTATURKÜRZEL:
• F11: Vollbild
• ESC: Vollbild verlassen"},

                // Language Selection
                {"SelectLanguage", "Sprache auswählen"},
                {"LanguageChanged", "Sprache geändert zu {0}"},
                {"RestartRequired", "Anwendung neu starten, um Änderungen vollständig zu übernehmen"}
            };

            // Portuguese
            translations["pt"] = new Dictionary<string, string>
            {
                // Common
                {"AppTitle", "Pedra Papel Tesoura"},
                {"Player", "Jogador"},
                {"Computer", "Computador"},
                {"Opponent", "Oponente"},
                {"Score", "Pontuação"},
                {"Back", "Voltar"},
                {"Close", "Fechar"},
                {"Cancel", "Cancelar"},
                {"OK", "OK"},
                {"Yes", "Sim"},
                {"No", "Não"},
                {"Language", "Idioma"},
                {"Settings", "Configurações"},

                // Login Form
                {"LoginTitle", "LOGIN"},
                {"PlayerNameLabel", "Nome do jogador:"},
                {"EnterPlayerName", "Por favor, insira o nome do jogador!"},
                {"Login", "Entrar"},

                // Main Menu
                {"MainMenuTitle", "PEDRA PAPEL TESOURA"},
                {"SinglePlayer", "Um jogador"},
                {"MultiPlayer", "Multijogador"},
                {"GameRules", "Regras do jogo"},
                {"Exit", "Sair"},

                // Game Choices
                {"Rock", "Pedra"},
                {"Paper", "Papel"},
                {"Scissors", "Tesoura"},
                {"RockEmoji", "?"},
                {"PaperEmoji", "?"},
                {"ScissorsEmoji", "??"},

                // Single Player
                {"SinglePlayerTitle", "UM JOGADOR"},
                {"YourChoice", "Sua escolha"},
                {"ComputerChoice", "Escolha do computador"},
                {"GameResult", "Resultado"},
                {"Reset", "Reiniciar"},
                {"YouWin", "Você ganhou!"},
                {"YouLose", "Você perdeu!"},
                {"Draw", "Empate!"},

                // Multi Player
                {"MultiPlayerTitle", "MULTIJOGADOR ONLINE"},
                {"ConnectionInfo", "Informações de conexão:"},
                {"ServerIPPlaceholder", "IP do servidor (ex: 192.168.1.100)"},
                {"PortPlaceholder", "Porta"},
                {"RoomIDPlaceholder", "ID da sala"},
                {"CreateRoom", "Criar sala"},
                {"JoinRoom", "Entrar na sala"},
                {"OpenWireshark", "Abrir Wireshark"},
                {"WiresharkHelp", "Ajuda do Wireshark"},
                {"WaitingForConnection", "Aguardando conexão do jogador"},
                {"Connected", "Conectado! Pronto para jogar."},
                {"ConnectionLost", "Conexão perdida"},
                {"WaitingForOpponent", "Aguardando oponente..."},
                {"ChooseYourMove", "Escolha sua jogada"},

                // Game Rules
                {"GameRulesTitle", "REGRAS DE PEDRA PAPEL TESOURA"},
                {"GameRulesContent", @"?? COMO JOGAR:
• Pedra (?) vence Tesoura (??)
• Tesoura (??) vence Papel (?)
• Papel (?) vence Pedra (?)
• Mesma escolha = Empate

?? UM JOGADOR:
• Jogar contra o computador
• Escolher Pedra, Papel ou Tesoura
• Ver resultados e pontuação

?? JOGO ONLINE:
• Criar ou entrar em uma sala
• Jogar com amigos online
• Animações sincronizadas e tela cheia

?? WIRESHARK:
• Monitorar tráfego de rede
• Analisar pacotes do jogo
• Filtro: tcp port 7777

?? ATALHOS DO TECLADO:
• F11: Tela cheia
• ESC: Sair da tela cheia"},

                // Language Selection
                {"SelectLanguage", "Selecionar idioma"},
                {"LanguageChanged", "Idioma alterado para {0}"},
                {"RestartRequired", "Reiniciar aplicação para aplicar completamente as mudanças"}
            };

            // Russian
            translations["ru"] = new Dictionary<string, string>
            {
                // Common
                {"AppTitle", "?????? ??????? ??????"},
                {"Player", "?????"},
                {"Computer", "?????????"},
                {"Opponent", "?????????"},
                {"Score", "????"},
                {"Back", "?????"},
                {"Close", "???????"},
                {"Cancel", "??????"},
                {"OK", "??"},
                {"Yes", "??"},
                {"No", "???"},
                {"Language", "????"},
                {"Settings", "?????????"},

                // Login Form
                {"LoginTitle", "????"},
                {"PlayerNameLabel", "??? ??????:"},
                {"EnterPlayerName", "??????????, ??????? ??? ??????!"},
                {"Login", "?????"},

                // Main Menu
                {"MainMenuTitle", "?????? ??????? ??????"},
                {"SinglePlayer", "????????? ????"},
                {"MultiPlayer", "???????????"},
                {"GameRules", "??????? ????"},
                {"Exit", "?????"},

                // Game Choices
                {"Rock", "??????"},
                {"Paper", "??????"},
                {"Scissors", "???????"},
                {"RockEmoji", "?"},
                {"PaperEmoji", "?"},
                {"ScissorsEmoji", "??"},

                // Single Player
                {"SinglePlayerTitle", "????????? ????"},
                {"YourChoice", "??? ?????"},
                {"ComputerChoice", "????? ??????????"},
                {"GameResult", "?????????"},
                {"Reset", "?????"},
                {"YouWin", "?? ????????!"},
                {"YouLose", "?? ?????????!"},
                {"Draw", "?????!"},

                // Multi Player
                {"MultiPlayerTitle", "??????????? ??????"},
                {"ConnectionInfo", "?????????? ? ??????????:"},
                {"ServerIPPlaceholder", "IP ??????? (????????: 192.168.1.100)"},
                {"PortPlaceholder", "????"},
                {"RoomIDPlaceholder", "ID ???????"},
                {"CreateRoom", "??????? ???????"},
                {"JoinRoom", "?????????????? ? ???????"},
                {"OpenWireshark", "??????? Wireshark"},
                {"WiresharkHelp", "??????? Wireshark"},
                {"WaitingForConnection", "???????? ??????????? ??????"},
                {"Connected", "??????????! ????? ? ????."},
                {"ConnectionLost", "?????????? ????????"},
                {"WaitingForOpponent", "???????? ??????????..."},
                {"ChooseYourMove", "???????? ??? ???"},

                // Game Rules
                {"GameRulesTitle", "??????? ?????? ??????? ??????"},
                {"GameRulesContent", @"?? ??? ??????:
• ?????? (?) ????????? ??????? (??)
• ??????? (??) ????????? ?????? (?)
• ?????? (?) ????????? ?????? (?)
• ?????????? ????? = ?????

?? ????????? ????:
• ?????? ?????? ??????????
• ??????? ??????, ?????? ??? ???????
• ??????????? ?????????? ? ????

?? ?????? ????:
• ??????? ??? ?????????????? ? ???????
• ?????? ? ???????? ??????
• ?????????????????? ???????? ? ????????????? ?????

?? WIRESHARK:
• ?????????? ???????? ???????
• ?????? ??????? ???????
• ??????: tcp port 7777

?? ??????? ???????:
• F11: ????????????? ?????
• ESC: ????? ?? ?????????????? ??????"},

                // Language Selection
                {"SelectLanguage", "??????? ????"},
                {"LanguageChanged", "???? ??????? ?? {0}"},
                {"RestartRequired", "????????????? ?????????? ??? ??????? ?????????? ?????????"}
            };
        }

        /// <summary>
        /// Save language settings to file
        /// </summary>
        private static void SaveLanguageSettings()
        {
            try
            {
                var settings = new { Language = CurrentLanguage };
                string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(settingsFile, json);
            }
            catch (Exception ex)
            {
                // Silent fail for settings save
                System.Diagnostics.Debug.WriteLine($"Failed to save language settings: {ex.Message}");
            }
        }

        /// <summary>
        /// Load language settings from file
        /// </summary>
        private static void LoadLanguageSettings()
        {
            try
            {
                if (File.Exists(settingsFile))
                {
                    string json = File.ReadAllText(settingsFile);
                    var settings = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
                    
                    if (settings != null && settings.ContainsKey("Language"))
                    {
                        string savedLanguage = settings["Language"].ToString() ?? "vi";
                        if (SupportedLanguages.ContainsKey(savedLanguage))
                        {
                            currentLanguage = savedLanguage;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Silent fail for settings load, use default
                System.Diagnostics.Debug.WriteLine($"Failed to load language settings: {ex.Message}");
            }
        }

        /// <summary>
        /// Get available languages for display in UI
        /// </summary>
        public static List<LanguageDisplayItem> GetLanguageList()
        {
            var languages = new List<LanguageDisplayItem>();
            
            foreach (var lang in SupportedLanguages.Values)
            {
                languages.Add(new LanguageDisplayItem
                {
                    Code = lang.Code,
                    DisplayText = $"{lang.Flag} {lang.NativeName} ({lang.EnglishName})",
                    NativeName = lang.NativeName,
                    EnglishName = lang.EnglishName,
                    Flag = lang.Flag
                });
            }
            
            return languages;
        }

        /// <summary>
        /// Check if a translation key exists
        /// </summary>
        public static bool HasTranslation(string key)
        {
            return translations.ContainsKey(CurrentLanguage) && 
                   translations[CurrentLanguage].ContainsKey(key);
        }

        /// <summary>
        /// Get formatted text with parameters
        /// </summary>
        public static string GetFormattedText(string key, params object[] args)
        {
            string text = GetText(key);
            try
            {
                return string.Format(text, args);
            }
            catch
            {
                return text;
            }
        }
    }

    /// <summary>
    /// Language information structure
    /// </summary>
    public class LanguageInfo
    {
        public string Code { get; set; }
        public string NativeName { get; set; }
        public string EnglishName { get; set; }
        public string Flag { get; set; }

        public LanguageInfo(string code, string nativeName, string englishName, string flag)
        {
            Code = code;
            NativeName = nativeName;
            EnglishName = englishName;
            Flag = flag;
        }
    }

    /// <summary>
    /// Language display item for UI controls
    /// </summary>
    public class LanguageDisplayItem
    {
        public string Code { get; set; } = "";
        public string DisplayText { get; set; } = "";
        public string NativeName { get; set; } = "";
        public string EnglishName { get; set; } = "";
        public string Flag { get; set; } = "";

        public override string ToString() => DisplayText;
    }
}