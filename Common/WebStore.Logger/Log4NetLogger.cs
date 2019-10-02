using log4net;
using log4net.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace WebStore.Logger
{
    public class Log4NetLogger : ILogger
    {
        private readonly ILog _log;

        public Log4NetLogger(string category, XmlElement config)
        {
            var logger_repository = LogManager.CreateRepository(
                Assembly.GetEntryAssembly(),
                typeof(log4net.Repository.Hierarchy.Hierarchy));

            _log = LogManager.GetLogger(logger_repository.Name, category);
            log4net.Config.XmlConfigurator.Configure(logger_repository, config);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                default: throw new ArgumentOutOfRangeException(nameof(LogLevel), logLevel, null);

                case LogLevel.Trace:
                case LogLevel.Debug:
                    return _log.IsDebugEnabled;

                case LogLevel.Information:
                    return _log.IsInfoEnabled;

                case LogLevel.Warning:
                    return _log.IsWarnEnabled;

                case LogLevel.Error:
                    return _log.IsErrorEnabled;

                case LogLevel.Critical:
                    return _log.IsFatalEnabled;

                case LogLevel.None:
                    return false;
            }
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            if (formatter is null) throw new ArgumentNullException(nameof(formatter));

            var log_message = formatter(state, exception);

            if (string.IsNullOrEmpty(log_message) && exception is null) return;

            switch (logLevel)
            {
                default: throw new ArgumentOutOfRangeException(nameof(LogLevel), logLevel, null);

                case LogLevel.Trace:
                case LogLevel.Debug:
                    _log.Debug(log_message);
                    break;
                case LogLevel.Information:
                    _log.Info(log_message);
                    break;
                case LogLevel.Warning:
                    _log.Warn(log_message);
                    break;
                case LogLevel.Error:
                    _log.Error(log_message ?? exception.ToString());
                    break;
                case LogLevel.Critical:
                    _log.Fatal(log_message ?? exception.ToString());
                    break;
                case LogLevel.None:
                    break;
            }
        }
    }
}
