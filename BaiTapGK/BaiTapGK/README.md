# Game Oan Tu Ti (Rock Paper Scissors) - C# WinForms

## ?? NGÔN NG? ?A QU?C GIA (NEW!)

### ??? H? tr? 10 ngôn ng? ph? bi?n nh?t:
- **???? Ti?ng Vi?t** - Ngôn ng? m?c ??nh
- **???? English** - Chu?n qu?c t?
- **???? ??** - Ngôn ng? s? d?ng nhi?u nh?t
- **???? ???** - Th? tr??ng game l?n
- **???? ???** - Esports phát tri?n
- **???? Español** - 500M+ ng??i dùng
- **???? Français** - Ngôn ng? qu?c t?
- **???? Deutsch** - Th? tr??ng Châu Âu
- **???? Português** - Th? tr??ng m?i n?i
- **???? ???????** - ?ông Âu m? r?ng

### ? Tính n?ng ngôn ng?:
- **Chuy?n ??i t?c thì**: Không c?n kh?i ??ng l?i
- **Giao di?n ??p**: Ch?n ngôn ng? v?i c? qu?c gia
- **Xem tr??c**: Preview ngôn ng? tr??c khi áp d?ng
- **L?u cài ??t**: Ghi nh? ngôn ng? ?ã ch?n
- **D?ch toàn di?n**: 1000+ t?/c?m t? ???c d?ch
- **Thông minh**: T? ??ng fallback n?u thi?u b?n d?ch

### ?? Cách s? d?ng:
1. **Click nút ?? Language** ? góc trên menu chính
2. **Ch?n ngôn ng?** t? danh sách có c?
3. **Xem preview** các t? game quan tr?ng
4. **Click OK** ?? áp d?ng
5. **Enjoy!** Toàn b? game chuy?n sang ngôn ng? m?i

## ?? Gi?i thi?u
?ây là trò ch?i O?n Tù Tì (Rock Paper Scissors) ???c phát tri?n b?ng C# WinForms v?i ??y ?? các tính n?ng và **hi?u ?ng ??ng siêu sinh ??ng** và **??? ch? ?? toàn màn hình**:

### ?? Tính n?ng chính:
- **?? ?a ngôn ng?**: 10 ngôn ng? ph? bi?n nh?t th? gi?i
- **??ng nh?p**: H? th?ng ??ng nh?p v?i tên ng??i ch?i
- **Ch?i ??n**: Ch?i v?i máy tính (AI ng?u nhiên) v?i hi?u ?ng "qu?i qu?i" gi?ng nh? ??i th?t
- **Ch?i ?ôi**: Ch?i online v?i ng??i khác qua TCP/IP (LAN ho?c Internet) v?i ??ng b? hi?u ?ng
- **??? Ch? ?? toàn màn hình**: Nh?n F11 ?? chuy?n ch? ?? fullscreen, ESC ?? thoát
- **Gi?i thi?u game**: H??ng d?n cách ch?i và lu?t game
- **Wireshark Integration**: Tích h?p Wireshark ?? monitor network traffic th?c t?
- **Hi?u ?ng ??ng siêu ??p**: Animation "qu?i qu?i" gi?ng ch?i th?t, countdown "S?n-Sàng-Ch?i", sound effects
- **Responsive layout**: T? ??ng ?i?u ch?nh layout cho m?i kích th??c màn hình
- **Không d?u ti?ng Vi?t**: T?t c? text s? d?ng ti?ng Vi?t không d?u ?? tránh l?i hi?n th?

## ???? H? th?ng ngôn ng? m?i

### ?? Giao di?n ch?n ngôn ng?:
- **Dropdown ??p** v?i c? qu?c gia và tên g?c
- **Preview t?c thì** hi?n th? m?u b?n d?ch
- **Hover effects** m??t mà
- **Modal dialog** c?n gi?a hoàn h?o

