# 📈 DignusSocketServer Benchmark
- Server address: 127.0.0.1
- Server port: 5000
- Working clients: 1
- Working messages: 1000
- Message size: 32
- Seconds to benchmarking: 10

- Errors: 0

- Total time: 10.001 s
- Total data: 1.225 GiB
- Total messages: 41,108,797
- Data throughput: 125.4 MiB/s
- Message throughput: 4,110,371 msg/s

---

![TopPerformance](Image/Dignus_Performance_41108797msg_1225MiB.png)

---

# 서버별 성능 비교 (2024-04-27)

## 🧪 테스트 조건

- 클라이언트가 서버에 접속합니다.
- 접속 이후, 각 클라이언트는 **30초 동안** 지속적으로 Echo 요청을 보냅니다.
- 서버는 Echo 요청을 수신 후, **간단한 비즈니스 로직(Json 역직렬화/직렬화)을 처리한 뒤** 응답합니다.
- 30초가 지나면 클라이언트는 연결을 종료하거나 패킷 전송을 멈춥니다.
- 측정 항목: 총 수신 패킷 수, 최대/최소 왕복 지연시간.

---

## 🚀 추가 참고사항 (초기 테스트 vs 웜업 후 테스트)

- 최초 테스트는 서버 부팅 직후 바로 진행되었으며, 최적화 전 상태였습니다.
- 웜업 후 테스트는 서버를 재시작하지 않고 클라이언트만 재실행하여 진행되었습니다.

---

# 🧊 서버 초기 상태 테스트 결과 (Cold Start)

| 항목 | DignusSocketServer | DotNetty | SuperSocket 2.0 |
|:---|:---|:---|:---|
| Total Clients | 5000 | 5000 | 5000 |
| Total Received | 5,984,326 | 4,946,353 | 3,170,083 |
| Max RTT (ms) | 73.02 | 140.65 | 128.70 |
| Min RTT (ms) | 0.03 | 0.04 | 0.05 |

---

# 🔥 웜업 이후 테스트 결과 (Warm-up)

| 항목 | DignusSocketServer | DotNetty | SuperSocket 2.0 |
|:---|:---|:---|:---|
| Total Clients | 5000 | 5000 | 5000 |
| Total Received | 6,543,785 | 5,226,484 | 3,127,793 |
| Max RTT (ms) | 40.56 | 136.36 | 146.61 |
| Min RTT (ms) | 0.02 | 0.04 | 0.05 |

---

# 📊 초기 vs 웜업 비교

| 항목 | DignusSocketServer (초기) | DignusSocketServer (웜업 후) | DotNetty (초기) | DotNetty (웜업 후) | SuperSocket 2.0 (초기) | SuperSocket 2.0 (웜업 후) |
|:---|:---|:---|:---|:---|:---|:---|
| Total Clients | 5000 | 5000 | 5000 | 5000 | 5000 | 5000 |
| Total Received | 5,984,326 | 6,543,785 | 4,946,353 | 5,226,484 | 3,170,083 | 3,127,793 |
| Max RTT (ms) | 73.02 | 40.56 | 140.65 | 136.36 | 128.70 | 146.61 |
| Min RTT (ms) | 0.03 | 0.02 | 0.04 | 0.04 | 0.05 | 0.05 |

---

# 🖼️ 테스트 결과 캡처 (초기)

| DignusSocketServer | DotNetty | SuperSocket 2.0 |
|:---:|:---:|:---:|
| ![Dignus Result](Image/DignusSocketResult.png) | ![DotNetty Result](Image/DotNettyResult.png) | ![SuperSocket 2.0 Result](Image/SuperSocket2.0Result.png) |

---

# 🖼️ 테스트 결과 캡처 (웜업 후)

| DignusSocketServer (Warm-up) | DotNetty (Warm-up) | SuperSocket 2.0 (Warm-up) |
|:---:|:---:|:---:|
| ![Dignus Warmup](Image/DignusSocketWarmup.png) | ![DotNetty Warmup](Image/DotNettyWarmup.png) | ![SuperSocket 2.0 Warmup](Image/SuperSocket2.0Warmup.png) |

---

## 📅 테스트 일자
- 2024-04-27


# Server Performance Comparison (2024-04-27)

## 🧪 Test Conditions

- Clients connect to the server.
- After connection, each client continuously sends Echo requests for **30 seconds**.
- Upon receiving an Echo request, the server **processes simple business logic (Json deserialization/serialization)** and then responds.
- After 30 seconds, clients disconnect or stop sending packets.
- Measurement items: Total packets received, Max RTT, Min RTT.

---

## 🚀 Additional Notes (Cold Start vs Warm-up Test)

- The initial test was conducted immediately after the server boot, before full optimization.
- The warm-up test was conducted by re-running only the clients without restarting the server.

---

# 🧊 Initial Server State Test Results (Cold Start)

| Item | DignusSocketServer | DotNetty | SuperSocket 2.0 |
|:---|:---|:---|:---|
| Total Clients | 5000 | 5000 | 5000 |
| Total Received | 5,984,326 | 4,946,353 | 3,170,083 |
| Max RTT (ms) | 73.02 | 140.65 | 128.70 |
| Min RTT (ms) | 0.03 | 0.04 | 0.05 |

---

# 🔥 Post-Warm-up Test Results

| Item | DignusSocketServer | DotNetty | SuperSocket 2.0 |
|:---|:---|:---|:---|
| Total Clients | 5000 | 5000 | 5000 |
| Total Received | 6,543,785 | 5,226,484 | 3,127,793 |
| Max RTT (ms) | 40.56 | 136.36 | 146.61 |
| Min RTT (ms) | 0.02 | 0.04 | 0.05 |

---

# 📊 Cold Start vs Warm-up Comparison

| Item | DignusSocketServer (Cold) | DignusSocketServer (Warm-up) | DotNetty (Cold) | DotNetty (Warm-up) | SuperSocket 2.0 (Cold) | SuperSocket 2.0 (Warm-up) |
|:---|:---|:---|:---|:---|:---|:---|
| Total Clients | 5000 | 5000 | 5000 | 5000 | 5000 | 5000 |
| Total Received | 5,984,326 | 6,543,785 | 4,946,353 | 5,226,484 | 3,170,083 | 3,127,793 |
| Max RTT (ms) | 73.02 | 40.56 | 140.65 | 136.36 | 128.70 | 146.61 |
| Min RTT (ms) | 0.03 | 0.02 | 0.04 | 0.04 | 0.05 | 0.05 |

# 🖼️ Test Result Captures (Cold Start)

| DignusSocketServer | DotNetty | SuperSocket 2.0 |
|:---:|:---:|:---:|
| ![Dignus Result](Image/DignusSocketResult.png) | ![DotNetty Result](Image/DotNettyResult.png) | ![SuperSocket 2.0 Result](Image/SuperSocket2.0Result.png) |

---

# 🖼️ Test Result Captures (Warm-up)

| DignusSocketServer (Warm-up) | DotNetty (Warm-up) | SuperSocket 2.0 (Warm-up) |
|:---:|:---:|:---:|
| ![Dignus Warmup](Image/DignusSocketWarmup.png) | ![DotNetty Warmup](Image/DotNettyWarmup.png) | ![SuperSocket 2.0 Warmup](Image/SuperSocket2.0Warmup.png) |
