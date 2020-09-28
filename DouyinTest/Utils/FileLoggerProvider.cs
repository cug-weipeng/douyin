using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DouyinTest.Utils
{
    public class FileLoggerProvider : ILoggerProvider
    {
        IConfiguration config;
        public FileLoggerProvider(IConfiguration configuration)
        {
            config = configuration;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(config,categoryName);
        }

        public void Dispose()
        {
        }
    }
}