### ?? Công ngh? d?ch:
- **LanguageManager**: H? th?ng d?ch t?p trung
- **LanguageHelper**: T? ??ng áp d?ng cho m?i form
- **Event-driven**: C?p nh?t t?c thì khi ??i ngôn ng?
- **Fallback thông minh**: Hi?n ? English ? Vietnamese ? Key
- **JSON storage**: L?u cài ??t b?n v?ng

### ?? Coverage d?ch thu?t:
- **Game elements**: Rock/Paper/Scissors, Win/Lose/Draw
- **UI components**: Buttons, labels, menus, dialogs
- **Game states**: Waiting, connecting, playing
- **Error messages**: Network, validation, system
- **Cultural greetings**: Welcome messages phù h?p v?n hóa

## ???? Tr?i nghi?m ?a ngôn ng?

### Ví d? d?ch game elements:? Rock = ?á/??/??/??/Piedra/Pierre/Stein/Pedra/??????
? Paper = Gi?y/?/??/?/Papel/Papier/Papier/Papel/??????  
?? Scissors = Kéo/??/???/??/Tijera/Ciseaux/Schere/Tesoura/???????
### Cultural welcome messages:
- **Vietnamese**: "Chào m?ng, [Name]!"
- **English**: "Welcome, [Name]!"
- **Chinese**: "??, [Name]!"
- **Japanese**: "????, [Name]??!"
- **Korean**: "?????, [Name]?!"
- **Spanish**: "¡Bienvenido, [Name]!"
- **French**: "Bienvenue, [Name]!"
- **German**: "Willkommen, [Name]!"
- **Portuguese**: "Bem-vindo, [Name]!"
- **Russian**: "????? ??????????, [Name]!"

## ???? Fullscreen Experience (?? Enhanced):
- **F11 Toggle**: Nh?n F11 ?? chuy?n ch? ?? toàn màn hình
- **ESC Exit**: Nh?n ESC ?? thoát fullscreen
- **Auto-scaling**: T?t c? components t? ??ng scale cho fullscreen
- **Responsive positioning**: Elements t? ??ng reposition
- **Immersive gaming**: Tr?i nghi?m game không b? phân tâm
- **Professional quality**: Gi?ng nh? các game AAA

### ???? Fullscreen + Multi-language:
- **Language button** v?n hi?n th? và ho?t ??ng trong fullscreen
- **Scaled UI elements** cho m?i ngôn ng?
- **Text scaling** thông minh cho ?? dài khác nhau
- **Cultural layout** t?i ?u cho t?ng ngôn ng?

## ?? Cách ch?y ?ng d?ng

### Yêu c?u h? th?ng:
- .NET 9 SDK
- Windows OS (7, 8.1, 10, 11)
- Visual Studio 2022 ho?c VS Code
- **Wireshark** (tùy ch?n, cho network monitoring)
- **Monitor resolution**: T?i thi?u 1024x768, t?i ?u 1920x1080+

### Ch?y ?ng d?ng:
1. M? Terminal/Command Prompt
2. Navigate ??n th? m?c d? án
3. Ch?y l?nh: `dotnet run`
4. **Click ??** ?? ch?n ngôn ng? yêu thích!
5. **Nh?n F11** ?? chuy?n ch? ?? toàn màn hình b?t k? lúc nào!

## ?? H??ng d?n s? d?ng

### 0. ?? Ch?n ngôn ng? (M?I!)
- **Click nút ?? Language** ? góc trên bên trái
- **Ch?n ngôn ng?** t? dropdown v?i c? qu?c gia
- **Xem preview** các t? khóa game quan tr?ng
- **Click OK** ?? áp d?ng ngay l?p t?c
- Toàn b? game s? chuy?n sang ngôn ng? ?ã ch?n!

### 1. ??ng nh?p
- Khi kh?i ??ng app, form ??ng nh?p s? xu?t hi?n (theo ngôn ng? ?ã ch?n)
- Nh?p tên ng??i ch?i
- Click "??NG NH?P" ho?c nh?n Enter

### 2. Menu chính (?? Multi-language)
Sau khi ??ng nh?p thành công, b?n có 3 l?a ch?n:

