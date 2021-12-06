using Discord;
using Discord.Commands;
using SnowyBot.Handlers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SnowyBot.Modules
{
  public class FunModule : ModuleBase
  {
    [Command("Info")]
    public async Task Info()
    {
      Console.WriteLine($"{Context.Message.Author.Username}#{Context.Message.Author.Discriminator} : {Context.Message.Author.Id} in {Context.Guild.Name} : {Context.Guild.Id}");

      SocketCommandContext context = Context as SocketCommandContext;
      int numUsers = 0;
      int numBots = 0;
      foreach (IGuildUser user in context.Guild.Users)
      {
        if (user.IsBot)
          numBots++;
        else
          numUsers++;
      }

      EmbedBuilder builder = new EmbedBuilder();
      builder.WithTitle($"{context.Guild.Name}");
      builder.WithThumbnailUrl(context.Guild.IconUrl);
      builder.WithDescription($"{context.Guild.Description}");
      builder.WithColor(new Color(0xac4554));
      builder.AddField("Prefix", "!", true);
      builder.AddField("Owner", context.Guild.Owner.Username, true);
      builder.AddField("Creation Date", $"{context.Guild.CreatedAt}");
      builder.AddField("Boost Level", $"{context.Guild.PremiumTier.ToString().Insert(4, " ")}", true);
      builder.AddField("Text Channels", $"{context.Guild.TextChannels.Count}", true);
      builder.AddField("Voice Channels", $"{context.Guild.VoiceChannels.Count}", true);
      builder.AddField("Users", $"{numUsers}", true);
      builder.AddField("Bots", $"{numBots}", true);
      builder.AddField("Total", $"{context.Guild.MemberCount}", true);
      builder.WithTimestamp(DateTime.UtcNow);
      builder.WithFooter("Created by SnowyStarfall - Snowy#0364", context.Client.CurrentUser.GetAvatarUrl());
      await Context.Channel.SendMessageAsync(null, false, builder.Build()).ConfigureAwait(false);
    }
    [Command("Mod")]
    public async Task Mod([Remainder] string query = null)
    {
      if (query == null)
      {
        System.Net.WebClient wc = new System.Net.WebClient();
        string webData = wc.DownloadString("http://javid.ddns.net/tModLoader/tools/modinfo.php?modname=ProvidenceMod");
        webData = webData.Remove(0, 1);
        webData = webData.Remove(webData.Length - 1, 1);
        webData = webData.Replace("\"", "");
        string[] arr = webData.Split(",");
        for (int i = 0; i < arr.Length; i++)
        {
          int index = arr[i].IndexOf(":");
          arr[i] = arr[i].Remove(0, index + 1);
        }

        arr[7] = arr[7].Remove(10);


        EmbedBuilder builder = new EmbedBuilder();
        builder.WithAuthor($"{arr[3]}");
        builder.WithTitle($"{arr[0]}");
        builder.WithUrl("http://javid.ddns.net/tModLoader/download.php?Down=mods/ProvidenceMod.tmod");
        builder.WithThumbnailUrl("https://github.com/ProvidenceMod/ProvidenceMod/blob/main/icon.png?raw=true");
        builder.WithDescription("A remorseful story about an unforgiving world.");
        builder.WithColor(new Color(0xac4554));
        builder.AddField("Discord", "https://discord.gg/bhCuppRdVF", true);
        builder.AddField("GitHub", "https://github.com/ProvidenceMod/ProvidenceMod ", true);
        builder.AddField("Wiki", "https://providencemod.fandom.com/", true);
        builder.AddField("Versions", $"Mod - {arr[2]}\n tMod - {arr[8]}\nLast Update - {arr[7]}", true);
        builder.AddField("Downloads", $"Today - {arr[6]}\nAll Time - {arr[5]}", true);
        builder.WithTimestamp(DateTime.UtcNow);
        builder.WithFooter("Bot created by SnowyStarfall - Snowy#0364", "https://cdn.discordapp.com/attachments/601939916728827915/903417708534706206/shady_and_crystal_vampires_cropped_for_bot.png");
        await Context.Channel.SendMessageAsync(null, false, builder.Build()).ConfigureAwait(false);
      }
      else
      {
        System.Net.WebClient wc = new System.Net.WebClient();
        string webData = wc.DownloadString($"http://javid.ddns.net/tModLoader/tools/modinfo.php?modname={query}");
        if (webData == "Failed: no mod of that modname")
        {
          await Context.Channel.SendMessageAsync("Failed: no mod of that modname").ConfigureAwait(false);
          return;
        }
        webData = webData.Remove(0, 1);
        webData = webData.Remove(webData.Length - 1, 1);
        webData = webData.Replace("\"", "");
        string[] arr = webData.Split(",");
        for (int i = 0; i < arr.Length; i++)
        {
          int index = arr[i].IndexOf(":");
          arr[i] = arr[i].Remove(0, index + 1);
        }

        arr[7] = arr[7].Remove(10);
        arr[4] = arr[4].Replace("\\", "");
        try
        {
          EmbedBuilder builder = new EmbedBuilder();
          builder.WithAuthor($"{arr[3]}");
          builder.WithTitle($"{arr[0]}");
          builder.WithUrl($"{arr[4]}");
          builder.WithColor(new Color(0xac4554));
          builder.AddField("Versions", $"Mod - {arr[2]}\n tMod - {arr[8]}\nLast Update - {arr[7]}", true);
          builder.AddField("Downloads", $"Today - {arr[6]}\nAll Time - {arr[5]}", true);
          builder.WithTimestamp(DateTime.UtcNow);
          builder.WithFooter("Bot created by SnowyStarfall - Snowy#0364", "https://cdn.discordapp.com/attachments/601939916728827915/903417708534706206/shady_and_crystal_vampires_cropped_for_bot.png");
          await Context.Channel.SendMessageAsync(null, false, builder.Build()).ConfigureAwait(false);
        }
        catch(Exception ex)
        {
          Console.WriteLine(ex);
        }
      }
    }
    [Command("Discord")]
    [Alias(new string[] { "dis", "invite", "inv" })]
    public async Task Discord()
    {
      EmbedBuilder builder = new EmbedBuilder();
      builder.WithAuthor($"{arr[3]}");
      builder.WithTitle($"{arr[0]}");
      builder.WithUrl("http://javid.ddns.net/tModLoader/download.php?Down=mods/ProvidenceMod.tmod");
      builder.WithThumbnailUrl("https://github.com/ProvidenceMod/ProvidenceMod/blob/main/icon.png?raw=true");
      builder.WithDescription("A remorseful story about an unforgiving world.");
      builder.WithColor(new Color(0xac4554));
      builder.AddField("Discord", "https://discord.gg/bhCuppRdVF", true);
      builder.AddField("GitHub", "https://github.com/ProvidenceMod/ProvidenceMod ", true);
      builder.AddField("Wiki", "https://providencemod.fandom.com/", true);
    }
    [Command("GitHub")]
    [Alias(new string[] { "git", "code" })]
    [Command("Wiki")]
    [Alias(new string[] { "wik", "wikipedia" })]
  }
}
