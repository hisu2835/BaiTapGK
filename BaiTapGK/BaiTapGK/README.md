# Game Oan Tu Ti (Rock Paper Scissors) - C# WinForms

## ?? Gioi thieu
Day la tro choi Oan Tu Ti (Rock Paper Scissors) duoc phat trien bang C# WinForms voi day du cac tinh nang:

### ? Tinh nang chinh:
- **Dang nhap**: He thong dang nhap voi ten nguoi choi
- **Choi don**: Choi voi may tinh (AI ngau nhien)
- **Choi doi**: Choi online voi nguoi khac qua TCP/IP
- **Gioi thieu game**: Huong dan cach choi va luat game

## ?? Cach chay ung dung

### Yeu cau he thong:
- .NET 9 SDK
- Windows OS
- Visual Studio 2022 hoac VS Code

### Chay ung dung:
1. Mo Terminal/Command Prompt
2. Navigate den thu muc du an
3. Chay lenh: `dotnet run`

## ?? Huong dan su dung

### 1. Dang nhap
- Khi khoi dong app, form dang nhap se xuat hien
- Nhap ten nguoi choi
- Click "DANG NHAP" hoac nhan Enter

### 2. Menu chinh
Sau khi dang nhap thanh cong, ban co 3 lua chon:

#### ?? Choi don (Single Player)
- Choi voi may tinh
- May se chon ngau nhien
- Diem so duoc tinh tu dong
- Co nut Reset de choi lai

#### ?? Choi doi (Multi Player)
- **Tao phong**: Click "TAO PHONG" de lam host
- **Tham gia phong**: Nhap Room ID va click "VAO PHONG"
- Can 2 nguoi choi de bat dau
- Game se tu dong so sanh ket qua

#### ?? Gioi thieu game
- Xem luat choi va cach thuc hoat dong
- Huong dan chi tiet ve game

## ?? Cau truc ma nguon

### Cac file chinh:
- `Form1.cs` - Form menu chinh
- `LoginForm.cs` - Form dang nhap
- `SinglePlayerForm.cs` - Form choi don
- `MultiPlayerForm.cs` - Form choi doi (TCP networking)
- `GameIntroForm.cs` - Form gioi thieu
- `GameConfig.cs` - Cac cau hinh game
- `NetworkUtils.cs` - Tien ich network

### Cong nghe su dung:
- **Frontend**: Windows Forms
- **Networking**: TCP/IP Socket
- **Threading**: Multi-threading cho server/client
- **Random**: Algorithm ngau nhien cho AI

## ?? Che do Multiplayer

### Cach hoat dong:
1. **Host** tao phong ? Server TCP lang nghe port 7777
2. **Client** tham gia ? Ket noi den localhost:7777
3. Trao doi du lieu qua TCP stream
4. Dong bo lua chon va tinh diem

### Giao thuc tin nhan:
- `PLAYER:[ten]` - Gui ten nguoi choi
- `CHOICE:[lua chon]` - Gui lua chon (Da/Giay/Keo)
- `RESULT:[ket qua]` - Ket qua tran dau

## ?? Giao dien

### Mau sac:
- **Da**: Xam nhat (Rock - Gray)
- **Giay**: Xanh nhat (Paper - Light Blue)  
- **Keo**: Vang nhat (Scissors - Light Yellow)

### Bieu tuong:
- ? Da (Rock)
- ? Giay (Paper)
- ?? Keo (Scissors)

## ?? Xu ly loi

### Cac loi thuong gap:
1. **Loi ket noi**: Kiem tra firewall va port 7777
2. **Mat ket noi**: Tu dong hien thi thong bao loi
3. **Thread exception**: Su dung Invoke de update UI thread-safe

## ?? Tinh nang mo rong

### Co the phat trien them:
- [ ] Chat trong game
- [ ] Luu lich su tran dau  
- [ ] Ranking system
- [ ] Sound effects
- [ ] Themes va skins
- [ ] Tournament mode
- [ ] Server dedicated (khong chi P2P)

## ?? Luat choi

### Nguyen tac co ban:
- **Da** thang **Keo** (da nghien nat keo)
- **Keo** thang **Giay** (keo cat giay)  
- **Giay** thang **Da** (giay bao da)
- Chon giong nhau = **HOA**

---
*Phat trien boi: BaiTapGK Team*
*Framework: .NET 9 + WinForms*
*Ngon ngu: Tieng Viet khong dau*