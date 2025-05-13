using System.Diagnostics;

namespace Services;

public static class Logger
{
    public static void Info(string message)
    {
        Console.WriteLine(message);
    }

    public static void Debug(string message)
    {
        if (Debugger.IsAttached)
        {
            Console.WriteLine(message);
        }
    }

    public static void Error(string message)
    {
        Console.BackgroundColor = ConsoleColor.Red;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}