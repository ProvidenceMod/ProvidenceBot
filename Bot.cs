﻿using DSharpPlus;
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
      };

      Commands = Client.UseCommandsNext(commandsConfig);

      Commands.RegisterCommands<Suggest>();
      Commands.RegisterCommands<Debug>();
      await Client.ConnectAsync();
      await Task.Delay(-1);
    }

    private Task OnClientReady(object sender, ReadyEventArgs e)
    {
      return Task.CompletedTask;
    }
  }
}
