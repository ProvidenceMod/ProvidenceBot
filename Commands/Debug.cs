using System;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Interactivity;

namespace ProvidenceBot.Commands
{
  public class Debug : BaseCommandModule
  { 
    [Command("debug")]
    [Hidden]
    [Description("Provides various commands for debugging the bot while it's running. Moderator use only!")]
    [RequirePermissions(DSharpPlus.Permissions.Administrator)]
    public async Task DebugRun(CommandContext context, [Description("Optional argument to specify what you need to do. Leave blank to toggle debug.")] string specification = "")
    {
      if (specification.Length == 0)
      {
        Bot.debugMode = !Bot.debugMode;
        await context.Channel.SendMessageAsync($"Debug mode is now {(Bot.debugMode ? "ON. Please be careful!" : "OFF.")}");
      }
      else
      {
        switch (specification)
        {
          case "status":
            await context.Channel.SendMessageAsync($"Debug mode is currently {(Bot.debugMode ? "ON. Please be careful!" : "OFF.")}");
            break;
          default:
            await context.Channel.SendMessageAsync("I didn't understand that, please try again.");
            break;
        }
      }
    }
  }
}