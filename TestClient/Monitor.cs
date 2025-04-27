using Dignus.Framework;

namespace EchoClient
{
    internal class Monitor : Singleton<Monitor>
    {
        private long _totalClientCount = 0;
        private long _totalReceivedCount = 0;
        public double MaxRttMs { get; private set; } = -1;
        public double MinRttMs { get; private set; } = 999999;

        private object _minRttSyncObj = new object();
        private object _maxRttSyncObj = new object();
        public void AddReceivedCount(long receivedCount)
        {
            Interlocked.Add(ref _totalReceivedCount, receivedCount);
        }
        public void AddClientCount(long count)
        {
            Interlocked.Add(ref _totalClientCount, count);
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

        public void Print(string serverName)
        {
            Console.WriteLine($"[Monitor]");
            if (string.IsNullOrEmpty(serverName) == false)
            {
                Console.WriteLine($"[{serverName}]");
            }
            Console.WriteLine($"Total Client: {_totalClientCount}");
            Console.WriteLine($"Total Received: {_totalReceivedCount}");
            Console.WriteLine($"Max RTT (ms): {MaxRttMs:F2}");
            Console.WriteLine($"Min RTT (ms): {MinRttMs:F2}");
        }

        public void Clear()
        {
            _totalClientCount = 0;
            _totalReceivedCount = 0;
            MaxRttMs = -1;
            MinRttMs = 999999;
        }
    }
}
