using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Interactivity;
using Microsoft.Extensions.DependencyInjection;
using SnowyBot.Handlers;
using System;
using System.Threading.Tasks;
using Victoria;
using System.Linq;
using System.Threading;
using SnowyBot.Modules;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace SnowyBot.Services
{
  public static class DiscordService
  {
    public static readonly DiscordSocketClient client;
    public static readonly ServiceProvider provider;
    public static readonly CommandService commands;
    public static readonly FunModule funModule;
    public static readonly ConfigModule configService;
    public static readonly InteractivityService interactivity;

    public static string ownerAvatarURL;

    static DiscordService()
    {

      // Service Setup //
      provider = ConfigureServices();
      client = provider.GetRequiredService<DiscordSocketClient>();
      funModule = provider.GetRequiredService<FunModule>();
      configService = provider.GetRequiredService<ConfigModule>();
      interactivity = provider.GetRequiredService<InteractivityService>();
      commands = provider.GetRequiredService<CommandService>();
      // Discord Events //
      client.Ready += ReadyAsync;
      client.Log += LogAsync;
    }

    public static async Task InitializeAsync()
    {
      provider.GetRequiredService<CommandHandler>();
      provider.GetRequiredService<EmbedHandler>();
      await provider.GetRequiredService<StartupService>().StartAsync().ConfigureAwait(false);
      await Task.Delay(-1).ConfigureAwait(false);
    }
    private static async Task ReadyAsync()
    {
      await client.SetActivityAsync(new Game("ProvidenceMod", ActivityType.Watching, ActivityProperties.None, "https://discord.gg/bhCuppRdVF")).ConfigureAwait(false);
    }
    private static async Task LogAsync(LogMessage logMessage)
    {
      await LoggingService.LogAsync(logMessage.Source, logMessage.Severity, logMessage.Message).ConfigureAwait(false);
    }
    private static ServiceProvider ConfigureServices()
    {
      return new ServiceCollection()
        .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
        {
          LogLevel = LogSeverity.Verbose,
          AlwaysDownloadUsers = true,
        }))
        .AddSingleton(new CommandService(new CommandServiceConfig()
        {
          LogLevel = LogSeverity.Verbose,
          DefaultRunMode = RunMode.Async,
          CaseSensitiveCommands = false
        }))
        .AddSingleton<CommandHandler>()
        .AddSingleton<EmbedHandler>()
        .AddSingleton<FunModule>()
        .AddSingleton<ConfigModule>()
        .AddSingleton<InteractivityService>()
        .AddSingleton(new InteractivityConfig
        {
          DefaultTimeout = TimeSpan.FromSeconds(20)
        })
        .AddSingleton<StartupService>()
        .BuildServiceProvider();
    }
  }
}
