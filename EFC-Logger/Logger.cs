using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFC_Logger;

public static class Logger
{
    public static string _logFileDirectory = "EFC-Logger/Logs";
    public static void AddLogging(this DbContextOptionsBuilder optionsBuilder, string providerName)
    {
        optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder =>
        {
            // builder.AddConsole();
        }))
        .EnableSensitiveDataLogging()
        // .LogTo(System.Console.WriteLine, LogLevel.Information)
        .LogToFile(providerName);
    }
    private static DbContextOptionsBuilder LogToFile(this DbContextOptionsBuilder optionsBuilder, string providerName)
    {
        void WriteToFile(string logMessage)
        {
            string? currentDirectory = (Directory.GetParent(Directory.GetCurrentDirectory())?.FullName) ?? throw new DirectoryNotFoundException("Directory not found.");
            string logFileDirectory = Path.Combine(currentDirectory, _logFileDirectory, providerName);
            if (!Directory.Exists(logFileDirectory))
            {
                Directory.CreateDirectory(logFileDirectory);
            }
            DateOnly date = DateOnly.FromDateTime(DateTime.Now);
            var (year, month, day) = (date.Year, date.Month, date.Day);
            string? logFilePath = Path.Combine(logFileDirectory, $"{year}_{month}_{day}_{providerName}.txt");
            using StreamWriter sw = File.AppendText(logFilePath);
            sw.WriteLine(logMessage);
        }

        return optionsBuilder.LogTo(WriteToFile, LogLevel.Information);
    }
}
