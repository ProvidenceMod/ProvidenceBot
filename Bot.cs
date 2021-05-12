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

    public async Task RunAsync()
    {
      string json = string.Empty;

      using (var fs = File.OpenRead("config.json"))
      using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
        json = await sr.ReadToEndAsync().ConfigureAwait(false);

      var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

      DiscordConfiguration config = new DiscordConfiguration
      {
        Token = configJson.Token,
        TokenType = TokenType.Bot,
        AutoReconnect = true,
        MinimumLogLevel = LogLevel.Debug,
      };
      Client = new DiscordClient(config);

      Client.Ready += OnClientReady;

      Client.UseInteractivity(new InteractivityConfiguration
      {
        Timeout = TimeSpan.FromMinutes(1)
      });

      CommandsNextConfiguration commandsConfig = new CommandsNextConfiguration()
      {
        StringPrefixes = new string[] { configJson.Prefix },
        EnableDms = false,
        EnableMentionPrefix = true,
        DmHelp = true,
      };

      Commands = Client.UseCommandsNext(commandsConfig);

      Commands.RegisterCommands<Suggest>();
      var discord = new DiscordClient(config);
      discord.MessageCreated += async (s, e) =>
      {
        if (e.Message.Content[0].ToString() == configJson.Prefix)
        {
          string[] words = e.Message.Content.ToLower().Substring(1).Split(" ");
          string arg = words[0];
          switch (arg)
          {
            case "help":
              await e.Message.RespondAsync("You too?");
              break;
            default:
              await e.Message.RespondAsync("I don't know what you're saying! Try harder!");
              break;
          }
        }
      };
      await Client.ConnectAsync();
      await Task.Delay(-1);
    }

    private Task OnClientReady(object sender, ReadyEventArgs e)
    {
      return Task.CompletedTask;
    }
  }
}
