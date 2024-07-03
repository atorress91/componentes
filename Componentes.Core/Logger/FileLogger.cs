namespace Componentes.Core.Logger
{
    public class FileLogger
    {
        private readonly string _logFilePath;
        private static readonly object _lock = new object();

        public FileLogger(string logFilePath)
        {
            _logFilePath = logFilePath;
            InitializeLogFile();
        }

        private void InitializeLogFile()
        {
            lock (_lock)
            {
                if (!File.Exists(_logFilePath))
                {
                    using (var stream = File.Create(_logFilePath))
                    {
                        Log("Log file created.");
                    }
                }
            }
        }

        public void Log(string message)
        {
            lock (_lock)
            {
                try
                {
                    using (var writer = new StreamWriter(_logFilePath, true))
                    {
                        writer.WriteLine($"{DateTime.Now}: {message}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to write to log file: {ex.Message}");
                }
            }
        }

        public void LogError(string message)
        {
            Log($"ERROR: {message}");
        }
    }
}