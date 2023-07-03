using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace NordPassHomeWorkTAF.StartupTeardown
{
    public class NUnitLogger : ILogger
    {
        private readonly string _name;

        public NUnitLogger(string name)
        {
            _name = name;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter != null)
            {
                TestContext.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}: {logLevel}: {formatter(state, exception)}");
            }
        }
    }

    public class NUnitLoggerProvider : ILoggerProvider
    {
        public void Dispose()
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new NUnitLogger(categoryName);
        }
    }

}
