using Discord;
using Discord.Commands;
using Discord.Webhook;
using SnowyBot.Services;
using System.Threading.Tasks;

namespace SnowyBot.Modules
{
  public class TestModule : ModuleBase
  {
    [Command("webhook")]
    [RequireOwner]
    public async Task Webook()
    {
      DiscordWebhookClient webClient = new DiscordWebhookClient("https://discord.com/api/webhooks/857531357119250444/ZWhK5i0vpAt5BP1jrps7jPIMmaWBrFm2EzhqLcooSgcSzEwb2gxaKzrFtHpK51BaKMCZ");
      await webClient.SendMessageAsync("Hello!", false, null, "Not Short", "https://cdn.discordapp.com/attachments/474886726343458816/897224772965003274/unknown.png").ConfigureAwait(false);
    }
  }
}