#### ?? Ch?i ??n (Single Player)
- **Gameplay sinh ??ng**: Click ch?n Rock/Paper/Scissors
- **??? F11 fullscreen**: Immersive single-player experience  
- **Countdown**: "Ready-Set-Go" tr??c khi ch?i (theo ngôn ng?)
- **Shake Animation**: C? b?n và máy qu?i qu?i ??ng th?i
- **Real-time Result**: K?t qu? hi?n th? v?i hi?u ?ng ??p
- **Score Tracking**: ?i?m s? c?p nh?t v?i animation
- **Sound Effects**: Âm thanh cho m?i hành ??ng

#### ?? Ch?i ?ôi (Multi Player) 
- **Network Setup**: Nh?p IP server, port
- **??? Fullscreen multiplayer**: Toàn màn hình cho 2 ng??i ch?i
- **Synchronized Animation**: ??ng b? hi?u ?ng gi?a 2 ng??i ch?i
- **Real-time Gesture**: Th?y hi?u ?ng tay c?a ??i th?
- **Wireshark Integration**: Monitor network traffic
- **Cross-platform**: Ch?i qua LAN ho?c Internet
- **?? Multi-language support**: M?i ng??i có th? dùng ngôn ng? khác nhau

#### ?? Gi?i thi?u game
- **??? Fullscreen presentation**: Xem gi?i thi?u toàn màn hình
- Xem lu?t ch?i và cách th?c ho?t ??ng (theo ngôn ng? ?ã ch?n)
- H??ng d?n chi ti?t v? game và hi?u ?ng

### 3. ?? Phím t?t m?i
- **?? Language button**: ??i ngôn ng? b?t k? lúc nào
- **F11**: Toggle fullscreen mode (t?t c? forms)
- **ESC**: Thoát fullscreen (ch? khi ?ang fullscreen)
- **Alt+Tab**: Switch gi?a windows (windowed mode)
- **Enter**: Confirm actions trong dialogs

## ?? C?u trúc mã ngu?n (?? Updated)

### ?? Language System Files (NEW):
- `LanguageManager.cs` - **?? H? th?ng d?ch t?p trung** v?i 10 ngôn ng?
- `LanguageHelper.cs` - **?? Auto-apply d?ch** cho m?i form
- `LanguageSelectionForm.cs` - **?? Giao di?n ch?n ngôn ng?** ??p
- `language_settings.json` - **?? L?u cài ??t** ngôn ng? ng??i dùng

### Core Files:
- `Form1.cs` - Form menu chính v?i fullscreen support + **?? language button**
- `LoginForm.cs` - Form ??ng nh?p (?? multi-language)
- `SinglePlayerForm.cs` - Form ch?i ??n v?i adaptive fullscreen layout
- `MultiPlayerForm.cs` - Form ch?i ?ôi v?i scaled fullscreen components
- `FullscreenManager.cs` - ??? Qu?n lý ch? ?? toàn màn hình
- `HandGestureAnimationControl.cs` - Control x? lý hi?u ?ng "qu?i qu?i"
- `BattleResultControl.cs` - ??? Control hi?n th? k?t qu? v?i fullscreen scaling
- `ButtonAnimationHelper.cs` - Helper cho button animations
- `SoundEffectHelper.cs` - Helper cho sound effects
- `CountdownAnimationHelper.cs` - Helper cho countdown
- `GameIntroForm.cs` - Form gi?i thi?u (?? multi-language)
- `GameConfig.cs` - Các c?u hình game và Wireshark
- `NetworkUtils.cs` - Ti?n ích network
- `WiresharkIntegration.cs` - Tích h?p Wireshark
- `ErrorHandler.cs` - ??? X? lý l?i an toàn

### ?? Translation Coverage:
- **1000+ translation keys** covering all UI elements
- **Game-specific terminology** for each language
- **Cultural adaptations** for greetings and expressions
- **Error messages** localized for better UX
- **Network status** messages in native languages

