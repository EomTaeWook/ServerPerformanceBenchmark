# 서버별 성능 비교

| 항목 | DotNetty | DignusSocketServer | SuperSocket 2.0 |
|:---|:---|:---|:---|
| Total Clients | 5000 | 5000 | 5000 |
| Total Received | 4,946,353 | 5,674,121 | 3,170,083 |
| Max RTT (ms) | 140.65 | 136.74 | 128.70 |
| Min RTT (ms) | 0.04 | 0.03 | 0.05 |


# 서버별 성능 결과 캡처

| DignusSocketServer | DotNetty | SuperSocket 2.0 |
|:---:|:---:|:---:|
| ![Dignus Result](image/DignusSocketResult.png) | ![DotNetty Result](image/DotNettyResult.png) | ![SuperSocket 2.0 Result](image/SuperSocket2.0Result.png) |