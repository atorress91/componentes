using Componentes.Core.Logger;
using Microsoft.Extensions.Logging;

namespace Componentes.Core.Provider
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private readonly FileLogger _fileLogger;

        public FileLoggerProvider(FileLogger fileLogger)
        {
            _fileLogger = fileLogger;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLoggerAdapter(_fileLogger);
        }

        public void Dispose()
        {
        }
    }

    public class FileLoggerAdapter : ILogger
    {
        private readonly FileLogger _fileLogger;

        public FileLoggerAdapter(FileLogger fileLogger)
        {
            _fileLogger = fileLogger;
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            string message = formatter(state, exception);

            switch (logLevel)
            {
                case LogLevel.Error:
                case LogLevel.Critical:
                    _fileLogger.LogError(message);
                    break;
                default:
                    _fileLogger.Log(message);
                    break;
            }
        }
    }
}