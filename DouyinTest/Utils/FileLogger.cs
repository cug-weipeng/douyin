using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DouyinTest.Utils
{
    public class FileLogger : ILogger
    {
        private static readonly object lockObj = new object();

        IConfiguration config;
        string name;

        public FileLogger(IConfiguration configuration, string category)
        {
            config = configuration;
            name = category;
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            LogLevel configLevel = LogLevel.Warning;
            var configLevelString = config["Logging:LogLevel:Default"];

            if (Enum.TryParse(typeof(LogLevel), configLevelString, out var temp))
            {
                configLevel = (temp as LogLevel?).Value;
            }

            return logLevel >= configLevel && logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }
            string path = $"{AppDomain.CurrentDomain.BaseDirectory}logger\\{DateTime.Now:yyyyMMdd}.txt";
            lock (lockObj)
            {
                using (StreamWriter streamWriter = new StreamWriter(path, true))
                {
                    streamWriter.WriteLine($"{DateTime.Now:HH:mm:ss}:{logLevel}: {name}:{formatter(state, exception)}");
                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }
        }
    }
}
