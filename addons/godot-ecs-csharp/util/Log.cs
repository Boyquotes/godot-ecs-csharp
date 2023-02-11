using System;
using Godot;

public sealed class Log
{

    public const int LOG_LEVEL_ALL = 0,
                        LOG_LEVEL_DEBUG = 10,
                        LOG_LEVEL_INFO = 20,
                        LOG_LEVEL_WARN = 30,
                        LOG_LEVEL_ERR = 40,
                        LOG_LEVEL_NONE = 50;

    public static int LogLevel = LOG_LEVEL_WARN;

    private Log() { }

    public static void Debug(string msg, Exception? exception = null)
    {
        LogPrefixed(LOG_LEVEL_DEBUG, "DEBUG", msg, exception);
    }

    public static void Info(string msg, Exception? exception = null)
    {
        LogPrefixed(LOG_LEVEL_INFO, "INFO", msg, exception);
    }

    public static void Warn(string msg, Exception? exception = null)
    {
        LogPrefixed(LOG_LEVEL_WARN, "WARN", msg, exception);
    }

    public static void Error(string msg, Exception? exception = null)
    {
        LogPrefixed(LOG_LEVEL_ERR, "ERROR", msg, exception);
    }

    private static void LogPrefixed(int level, string prefix, string msg, Exception? exception)
    {
        if (level < LogLevel)
            return;
        GD.Print($"{DateTime.Now} [{prefix}] {msg}");
        if (exception != null)
            GD.PrintErr($"\tException: {exception.Message}\n{exception.StackTrace}");
    }

}