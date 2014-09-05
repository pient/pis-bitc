using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using log4net;
using PIC.Common;

namespace PIC.Portal
{
    public class LogManager
    {
        #region Consts & Enums

        public const string DefaultLoggerName = "Default";

        public enum LogLevel
        {
            OFF = 0,
            FATAL = 1,
            ERROR = 2,
            WARN = 4,
            INFO = 8,
            DEBUG = 16,
            ALL = 32
        }

        #endregion

        public static void Configure()
        {
            XmlNode node = PICConfigurationManager.LogConfiguration.GetConfig("Log4net");

            if (node != null)
            {
                log4net.Config.XmlConfigurator.Configure(node.FirstChild as XmlElement);
            }
        }

        public static ILog GetLogger(string name, string loggerName = DefaultLoggerName)
        {
            return log4net.LogManager.GetLogger(name);
        }

        public static void Log(string message, string loggerName = DefaultLoggerName)
        {
            Log(message, LogLevel.DEBUG, loggerName);
        }

        public static void Log(string message, Exception ex, string loggerName = DefaultLoggerName)
        {
            Log(message, ex, LogLevel.DEBUG, loggerName);
        }

        public static void Log(string message, LogLevel level, string loggerName = DefaultLoggerName)
        {
            ILog logger = LogManager.GetLogger(loggerName ?? DefaultLoggerName);

            switch (level)
            {
                case LogLevel.DEBUG:
                    logger.Debug(message);
                    break;
                case LogLevel.WARN:
                    logger.Info(message);
                    break;
                case LogLevel.INFO:
                    logger.Info(message);
                    break;
                case LogLevel.ERROR:
                    logger.Error(message);
                    break;
                case LogLevel.FATAL:
                    logger.Fatal(message);
                    break;
                default:
                    logger.Debug(message);
                    break;
            }
        }

        public static void Log(string message, Exception ex, LogLevel level, string loggerName = DefaultLoggerName)
        {
            ILog logger = LogManager.GetLogger(loggerName ?? DefaultLoggerName);

            switch (level)
            {
                case LogLevel.DEBUG:
                    logger.Debug(message, ex);
                    break;
                case LogLevel.WARN:
                    logger.Info(message, ex);
                    break;
                case LogLevel.INFO:
                    logger.Info(message, ex);
                    break;
                case LogLevel.ERROR:
                    logger.Error(message, ex);
                    break;
                case LogLevel.FATAL:
                    logger.Fatal(message, ex);
                    break;
                default:
                    logger.Debug(message, ex);
                    break;
            }
        }
    }
}
