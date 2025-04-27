# 서버별 성능 비교 (2024-04-27)

## 🧪 테스트 조건

- 클라이언트가 서버에 접속합니다.
- 접속 이후, 각 클라이언트는 **30초 동안** 지속적으로 Echo 요청을 보냅니다.
- 서버는 Echo 요청을 수신 후, **간단한 비즈니스 로직(Json 역직렬화/직렬화)을 처리한 뒤** 응답합니다.
- 30초가 지나면 클라이언트는 연결을 종료하거나 패킷 전송을 멈춥니다.
- 측정 항목: 총 수신 패킷 수, 최대/최소 왕복 지연시간.

# 서버별 성능 비교 (2024-04-27)

| 항목 | DignusSocketServer | DotNetty | SuperSocket 2.0 |
|:---|:---|:---|:---|
| Total Clients | 5000 | 5000 | 5000 |
| Total Received | 5,674,121 | 4,946,353 | 3,170,083 |
| Max RTT (ms) | 136.74 | 140.65 | 128.70 |
| Min RTT (ms) | 0.03 | 0.04 | 0.05 |

# 서버별 성능 결과 캡처

| DignusSocketServer | DotNetty | SuperSocket 2.0 |
|:---:|:---:|:---:|
| ![Dignus Result](Image/DignusSocketResult.png) | ![DotNetty Result](Image/DotNettyResult.png) | ![SuperSocket 2.0 Result](Image/SuperSocket2.0Result.png) |

---

## 🧪 Test Conditions

- Clients connect to the server.
- After connection, each client continuously sends Echo requests for **30 seconds**.
- Upon receiving an Echo request, the server **processes simple business logic (Json deserialization/serialization)** and then responds.
- After 30 seconds, clients disconnect or stop sending packets.
- Measurement items: Total packets received, Max RTT, Min RTT.

# Server Performance Comparison (2024-04-27)

| Item | DignusSocketServer | DotNetty | SuperSocket 2.0 |
|:---|:---|:---|:---|
| Total Clients | 5000 | 5000 | 5000 |
| Total Received | 5,674,121 | 4,946,353 | 3,170,083 |
| Max RTT (ms) | 136.74 | 140.65 | 128.70 |
| Min RTT (ms) | 0.03 | 0.04 | 0.05 |

# Server Performance Result Screenshots

| DignusSocketServer | DotNetty | SuperSocket 2.0 |
|:---:|:---:|:---:|
| ![Dignus Result](Image/DignusSocketResult.png) | ![DotNetty Result](Image/DotNettyResult.png) | ![SuperSocket 2.0 Result](Image/SuperSocket2.0Result.png) |

---

## 📅 Test Date
- 2024-04-27
