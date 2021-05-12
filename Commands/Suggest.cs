using System;
using System.Text;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Newtonsoft.Json;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Interactivity;

namespace ProvidenceBot.Commands
{
  public class Suggest : BaseCommandModule
  {
    [Command("suggest")]
    [Description("Suggests something in the #suggestions channel.")]
    public async Task Suggestion(CommandContext context)
    {
      DiscordMessage commandMesssage = context.Message;
      DiscordMessage titleQuestion = await context.Channel.SendMessageAsync("What is this suggestion about?");
      InteractivityExtension titleInteractivity = context.Client.GetInteractivity();
      InteractivityResult<DiscordMessage> titleResponse = await titleInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name).ConfigureAwait(false);
      DiscordMessage titleMessage = titleResponse.Result;
      DiscordMessage descriptionQuestion = await context.Channel.SendMessageAsync("Please provide a description about this suggestion.");
      InteractivityExtension descriptionInteractivity = context.Client.GetInteractivity();
      InteractivityResult<DiscordMessage> descriptionResponse = await descriptionInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name).ConfigureAwait(false);
      DiscordMessage descriptionMessage = descriptionResponse.Result;
      await context.Channel.SendMessageAsync($"{titleMessage} | {context.User.Mention} | {DateTime.Now.Day} - {DateTime.Now.Month} - {DateTime.Now.Year}\n{descriptionMessage}");
      await titleQuestion.DeleteAsync();
      await titleMessage.DeleteAsync();
      await descriptionQuestion.DeleteAsync();
      await descriptionMessage.DeleteAsync();
    }
  }
}