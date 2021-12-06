using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SnowyBot.Services
{
  public static class LoggingService
  {
    public static async Task LogAsync(string src, LogSeverity severity, string message, Exception exception = null)
    {
      if (severity.Equals(null))
        severity = LogSeverity.Warning;

      await Append(GetSeverityString(severity), GetConsoleColor(severity)).ConfigureAwait(false);
      await Append($" [{SourceToString(src)}] ", ConsoleColor.DarkGray).ConfigureAwait(false);

      if (!string.IsNullOrWhiteSpace(message))
        await Append($"{message}\n", ConsoleColor.White).ConfigureAwait(false);
      else if (exception == null)
        await Append("Uknown Exception. Exception Returned Null.\n", ConsoleColor.DarkRed).ConfigureAwait(false);
      else if (exception.Message == null)
        await Append($"Unknown \n{exception.StackTrace}\n", GetConsoleColor(severity)).ConfigureAwait(false);
      else
        await Append($"{exception.Message ?? "Unknown"}\n{exception.StackTrace ?? "Unknown"}\n", GetConsoleColor(severity)).ConfigureAwait(false);
    }

    public static async Task LogCriticalAsync(string source, string message, Exception exc = null)
        => await LogAsync(source, LogSeverity.Critical, message, exc).ConfigureAwait(false);

    public static async Task LogInformationAsync(string source, string message)
        => await LogAsync(source, LogSeverity.Info, message).ConfigureAwait(false);

    private static async Task Append(string message, ConsoleColor color)
    {
      await Task.Run(() =>
      {
        Console.ForegroundColor = color;
        Console.Write(message);
      }).ConfigureAwait(false);
    }
    private static string SourceToString(string src)
    {
      return src.ToLower() switch
      {
        "discord" => "DISC",
        "admin" => "ADMN",
        "gateway" => "GTWY",
        "bot" => "RBOT",
        _ => src,
      };
    }
    private static string GetSeverityString(LogSeverity severity)
    {
      return severity switch
      {
        LogSeverity.Critical => "CRIT",
        LogSeverity.Error => "EROR",
        LogSeverity.Warning => "WARN",
        LogSeverity.Info => "INFO",
        LogSeverity.Verbose => "VERB",
        LogSeverity.Debug => "DBUG",
        _ => "UNKN",
      };
    }

    /* Return The Console Color Based On Severity Selected */
    private static ConsoleColor GetConsoleColor(LogSeverity severity)
    {
      return severity switch
      {
        LogSeverity.Critical => ConsoleColor.Red,
        LogSeverity.Error => ConsoleColor.DarkRed,
        LogSeverity.Warning => ConsoleColor.Yellow,
        LogSeverity.Info => ConsoleColor.Green,
        LogSeverity.Verbose => ConsoleColor.Blue,
        LogSeverity.Debug => ConsoleColor.DarkBlue,
        _ => ConsoleColor.White,
      };
    }
  }
}
