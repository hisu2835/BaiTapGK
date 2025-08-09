# H??ng d?n s? d?ng Wireshark v?i Game Oan Tu Ti

## ?? T?ng quan
Game Oan Tu Ti ?ã ???c tích h?p v?i Wireshark ?? b?n có th? monitor và phân tích network traffic real-time khi ch?i multiplayer.

## ?? Cài ??t Wireshark

### B??c 1: T?i Wireshark
```
1. Truy c?p: https://www.wireshark.org/
2. Click "Download" 
3. Ch?n Windows x64 Installer
4. T?i v? file .exe
```

### B??c 2: Cài ??t
```
1. Ch?y file installer as Administrator
2. Follow wizard (m?c ??nh là OK)
3. Ch?n cài ??t Npcap khi ???c h?i
4. Restart computer sau khi cài ??t
```

## ?? S? d?ng v?i Game

### Cách 1: Auto-launch t? Game
```
1. M? game ? Multiplayer
2. Nh?p thông tin server (IP, Port)
3. Click "MO WIRESHARK"
4. Wireshark s? t? ??ng m? v?i filter phù h?p
5. B?t ??u ch?i ?? th?y traffic!
```

### Cách 2: Manual setup
```
1. M? Wireshark ??c l?p
2. Ch?n network interface (Ethernet/WiFi)
3. Áp d?ng filter: tcp port 7777
4. Click Start (shark fin icon)
5. Ch?i game trong tab khác
```

## ?? Filters h?u ích

### Basic Filters
```sh
# T?t c? traffic game
tcp port 7777

# Ch? packets có data
tcp port 7777 and tcp.len > 0

# Ch? traffic t?/??n IP c? th?
tcp port 7777 and ip.addr == 192.168.1.100
```

### Game Protocol Filters
```sh
# Player join messages
tcp port 7777 and tcp contains "PLAYER"

# Game moves
tcp port 7777 and tcp contains "CHOICE"

# Game results  
tcp port 7777 and tcp contains "RESULT"
```

### Advanced Filters
```sh
# Ch? stream ??u tiên
tcp.stream eq 0

# TCP handshake packets
tcp.flags.syn == 1

# Connection teardown
tcp.flags.fin == 1

# Specific TCP stream
tcp.stream eq 0 and tcp port 7777
```

## ?? Phân tích Traffic

### Game Protocol Structure
```
1. TCP Handshake:
   SYN ? SYN-ACK ? ACK

2. Player Join:
   PLAYER:[tên ng??i ch?i]

3. Game Loop:
   CHOICE:[Da/Giay/Keo] ? CHOICE:[Da/Giay/Keo]
   
4. Result:
   RESULT:[k?t qu?]

5. Connection Close:
   FIN ? ACK ? FIN ? ACK
```

### Useful Analysis Features
```
1. Follow TCP Stream:
   - Right-click packet ? Follow ? TCP Stream
   - Xem toàn b? conversation

2. Statistics:
   - Statistics ? Conversations ? TCP tab
   - Xem bandwidth, packets count

3. IO Graph:
   - Statistics ? I/O Graph
   - Visualize traffic over time

4. Export Data:
   - File ? Export Packet Dissections
   - Save analysis results
```

## ?? Use Cases th?c t?

### 1. Debug Connection Issues
```
Filter: tcp port 7777
Look for:
- TCP retransmissions [TCP Retransmission]
- Connection resets [TCP Reset]
- Long response times
```

### 2. Analyze Game Performance
```
Filter: tcp port 7777 and tcp.len > 0
Measure:
- Time between CHOICE messages
- Response latency
- Packet loss
```

### 3. Security Analysis
```
Filter: tcp port 7777
Check:
- No unexpected data
- Proper connection handling
- Clean disconnects
```

### 4. Network Learning
```
Observe:
- TCP 3-way handshake
- Application protocol design
- Network error handling
- Connection management
```

## ?? Troubleshooting

### Wireshark không b?t ???c packets
```
Solutions:
1. Ch?n ?úng network interface
2. Check permissions (run as Admin)
3. Disable firewall t?m th?i
4. Try different interface
```

### Filter không ho?t ??ng
```
Solutions:
1. Clear filter và th? l?i
2. Check syntax: tcp port 7777
3. Use display filter thay vì capture filter
4. Try simpler filter first
```

### Game traffic không hi?n
```
Solutions:
1. Ensure game is using correct port
2. Check if playing on localhost vs LAN
3. Verify network interface selection
4. Try capture on "any" interface
```

## ?? Pro Tips

### Display Optimization
```
1. View ? Time Display Format ? Seconds Since Beginning
2. Right-click columns ? Adjust width
3. View ? Colorize Packet List (enable)
4. Analyze ? Display Filters ? Save custom filters
```

### Export và Save
```
1. File ? Export Packet Dissections ? CSV
2. Statistics ? Conversations ? Copy data
3. File ? Save capture for later analysis
4. Edit ? Find Packet (Ctrl+F) for search
```

### Keyboard Shortcuts
```
Ctrl+E - Start/Stop capture
Ctrl+F - Find packet
Ctrl+G - Go to packet number
Ctrl+Shift+O - Open capture file
Space - Mark packet
```

## ?? Educational Value

### Network Concepts h?c ???c
```
1. TCP Protocol:
   - Connection establishment
   - Data transfer
   - Flow control
   - Connection termination

2. Application Protocol:
   - Message format design
   - Request-response patterns
   - Error handling
   - State management

3. Network Performance:
   - Latency measurement
   - Throughput analysis
   - Packet loss detection
   - Connection quality
```

### Skills phát tri?n
```
1. Network Troubleshooting
2. Protocol Analysis
3. Performance Monitoring
4. Security Assessment
```

## ?? Tài li?u tham kh?o

### Wireshark Resources
```
- Official Documentation: https://www.wireshark.org/docs/
- User Guide: https://www.wireshark.org/docs/wsug_html_chunked/
- Display Filters: https://wiki.wireshark.org/DisplayFilters
- Sample Captures: https://wiki.wireshark.org/SampleCaptures
```

### TCP/IP Learning
```
- RFC 793 (TCP): https://tools.ietf.org/html/rfc793
- TCP/IP Illustrated book series
- Computer Networks textbooks
- Online courses (Coursera, edX)
```

---
*T?o b?i: BaiTapGK Team*  
*M?c ?ích: Giáo d?c và h?c t?p network protocols*  
*Tool: Wireshark + Game Oan Tu Ti*