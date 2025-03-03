using Bank.Contract;
using NLog;
using ILogger = NLog.ILogger;

namespace Bank.Logger;

public class LoggerManager : ILoggerManager
{
    private static ILogger logger = LogManager.GetCurrentClassLogger();

    public LoggerManager()
    {
    }

    public void Debug(string message) => logger.Debug(message);

    public void Error(string message) => logger.Error(message);

    public void Info(string message) => logger.Info(message);

    public void Warn(string message) => logger.Warn(message);
}
