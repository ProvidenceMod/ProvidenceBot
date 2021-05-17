using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProvidenceBot.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ProvidenceBot
{
  public class Bot
  {
    public DiscordClient Client { get; private set; }
    public InteractivityExtension Interactivity { get; private set; }
    public CommandsNextExtension Commands { get; private set; }
    public static bool debugMode;

    public Bot(IServiceProvider services)
    {
      string json = string.Empty;

      using (var fs = File.OpenRead("config.json"))
      using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
        json =  sr.ReadToEnd();
      var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

      DiscordConfiguration config = new DiscordConfiguration
      {
        Token = configJson.Token,
        TokenType = TokenType.Bot,
        AutoReconnect = true,
        MinimumLogLevel = LogLevel.Debug
      };
      debugMode = configJson.DebugMode;

      Client = new DiscordClient(config);

      Client.Ready += OnClientReady;

      Client.UseInteractivity(new InteractivityConfiguration
      {
        Timeout = TimeSpan.FromMinutes(1)
      });

      CommandsNextConfiguration commandsConfig = new CommandsNextConfiguration()
      {
        StringPrefixes = new string[] { configJson.Prefix },
        EnableDms = true,
        EnableMentionPrefix = true,
        DmHelp = true,
        Services = services,
      };

      Commands = Client.UseCommandsNext(commandsConfig);

      Commands.RegisterCommands<Suggest>();
      Commands.RegisterCommands<Debug>();
      Client.ConnectAsync();
    }

    private Task OnClientReady(object sender, ReadyEventArgs e)
    {
      return Task.CompletedTask;
    }
  }
}
