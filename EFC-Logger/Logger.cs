using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFC_Logger;

public static class Logger
{
    public static string _logFileDirectory = "EFC-Logger/Logs";
    public static void AddLogging(this DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder =>
        {
            // builder.AddConsole();
        }))
        .EnableSensitiveDataLogging()
        // .LogTo(System.Console.WriteLine, LogLevel.Information)
        .LogToFile();
    }
    private static DbContextOptionsBuilder LogToFile(this DbContextOptionsBuilder optionsBuilder)
    {
        return optionsBuilder.LogTo(WriteToFile, LogLevel.Information);
    }

    private static void WriteToFile(string logMessage)
    {
        string? currentDirectory = (Directory.GetParent(Directory.GetCurrentDirectory())?.FullName) ?? throw new DirectoryNotFoundException("Directory not found.");
        string logFileDirectory = Path.Combine(currentDirectory, _logFileDirectory);
        if (!Directory.Exists(logFileDirectory))
        {
            Directory.CreateDirectory(logFileDirectory);
        }
        string? logFilePath = Path.Combine(currentDirectory, _logFileDirectory, "logs.txt");
        using StreamWriter sw = File.AppendText(logFilePath);
        sw.WriteLine(logMessage);
    }
}
