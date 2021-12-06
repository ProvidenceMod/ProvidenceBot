using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using SnowyBot.Handlers;
using SnowyBot.Structs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SnowyBot.Services
{
  public class StartupService
  {
    public static IServiceProvider provider;
    private readonly DiscordSocketClient discord;
    private readonly CommandService commands;

    public StartupService(IServiceProvider _provider, DiscordSocketClient _discord, CommandService _commands)
    {
      provider = _provider;
      discord = _discord;
      commands = _commands;
    }

    public async Task StartAsync()
    {
      string json = File.ReadAllText("config.json", new UTF8Encoding(false));

      BotConfig botConfig = JsonConvert.DeserializeObject<BotConfig>(json);

      await discord.LoginAsync(TokenType.Bot, botConfig.DiscordToken).ConfigureAwait(false);

      await discord.StartAsync().ConfigureAwait(false);

      await commands.AddModulesAsync(Assembly.GetEntryAssembly(), provider).ConfigureAwait(false);
    }
  }
}
