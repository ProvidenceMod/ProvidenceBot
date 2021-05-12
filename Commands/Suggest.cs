using System;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Interactivity;
using DSharpPlus;

namespace ProvidenceBot.Commands
{
  public class Suggest : BaseCommandModule
  {
    public BaseDiscordClient Client { get; set; }

    [Command("suggest")]
    [Description("Suggests something in the #suggestions channel. Step by step for readability!")]
    public async Task Suggestion(CommandContext context)
    {
      // DiscordEmoji starEmoji = DiscordEmoji.FromName(Client, ":star:");
      // DiscordEmoji checkEmoji = DiscordEmoji.FromGuildEmote(Client, 841964605445898261);
      // DiscordEmoji crossEmoji = DiscordEmoji.FromGuildEmote(Client, 841964658743312394);
      if (context.Channel.Name != "suggestions")
      {
        await context.Channel.SendMessageAsync("Wrong channel! Please go to the suggestions channel!").ConfigureAwait(false);
      }
      else
      {
        DiscordMessage commandMesssage = context.Message;
        ulong authorID = context.Message.Author.Id;
        await commandMesssage.DeleteAsync().ConfigureAwait(false);

        DiscordEmbedBuilder mainCategoryBuilder = new DiscordEmbedBuilder();
        mainCategoryBuilder.WithTitle("Please choose the type of suggestion you will be making:");
        mainCategoryBuilder.AddField("1. GUI", "User Interface");
        mainCategoryBuilder.AddField("2. Feature", "Feature or Change");
        mainCategoryBuilder.AddField("3. Entity", "Items, NPCs, Projectiles, Tiles, etc");
        mainCategoryBuilder.AddField("4. Mechanic", "Combat Mechanics, Player Mechanics, Enemy Mechanics, etc");
        mainCategoryBuilder.AddField("5. Worldgen", "Biomes, Subworlds, Structures, etc");

        DiscordMessage mainCategoryQuestion = await context.Channel.SendMessageAsync(mainCategoryBuilder.Build()).ConfigureAwait(false);
        InteractivityExtension mainCategoryInteractivity = context.Client.GetInteractivity();
        InteractivityResult<DiscordMessage> mainCategoryResponse = await mainCategoryInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
        DiscordMessage mainCategoryMessage = mainCategoryResponse.Result;

        int mResult = int.Parse(mainCategoryMessage.Content);
        if (mResult != 1 && mResult != 2 && mResult != 3 && mResult != 4 && mResult != 5)
        {
          await context.Channel.SendMessageAsync("Please enter a valid option.").ConfigureAwait(false);
        }
        else
        {
          await mainCategoryQuestion.DeleteAsync().ConfigureAwait(false);
          await mainCategoryMessage.DeleteAsync().ConfigureAwait(false);
          string mainCategoryReply = mResult == 1 ? "GUI" :
                                     mResult == 2 ? "Feature" :
                                     mResult == 3 ? "Entity" :
                                     mResult == 4 ? "Mechanic" :
                                     mResult == 5 ? "Worldgen" :
                                     "Error: Undefined";
          switch (mResult)
          {
            case 1:
              break;
            case 2:
              break;
            case 3:
              DiscordEmbedBuilder entityCategoryBuilder = new DiscordEmbedBuilder();
              entityCategoryBuilder.WithTitle("Please choose the type of entity you will be suggesting.");
              entityCategoryBuilder.AddField("1. Item", "Accessories, Weapons, Potions, etc");
              entityCategoryBuilder.AddField("2. NPC", "Critters, Town NPCs, Miniboss NPCs, etc");
              entityCategoryBuilder.AddField("3. Projectile", "Boss Projectiles, Environmental Projectiles, Weapon Projectiles, etc");
              entityCategoryBuilder.AddField("4. Tile", "Building Tiles, Worldgen Tiles, Utility Tiles, etc");

              DiscordMessage entityCategoryQuestion = await context.Channel.SendMessageAsync(entityCategoryBuilder.Build()).ConfigureAwait(false);
              InteractivityExtension entityCategoryInteractivity = context.Client.GetInteractivity();
              InteractivityResult<DiscordMessage> entityCategoryResponse = await entityCategoryInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
              DiscordMessage entityCategoryMessage = entityCategoryResponse.Result;

              int eResult = int.Parse(entityCategoryMessage.Content);
              if (eResult != 1 && eResult != 2 && eResult != 3 && eResult != 4)
              {
                await context.Channel.SendMessageAsync("Please enter a valid option.").ConfigureAwait(false);
              }
              else
              {
                await entityCategoryQuestion.DeleteAsync().ConfigureAwait(false);
                await entityCategoryMessage.DeleteAsync().ConfigureAwait(false);
                string entityCategoryReply = eResult == 1 ? "Item" :
                                             eResult == 2 ? "NPC" :
                                             eResult == 3 ? "Projectile" :
                                             eResult == 4 ? "Tile" :
                                             "Error: Undefined";
                DiscordEmbedBuilder entityGamestageBuilder = new DiscordEmbedBuilder();
                entityGamestageBuilder.WithTitle("Please choose the gamestage that this entity should appear during.");
                entityGamestageBuilder.AddField("1. Pre-bosses", "Before any boss is fought, this entity will be present for the entire playthrough");
                entityGamestageBuilder.AddField("2. Pre-Hardmode", "Before entering Hardmode");
                entityGamestageBuilder.AddField("3. Pre-mechanical bosses", "Before fighting the mechanical bosses");
                entityGamestageBuilder.AddField("4. Pre-Plantera", "Before fighting Plantera");
                entityGamestageBuilder.AddField("5. Pre-Golem", "Before fighting Golem");
                entityGamestageBuilder.AddField("6. Pre-Lunar Events", "Before initiating the Lunar Events");
                entityGamestageBuilder.AddField("7. Post-Moonlord", "After killing Moonlord");

                DiscordMessage entityGamestageQuestion = await context.Channel.SendMessageAsync(entityGamestageBuilder.Build()).ConfigureAwait(false);
                InteractivityExtension entityGamestageInteractivity = context.Client.GetInteractivity();
                InteractivityResult<DiscordMessage> entityGamestageResponse = await entityGamestageInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                DiscordMessage entityGamestageMessage = entityGamestageResponse.Result;

                int egResult = int.Parse(entityCategoryMessage.Content);
                if (egResult != 1 && egResult != 2 && egResult != 3 && egResult != 4 && egResult != 5 && egResult != 6 && egResult != 7)
                {
                  await context.Channel.SendMessageAsync("Please enter a valid option.").ConfigureAwait(false);
                }
                else
                {
                  await entityGamestageQuestion.DeleteAsync().ConfigureAwait(false);
                  await entityGamestageMessage.DeleteAsync().ConfigureAwait(false);
                  string entityGamestageReply = mResult == 1 ? "Pre-bosses" :
                                                mResult == 2 ? "Pre-Hardmode" :
                                                mResult == 3 ? "Pre-mechanical bosses" :
                                                mResult == 4 ? "Pre-Plantera" :
                                                mResult == 5 ? "Pre-Golem" :
                                                mResult == 6 ? "Pre-Lunar Events" :
                                                mResult == 7 ? "Post-Moonlord" :
                                                "Error: Undefined";
                  DiscordEmbedBuilder itemCategoryBuilder = new DiscordEmbedBuilder();
                  itemCategoryBuilder.WithTitle("Please choose the type of item that this suggestion will be.");
                  itemCategoryBuilder.AddField("1. Accessory", "Equippable Accessories");
                  itemCategoryBuilder.AddField("2. Ammo", "Ammo for Guns");
                  itemCategoryBuilder.AddField("3. Armor", "Armor for Players");
                  itemCategoryBuilder.AddField("4. Consumable", "Consumables like Life Fruits, food, and items that can be \"Used-up on use\"");
                  itemCategoryBuilder.AddField("5. Material", "Items used purely for crafting");
                  itemCategoryBuilder.AddField("6. Placeable", "Placeable items, that create Tiles");
                  itemCategoryBuilder.AddField("7. Potion", "Potions that Buff or Debuff the Player or targeted NPC");
                  itemCategoryBuilder.AddField("8. Tool", "Tools that the Player can use");
                  itemCategoryBuilder.AddField("9. Weapon", "Weapons that the Player can use");

                  DiscordMessage itemCategoryQuestion = await context.Channel.SendMessageAsync(itemCategoryBuilder.Build()).ConfigureAwait(false);
                  InteractivityExtension itemCategoryInteractivity = context.Client.GetInteractivity();
                  InteractivityResult<DiscordMessage> itemCategoryResponse = await itemCategoryInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                  DiscordMessage itemCategoryMessage = itemCategoryResponse.Result;
                  int icResult = int.Parse(entityCategoryMessage.Content);
                  if (icResult != 1 && icResult != 2 && icResult != 3 && icResult != 4 && icResult != 5 && icResult != 6 && icResult != 7 && icResult != 8 && icResult != 9)
                  {
                    await context.Channel.SendMessageAsync("Please enter a valid option.").ConfigureAwait(false);
                  }
                  else
                  {
                    await itemCategoryQuestion.DeleteAsync().ConfigureAwait(false);
                    await itemCategoryMessage.DeleteAsync().ConfigureAwait(false);
                    string itemCategoryReply = icResult == 1 ? "Accessory" :
                                               icResult == 2 ? "Ammo" :
                                               icResult == 3 ? "Armor" :
                                               icResult == 4 ? "Consumable" :
                                               icResult == 5 ? "Material" :
                                               icResult == 6 ? "Placeable" :
                                               icResult == 7 ? "Potion" :
                                               icResult == 8 ? "Tool" :
                                               icResult == 9 ? "Weapon" :
                                               "Error: Undefined";
                    switch (icResult)
                    {
                      case 1: // Accessory
                        DiscordMessage accessoryNameQuestion = await context.Channel.SendMessageAsync("Please enter the name of this accessory:").ConfigureAwait(false);
                        InteractivityExtension accessoryNameInteractivity = context.Client.GetInteractivity();
                        InteractivityResult<DiscordMessage> accessoryNameResponse = await accessoryNameInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                        DiscordMessage accessoryNameMessage = accessoryNameResponse.Result;
                        await accessoryNameQuestion.DeleteAsync().ConfigureAwait(false);
                        await accessoryNameMessage.DeleteAsync().ConfigureAwait(false);
                        DiscordMessage accessoryDescriptionQuestion = await context.Channel.SendMessageAsync("Please provide a clear and concise description of this accessory:").ConfigureAwait(false);
                        InteractivityExtension accessoryDescriptionInteractivity = context.Client.GetInteractivity();
                        InteractivityResult<DiscordMessage> accessoryDescriptionResponse = await accessoryDescriptionInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                        DiscordMessage accessoryDescriptionMessage = accessoryDescriptionResponse.Result;
                        DiscordMessage finalResult = await context.Channel.SendMessageAsync($"\" {accessoryNameMessage.Content} \" | {itemCategoryReply} | {entityGamestageReply} | {entityCategoryReply} | {mainCategoryReply} | {context.User.Mention} | {DateTime.Now.Day} / {DateTime.Now.Month} / {DateTime.Now.Year} | #{0}\n{accessoryDescriptionMessage.Content}").ConfigureAwait(false);
                        // await finalResult.CreateReactionAsync(starEmoji).ConfigureAwait(false);
                        // await finalResult.CreateReactionAsync(checkEmoji).ConfigureAwait(false);
                        // await finalResult.CreateReactionAsync(crossEmoji).ConfigureAwait(false);
                        await accessoryDescriptionQuestion.DeleteAsync().ConfigureAwait(false);
                        await accessoryDescriptionMessage.DeleteAsync().ConfigureAwait(false);
                        break;
                      case 2:
                        break;
                      case 3:
                        break;
                      case 4:
                        break;
                      case 5:
                        break;
                      case 6:
                        break;
                      case 7:
                        break;
                      case 8:
                        break;
                      case 9:
                        break;
                    }
                  }
                }
              }
              break;
            case 4:
              break;
            case 5:
              break;
          }
        }
      }
    }
  }
}