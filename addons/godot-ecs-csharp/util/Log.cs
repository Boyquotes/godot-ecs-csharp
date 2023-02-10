using System;
using Godot;

public class Log
{

    private Log() { }

    public static void Debug(string msg, Exception? exception = null)
    {
        LogPrefixed("DEBUG", msg, exception);
    }

    public static void Info(string msg, Exception? exception = null)
    {
        LogPrefixed("INFO", msg, exception);
    }

    public static void Warn(string msg, Exception? exception = null)
    {
        LogPrefixed("WARN", msg, exception);
    }

    public static void Error(string msg, Exception? exception = null)
    {
        LogPrefixed("ERROR", msg, exception);
    }

    private static void LogPrefixed(string prefix, string msg, Exception? exception)
    {
        GD.Print($"{DateTime.Now} [{prefix}] {msg}");
        if (exception != null)
        {
            GD.PrintErr($"\tException: {exception.Message}\n{exception.StackTrace}");
        }
    }

}