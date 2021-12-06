using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.Webhook;
using Discord.WebSocket;
using Interactivity;
using SnowyBot.Handlers;
using SnowyBot.Services;
using SnowyBot.Structs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyBot.Handlers
{
  public class CommandHandler
  {
    public static IServiceProvider provider;
    public static DiscordSocketClient client;
    public static CommandService commands;
    public CommandHandler(DiscordSocketClient _client, CommandService _commands, IServiceProvider _provider)
    {
      provider = _provider;
      client = _client;
      commands = _commands;

      client.MessageReceived += Client_MessageRecieved;
      client.InteractionCreated += Client_InteractionCreated;
      client.ReactionAdded += Client_ReactionAdded;
    }

    private async Task Client_MessageRecieved(SocketMessage arg)
    {
      var socketMessage = arg as SocketUserMessage;

      if (!(arg is SocketUserMessage message) || message.Author.IsBot || message.Author.IsWebhook || message.Channel is IPrivateChannel)
        return;

      var context = new SocketCommandContext(client, socketMessage);
      var argPos = 0;

      if (!message.HasStringPrefix("!", ref argPos) && !message.HasMentionPrefix(DiscordService.client.CurrentUser, ref argPos))
        return;

      await commands.ExecuteAsync(context, argPos, provider, MultiMatchHandling.Best).ConfigureAwait(false);
        return;
    }
    private async Task Client_InteractionCreated(SocketInteraction interaction)
    {
      switch (interaction)
      {
        case SocketSlashCommand commandInteraction:
          await HandleSlash(commandInteraction).ConfigureAwait(false);
          break;
        case SocketMessageComponent componentInteraction:
          await HandleComponent(componentInteraction).ConfigureAwait(false);
          break;
        default:
          throw new NotImplementedException();
      }
    }
    private async Task Client_ReactionAdded(Cacheable<IUserMessage, ulong> arg1, Cacheable<IMessageChannel, ulong> arg2, SocketReaction arg3)
    {
      throw new NotImplementedException();
    }
    private async Task HandleSlash(SocketSlashCommand command)
    {
      throw new NotImplementedException();
    }
    private async Task HandleComponent(SocketMessageComponent component)
    {
      throw new NotImplementedException();
    }
  }
}
