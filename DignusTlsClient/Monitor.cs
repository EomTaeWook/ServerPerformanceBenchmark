using Dignus.Framework;

namespace DignusTlsClient
{
    internal class Monitor : Singleton<Monitor>
    {
        private long _totalClientCount = 0;
        private long _totalReceivedCount = 0;
        private long _totalBytes = 0;
        private long _totalSendCount = 0;
        public double MaxRttMs { get; private set; } = -1;
        public double MinRttMs { get; private set; } = 999999;

        private object _minRttSyncObj = new object();
        private object _maxRttSyncObj = new object();

        private DateTime _startDateTime = DateTime.MinValue;
        private DateTime _stopDateTime = DateTime.MinValue;
        public void AddReceivedCount(long receivedCount)
        {
            Interlocked.Add(ref _totalReceivedCount, receivedCount);
        }
        public void AddTotalBytes(long bytes)
        {
            Interlocked.Add(ref _totalBytes, bytes);
        }
        public void OnReceived(int size)
        {
            Interlocked.Add(ref _totalBytes, size);
            _stopDateTime = DateTime.UtcNow;
        }
        public void AddTotalSendCount(long sendCount)
        {
            Interlocked.Add(ref _totalSendCount, sendCount);
        }
        public void AddClientCount(long count)
        {
            Interlocked.Add(ref _totalClientCount, count);
        }
        public void Start()
        {
            _startDateTime = DateTime.UtcNow;
        }
        public void Stop() 
        { 
            _stopDateTime = DateTime.UtcNow; 
        }
        public void SetMaxRttMs(double maxRttMs)
        {
            lock (_maxRttSyncObj)
            {
                if (MaxRttMs < maxRttMs)
                {
                    MaxRttMs = maxRttMs;
                }
            }
        }
        public void SetMinRttMs(double minRttMs)
        {
            lock (_minRttSyncObj)
            {
                if (MinRttMs > minRttMs)
                {
                    MinRttMs = minRttMs;
                }
            }
        }
        private string GenerateDataSize(long bytes)
        {
            if (bytes >= 1024L * 1024L * 1024L) return $"{bytes / (1024.0 * 1024.0 * 1024.0):F2} GiB";
            if (bytes >= 1024L * 1024L) return $"{bytes / (1024.0 * 1024.0):F2} MiB";
            if (bytes >= 1024L) return $"{bytes / 1024.0:F2} KiB";
            return $"{bytes} B";
        }
        private string GenerateTimePeriod(double ms)
        {
            if (ms >= 1000) return $"{ms / 1000.0:F3} s";
            return $"{ms:F3} ms";
        }
        public void PrintEchoReport(int messageSize)
        {
            // NetCoreServer와 동일한 계산 방식
            var duration = _stopDateTime - _startDateTime;
            double totalSeconds = duration.TotalSeconds;
            double totalMs = duration.TotalMilliseconds;
            long totalBytes = Interlocked.Read(ref _totalBytes);
            long totalMessages = totalBytes / messageSize;

            Console.WriteLine($"[Monitor]");
            Console.WriteLine($"Total time: {GenerateTimePeriod(totalMs)}");
            Console.WriteLine($"Total data: {GenerateDataSize(totalBytes)}");
            Console.WriteLine($"Total messages: {totalMessages:N0}");

            if (totalSeconds > 0)
            {
                double throughput = totalBytes / totalSeconds;
                Console.WriteLine($"Data throughput: {GenerateDataSize((long)throughput)}/s");
            }

            if (totalMessages > 0)
            {
                double latency = totalMs / totalMessages;
                Console.WriteLine($"Message latency: {GenerateTimePeriod(latency)}");

                double msgThroughput = totalMessages / totalSeconds;
                Console.WriteLine($"Message throughput: {msgThroughput:N0} msg/s");
            }
        }
    }
}