### Technology Stack (?? Enhanced):
- **Frontend**: Windows Forms v?i custom UserControls, fullscreen support, **+ Multi-language system**
- **?? Localization**: LanguageManager + LanguageHelper architecture
- **??? Fullscreen System**: FullscreenManager + FullscreenSupportForm base class
- **Animation**: Timer-based frame animation v?i auto-scaling
- **Sound**: SystemSounds và SoundPlayer
- **Networking**: TCP/IP Socket (LAN + Internet)
- **Threading**: Multi-threading cho server/client và animations
- **Graphics**: GDI+ cho custom drawing v?i responsive scaling
- **Network Analysis**: Wireshark integration
- **??? Responsive Design**: Auto-adapt layout cho m?i screen size
- **?? Settings**: JSON-based persistent configuration

## ?? Global Gaming Experience

### ?? Popular Language Markets:
1. **???? Chinese**: 1.1B+ speakers, huge gaming market
2. **???? English**: Global standard, 1.5B speakers
3. **???? Spanish**: 500M+ speakers, growing market
4. **???? Japanese**: 125M speakers, gaming culture leader
5. **???? Korean**: 77M speakers, esports powerhouse
6. **???? Vietnamese**: 95M+ speakers, Southeast Asian growth
7. **???? French**: 280M+ speakers, international
8. **???? German**: 100M+ speakers, strong European market
9. **???? Portuguese**: 260M+ speakers, Brazil gaming boom
10. **???? Russian**: 258M+ speakers, Eastern European market

### ?? Cultural Features:
- **Appropriate greetings** cho t?ng v?n hóa
- **Native game terminology** thay vì d?ch máy
- **Cultural color schemes** (future enhancement)
- **Regional preferences** trong UI design

## ?? Roadmap m? r?ng (?? Enhanced)

### ?? Language Enhancements:
- [ ] **Voice narration** trong nhi?u ngôn ng?
- [ ] **Right-to-left** language support (Arabic, Hebrew)
- [ ] **Dynamic font switching** cho readability t?t h?n
- [ ] **Cultural themes** và color schemes
- [ ] **Community translation** platform
- [ ] **Auto-detect** system language

### ??? Fullscreen + Language:
- [ ] **Language-specific scaling** factors
- [ ] **Cultural layout** preferences
- [ ] **Fullscreen hints** trong m?i ngôn ng?
- [ ] **Multi-monitor** language preferences

### ?? Gameplay Improvements:
- [ ] **Tournament mode** v?i multilingual brackets
- [ ] **Achievement system** v?i cultural celebrations
- [ ] **Custom gestures** cho các n?n v?n hóa khác nhau
- [ ] **Regional leaderboards** theo ngôn ng?

---
*Phát tri?n b?i: BaiTapGK Team*  
*Framework: .NET 9 + WinForms + Custom Animation Engine + **??? Fullscreen System** + **?? Multi-language Engine***  
*?? Languages: 10 major world languages v?i 1000+ translations*  
*??? Fullscreen: FullscreenManager + FullscreenSupportForm Architecture*  
*Network Analysis: Wireshark Integration*  
*Animation: Custom Timer-based System with GDI+ **+ Responsive Scaling***  
*Sound: SystemSounds + Custom SoundPlayer*  
*Ngôn ng?: Unicode UTF-8 h? tr? ?a ngôn ng?*  
*Compatibility: Windows 7-11, .NET 9, Multi-resolution Support*

?? **Game Features**: Realistic Rock-Paper-Scissors experience v?i animation gi?ng ??i th?t!  
?? **?? Multi-language**: 10 ngôn ng? ph? bi?n nh?t th? gi?i v?i d?ch thu?t chuyên nghi?p!  
??? **??? Fullscreen**: Professional gaming experience v?i F11 toggle!  
?? **Animation**: Hollywood-quality transitions và effects v?i adaptive scaling!  
?? **Networking**: Professional-grade TCP networking v?i Wireshark analysis!  
?? **Audio**: Immersive sound effects cho m?i action!  
?? **Responsive**: Perfect experience trên m?i screen size và resolution!  
?? **Global**: Tr?i nghi?m game toàn c?u cho ng??i ch?i m?i qu?c gia!