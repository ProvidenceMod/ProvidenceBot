using System;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Interactivity;
using DSharpPlus;
using System.Drawing;

namespace ProvidenceBot.Commands
{
  public class Suggest : BaseCommandModule
  {
    public DiscordMessage validOptionWarn;
    [Command("suggest")]
    [Description("Suggests something in the #suggestions channel. Step by step for readability.")]
    public async Task Suggestion(CommandContext context)
    {
      DiscordColor color = new DiscordColor("651f33"); // Embed Color

      //DiscordEmoji starEmoji = DiscordEmoji.FromName(context.Client, ":star:");
      //DiscordEmoji checkEmoji = DiscordEmoji.FromGuildEmote(context.Client, 841964605445898261);
      //DiscordEmoji crossEmoji = DiscordEmoji.FromGuildEmote(context.Client, 841964658743312394);

      if (context.Channel.Name != "suggestions") // Make sure this is the right channel
      {
        await context.Channel.SendMessageAsync("Wrong channel! Please go to the suggestions channel!").ConfigureAwait(false);
      }
      else
      {
        DiscordMessage commandMesssage = context.Message;
        ulong authorID = context.Message.Author.Id;
        await commandMesssage.DeleteAsync().ConfigureAwait(false);

        //-----MAIN CATEGORY-----//

        DiscordEmbedBuilder suggestionBuilder = new DiscordEmbedBuilder();
        suggestionBuilder.WithTitle("Please choose the type of suggestion you will be making:");
        suggestionBuilder.WithColor(color);
        suggestionBuilder.AddField("1. GUI", "User Interface");
        suggestionBuilder.AddField("2. Feature", "Feature or Change");
        suggestionBuilder.AddField("3. Entity", "Items, NPCs, Projectiles, Tiles, etc");
        suggestionBuilder.AddField("4. Mechanic", "Combat Mechanics, Player Mechanics, Enemy Mechanics, etc");
        suggestionBuilder.AddField("5. Worldgen", "Biomes, Subworlds, Structures, etc");

        DiscordMessage mainCategoryQuestion = await context.Channel.SendMessageAsync(suggestionBuilder.Build()).ConfigureAwait(false);
        InteractivityExtension mainCategoryInteractivity = context.Client.GetInteractivity();
        InteractivityResult<DiscordMessage> mainCategoryResponse = await mainCategoryInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
        DiscordMessage mainCategoryMessage = mainCategoryResponse.Result;
        // await mainCategoryQuestion.DeleteAsync().ConfigureAwait(false);
        await mainCategoryMessage.DeleteAsync().ConfigureAwait(false);

        int mResult = int.Parse(mainCategoryMessage.Content);
        if (mResult != 1 && mResult != 2 && mResult != 3 && mResult != 4 && mResult != 5)
        {
          var task = Task.Run(() => Timeout(context));
          {
            await Task.Delay(5000).ConfigureAwait(false);
          }
          if (task.Status == TaskStatus.RanToCompletion)
          {
            await validOptionWarn.DeleteAsync().ConfigureAwait(false);
          }
        }
        else
        {
          string mainCategoryReply = mResult == 1 ? "GUI" :
                                     mResult == 2 ? "Feature" :
                                     mResult == 3 ? "Entity" :
                                     mResult == 4 ? "Mechanic" :
                                     mResult == 5 ? "Worldgen" :
                                     "Error: Undefined";
          switch (mResult) // Main Category Result
          {
            case 1: // If it's a GUI
              DiscordMessage guiNameQuestion = await context.Channel.SendMessageAsync("Please enter the name of this GUI:").ConfigureAwait(false);
              InteractivityExtension guiNameInteractivity = context.Client.GetInteractivity();
              InteractivityResult<DiscordMessage> guiNameResponse = await guiNameInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
              DiscordMessage guiNameMessage = guiNameResponse.Result;
              await guiNameQuestion.DeleteAsync().ConfigureAwait(false);
              await guiNameMessage.DeleteAsync().ConfigureAwait(false);

              DiscordMessage guiDescriptionQuestion = await context.Channel.SendMessageAsync("Please provide a clear and concise description of this GUI and its purpose:").ConfigureAwait(false);
              InteractivityExtension guiDescriptionInteractivity = context.Client.GetInteractivity();
              InteractivityResult<DiscordMessage> guiDescriptionResponse = await guiDescriptionInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
              DiscordMessage guiDescriptionMessage = guiDescriptionResponse.Result;
              await guiDescriptionQuestion.DeleteAsync().ConfigureAwait(false);
              await guiDescriptionMessage.DeleteAsync().ConfigureAwait(false);

              DiscordMessage finalGUIResult = await context.Channel.SendMessageAsync($"{guiNameMessage.Content} | {context.User.Mention} | {DateTime.Now.Day} / {DateTime.Now.Month} / {DateTime.Now.Year} | #{0}\n{guiDescriptionMessage.Content}").ConfigureAwait(false);
              break;
            case 2: // If it's a Feature
              suggestionBuilder.ClearFields();
              suggestionBuilder.WithTitle("Please choose the type of feature you will be suggesting:");
              suggestionBuilder.WithColor(color);
              suggestionBuilder.AddField("1. Recipe", "Recipes for items");
              suggestionBuilder.AddField("2. Environmental Change", "Ambience, design, dusts, effects, etc");
              suggestionBuilder.AddField("3. NPC Change", "AI, health, attack patterns, etc");
              suggestionBuilder.AddField("4. Player Change", "Effects, visuals, abilities, etc");
              suggestionBuilder.AddField("5. Hazard Addition", "Dangers encountered in general or in certain biomes");
              suggestionBuilder.AddField("6. Modifier", "Difficulty modifiers, recreational modifiers, challenge modifiers");
              await mainCategoryQuestion.ModifyAsync(msg => msg.Embed = suggestionBuilder.Build()).ConfigureAwait(false);

              int fResult = int.Parse(mainCategoryMessage.Content);
              if (fResult != 1 && fResult != 2 && fResult != 3 && fResult != 4 && fResult != 5 && fResult != 6)
              {
                var task = Task.Run(() => Timeout(context));
                {
                  await Task.Delay(5000).ConfigureAwait(false);
                }
                if (task.Status == TaskStatus.RanToCompletion)
                {
                  await validOptionWarn.DeleteAsync().ConfigureAwait(false);
                }
              }
              else
              {
                string featureCategoryReply = mResult == 1 ? "Recipe" :
                                       mResult == 2 ? "Environmental Change" :
                                       mResult == 3 ? "NPC Change" :
                                       mResult == 4 ? "Player Change" :
                                       mResult == 5 ? "Hazard Addition" :
                                       mResult == 6 ? "Modifier" :
                                       "Error: Undefined";
                DiscordMessage featureNameQuestion = await context.Channel.SendMessageAsync("Please enter the name of this feature:").ConfigureAwait(false);
                InteractivityExtension featureNameInteractivity = context.Client.GetInteractivity();
                InteractivityResult<DiscordMessage> featureNameResponse = await featureNameInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                DiscordMessage featureNameMessage = featureNameResponse.Result;
                await featureNameQuestion.DeleteAsync().ConfigureAwait(false);
                await featureNameMessage.DeleteAsync().ConfigureAwait(false);

                DiscordMessage featureDescriptionQuestion = await context.Channel.SendMessageAsync("Please provide a clear and concise description of this feature:").ConfigureAwait(false);
                InteractivityExtension featureDescriptionInteractivity = context.Client.GetInteractivity();
                InteractivityResult<DiscordMessage> featureDescriptionResponse = await featureDescriptionInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                DiscordMessage featureDescriptionMessage = featureDescriptionResponse.Result;
                await featureDescriptionQuestion.DeleteAsync().ConfigureAwait(false);
                await featureDescriptionMessage.DeleteAsync().ConfigureAwait(false);

                DiscordMessage finalFeatureResult = await context.Channel.SendMessageAsync($"{featureNameMessage.Content} | {featureCategoryReply} | {context.User.Mention} | {DateTime.Now.Day} / {DateTime.Now.Month} / {DateTime.Now.Year} | #{0}\n{featureDescriptionMessage.Content}").ConfigureAwait(false);
              }
              break;
            case 3: // If it's an Entity

              //-----ENTITY CATEGORY-----//

              // DiscordEmbedBuilder entityCategoryBuilder = new DiscordEmbedBuilder();
              suggestionBuilder.ClearFields();
              suggestionBuilder.WithTitle("Please choose the type of entity you will be suggesting:");
              suggestionBuilder.WithColor(color);
              suggestionBuilder.AddField("1. Item", "Accessories, Weapons, Potions, etc");
              suggestionBuilder.AddField("2. NPC", "Critters, Town NPCs, Miniboss NPCs, etc");
              suggestionBuilder.AddField("3. Projectile", "Boss Projectiles, Environmental Projectiles, Weapon Projectiles, etc");
              suggestionBuilder.AddField("4. Tile", "Building Tiles, Worldgen Tiles, Utility Tiles, etc");
              await mainCategoryQuestion.ModifyAsync(msg => msg.Embed = suggestionBuilder.Build()).ConfigureAwait(false);

              // DiscordMessage entityCategoryQuestion = await context.Channel.SendMessageAsync(suggestionBuilder.Build()).ConfigureAwait(false);
              InteractivityExtension entityCategoryInteractivity = context.Client.GetInteractivity();
              InteractivityResult<DiscordMessage> entityCategoryResponse = await entityCategoryInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
              DiscordMessage entityCategoryMessage = entityCategoryResponse.Result;
              // await entityCategoryQuestion.DeleteAsync().ConfigureAwait(false);
              await entityCategoryMessage.DeleteAsync().ConfigureAwait(false);

              int eResult = int.Parse(entityCategoryMessage.Content);
              if (eResult != 1 && eResult != 2 && eResult != 3 && eResult != 4)
              {
                var task = Task.Run(() => Timeout(context));
                {
                  await Task.Delay(5000).ConfigureAwait(false);
                }
                if (task.Status == TaskStatus.RanToCompletion)
                {
                  await validOptionWarn.DeleteAsync().ConfigureAwait(false);
                }
              }
              else
              {
                string entityCategoryReply = eResult == 1 ? "Item" :
                                             eResult == 2 ? "NPC" :
                                             eResult == 3 ? "Projectile" :
                                             eResult == 4 ? "Tile" :
                                             "Error: Undefined";

                //-----GAMESTAGE-----//

                // DiscordEmbedBuilder entityGamestageBuilder = new DiscordEmbedBuilder();
                suggestionBuilder.ClearFields();
                suggestionBuilder.WithTitle("Please choose the gamestage that this entity should appear during:");
                suggestionBuilder.WithColor(color);
                suggestionBuilder.AddField("1. Pre-bosses", "Before any boss is fought, this entity will be present for the entire playthrough");
                suggestionBuilder.AddField("2. Pre-Hardmode", "Before entering Hardmode");
                suggestionBuilder.AddField("3. Pre-mechanical bosses", "Before fighting the mechanical bosses");
                suggestionBuilder.AddField("4. Pre-Plantera", "Before fighting Plantera");
                suggestionBuilder.AddField("5. Pre-Golem", "Before fighting Golem");
                suggestionBuilder.AddField("6. Pre-Lunar Events", "Before initiating the Lunar Events");
                suggestionBuilder.AddField("7. Post-Moonlord", "After killing Moonlord");
                await mainCategoryQuestion.ModifyAsync(msg => msg.Embed = suggestionBuilder.Build()).ConfigureAwait(false);

                // DiscordMessage entityGamestageQuestion = await context.Channel.SendMessageAsync(entityGamestageBuilder.Build()).ConfigureAwait(false);
                InteractivityExtension entityGamestageInteractivity = context.Client.GetInteractivity();
                InteractivityResult<DiscordMessage> entityGamestageResponse = await entityGamestageInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                DiscordMessage entityGamestageMessage = entityGamestageResponse.Result;
                // await entityGamestageQuestion.DeleteAsync().ConfigureAwait(false);
                await entityGamestageMessage.DeleteAsync().ConfigureAwait(false);

                int egResult = int.Parse(entityCategoryMessage.Content);
                if (egResult != 1 && egResult != 2 && egResult != 3 && egResult != 4 && egResult != 5 && egResult != 6 && egResult != 7)
                {
                  var task = Task.Run(() => Timeout(context));
                  {
                    await Task.Delay(5000).ConfigureAwait(false);
                  }
                  if (task.Status == TaskStatus.RanToCompletion)
                  {
                    await validOptionWarn.DeleteAsync().ConfigureAwait(false);
                  }
                }
                else
                {
                  string entityGamestageReply = mResult == 1 ? "Pre-bosses" :
                                                mResult == 2 ? "Pre-Hardmode" :
                                                mResult == 3 ? "Pre-mechanical bosses" :
                                                mResult == 4 ? "Pre-Plantera" :
                                                mResult == 5 ? "Pre-Golem" :
                                                mResult == 6 ? "Pre-Lunar Events" :
                                                mResult == 7 ? "Post-Moonlord" :
                                                "Error: Undefined";
                  switch (eResult) // Entity Type Result
                  {
                    case 1:
                      //-----ITEM CATEGORY-----//

                      // DiscordEmbedBuilder itemCategoryBuilder = new DiscordEmbedBuilder();
                      suggestionBuilder.ClearFields();
                      suggestionBuilder.WithTitle("Please choose the type of item that this suggestion will be:");
                      suggestionBuilder.WithColor(color);
                      suggestionBuilder.AddField("1. Accessory", "Equippable Accessories");
                      suggestionBuilder.AddField("2. Ammo", "Ammo for Guns");
                      suggestionBuilder.AddField("3. Armor", "Armor for Players");
                      suggestionBuilder.AddField("4. Consumable", "consumable like Life Fruits, food, and items that can be \"Used-up on use\"");
                      suggestionBuilder.AddField("5. Material", "Items used purely for crafting");
                      suggestionBuilder.AddField("6. Placeable", "Placeable items, that create Tiles");
                      suggestionBuilder.AddField("7. Potion", "Potions that Buff or Debuff the Player or targeted NPC");
                      suggestionBuilder.AddField("8. Tool", "Tools that the Player can use");
                      suggestionBuilder.AddField("9. Weapon", "Weapons that the Player can use");
                      await mainCategoryQuestion.ModifyAsync(msg => msg.Embed = suggestionBuilder.Build()).ConfigureAwait(false);

                      // DiscordMessage itemCategoryQuestion = await context.Channel.SendMessageAsync(itemCategoryBuilder.Build()).ConfigureAwait(false);
                      InteractivityExtension itemCategoryInteractivity = context.Client.GetInteractivity();
                      InteractivityResult<DiscordMessage> itemCategoryResponse = await itemCategoryInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                      DiscordMessage itemCategoryMessage = itemCategoryResponse.Result;
                      // await itemCategoryQuestion.DeleteAsync().ConfigureAwait(false);
                      await itemCategoryMessage.DeleteAsync().ConfigureAwait(false);

                      int icResult = int.Parse(entityCategoryMessage.Content);
                      if (icResult != 1 && icResult != 2 && icResult != 3 && icResult != 4 && icResult != 5 && icResult != 6 && icResult != 7 && icResult != 8 && icResult != 9)
                      {
                        var task = Task.Run(() => Timeout(context));
                        {
                          await Task.Delay(5000).ConfigureAwait(false);
                        }
                        if (task.Status == TaskStatus.RanToCompletion)
                        {
                          await validOptionWarn.DeleteAsync().ConfigureAwait(false);
                        }
                      }
                      else
                      {
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
                        switch (icResult) // Item Category Result
                        {
                          case 1: // Accessory

                            //-----ACCESSORY CATEGORY-----//

                            // DiscordEmbedBuilder accessoryClassBuilder = new DiscordEmbedBuilder();
                            suggestionBuilder.ClearFields();
                            suggestionBuilder.WithTitle("Please choose which class this accessory should be aimed at:");
                            suggestionBuilder.WithColor(color);
                            suggestionBuilder.AddField("1. Melee", "Suitable for Melee builds");
                            suggestionBuilder.AddField("2. Ranged", "Suitable for Ranged builds");
                            suggestionBuilder.AddField("3. Magic", "Suitable for Mage builds");
                            suggestionBuilder.AddField("4. Summoner", "Suitable for Summoner builds");
                            suggestionBuilder.AddField("5. Cleric", "Suitable for CLeric builds");
                            suggestionBuilder.AddField("6. General", "Suitable for every class");
                            await mainCategoryQuestion.ModifyAsync(msg => msg.Embed = suggestionBuilder.Build()).ConfigureAwait(false);

                            // DiscordMessage accessoryClassQuestion = await context.Channel.SendMessageAsync(accessoryClassBuilder.Build()).ConfigureAwait(false);
                            InteractivityExtension accessoryClassInteractivity = context.Client.GetInteractivity();
                            InteractivityResult<DiscordMessage> accessoryClassResponse = await accessoryClassInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                            DiscordMessage accessoryClassMessage = accessoryClassResponse.Result;
                            // await accessoryClassQuestion.DeleteAsync().ConfigureAwait(false);
                            await accessoryClassMessage.DeleteAsync().ConfigureAwait(false);

                            int acResult = int.Parse(accessoryClassMessage.Content);
                            if (acResult != 1 && acResult != 2 && acResult != 3 && acResult != 4 && acResult != 5 && acResult != 6)
                            {
                              var task = Task.Run(() => Timeout(context));
                              {
                                await Task.Delay(5000).ConfigureAwait(false);
                              }
                              if (task.Status == TaskStatus.RanToCompletion)
                              {
                                await validOptionWarn.DeleteAsync().ConfigureAwait(false);
                              }
                            }
                            else
                            {
                              string accessoryClassReply = acResult == 1 ? "Melee" :
                                                           acResult == 2 ? "Ranged" :
                                                           acResult == 3 ? "Magic" :
                                                           acResult == 4 ? "Summoner" :
                                                           acResult == 5 ? "Cleric" :
                                                           acResult == 6 ? "General" :
                                                           "Error: Undefined";

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
                              await accessoryDescriptionQuestion.DeleteAsync().ConfigureAwait(false);
                              await accessoryDescriptionMessage.DeleteAsync().ConfigureAwait(false);

                              DiscordMessage finalAccessoryResult = await context.Channel.SendMessageAsync($"{accessoryNameMessage.Content} | {accessoryClassReply} | {itemCategoryReply} | {entityGamestageReply} | {entityCategoryReply} | {mainCategoryReply} | {context.User.Mention} | {DateTime.Now.Day} / {DateTime.Now.Month} / {DateTime.Now.Year} | #{0}\n{accessoryDescriptionMessage.Content}").ConfigureAwait(false);

                              // await finalResult.CreateReactionAsync(starEmoji).ConfigureAwait(false);
                              // await finalResult.CreateReactionAsync(checkEmoji).ConfigureAwait(false);
                              // await finalResult.CreateReactionAsync(crossEmoji).ConfigureAwait(false);
                            }
                            break;

                          case 2: // Ammo

                            //-----AMMO CATEGORY-----//

                            DiscordMessage ammoNameQuestion = await context.Channel.SendMessageAsync("Please enter the name of this ammo:").ConfigureAwait(false);
                            InteractivityExtension ammoNameInteractivity = context.Client.GetInteractivity();
                            InteractivityResult<DiscordMessage> ammoNameResponse = await ammoNameInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                            DiscordMessage ammoNameMessage = ammoNameResponse.Result;
                            await ammoNameQuestion.DeleteAsync().ConfigureAwait(false);
                            await ammoNameMessage.DeleteAsync().ConfigureAwait(false);

                            DiscordMessage ammoDescriptionQuestion = await context.Channel.SendMessageAsync("Please provide a clear and concise description of this ammo:").ConfigureAwait(false);
                            InteractivityExtension ammoDescriptionInteractivity = context.Client.GetInteractivity();
                            InteractivityResult<DiscordMessage> ammoDescriptionResponse = await ammoDescriptionInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                            DiscordMessage ammoDescriptionMessage = ammoDescriptionResponse.Result;
                            await ammoDescriptionQuestion.DeleteAsync().ConfigureAwait(false);
                            await ammoDescriptionMessage.DeleteAsync().ConfigureAwait(false);

                            DiscordMessage finalResult = await context.Channel.SendMessageAsync($"{ammoNameMessage.Content} | {itemCategoryReply} | {entityGamestageReply} | {entityCategoryReply} | {mainCategoryReply} | {context.User.Mention} | {DateTime.Now.Day} / {DateTime.Now.Month} / {DateTime.Now.Year} | #{0}\n{ammoDescriptionMessage.Content}").ConfigureAwait(false);
                            break;
                          case 3: // Armor

                            //-----ARMOR CATEGORY-----//

                            suggestionBuilder.ClearFields();
                            suggestionBuilder.WithTitle("Please choose which class this armor should be aimed at:");
                            suggestionBuilder.WithColor(color);
                            suggestionBuilder.AddField("1. Melee", "Suitable for Melee builds");
                            suggestionBuilder.AddField("2. Ranged", "Suitable for Ranged builds");
                            suggestionBuilder.AddField("3. Magic", "Suitable for Mage builds");
                            suggestionBuilder.AddField("4. Summoner", "Suitable for Summoner builds");
                            suggestionBuilder.AddField("5. Cleric", "Suitable for CLeric builds");
                            suggestionBuilder.AddField("6. General", "Suitable for every class");
                            await mainCategoryQuestion.ModifyAsync(msg => msg.Embed = suggestionBuilder.Build()).ConfigureAwait(false);

                            // DiscordMessage accessoryClassQuestion = await context.Channel.SendMessageAsync(accessoryClassBuilder.Build()).ConfigureAwait(false);
                            InteractivityExtension armorClassInteractivity = context.Client.GetInteractivity();
                            InteractivityResult<DiscordMessage> armorClassResponse = await armorClassInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                            DiscordMessage armorClassMessage = armorClassResponse.Result;
                            // await accessoryClassQuestion.DeleteAsync().ConfigureAwait(false);
                            await armorClassMessage.DeleteAsync().ConfigureAwait(false);

                            int arResult = int.Parse(armorClassMessage.Content);
                            if (arResult != 1 && arResult != 2 && arResult != 3 && arResult != 4 && arResult != 5 && arResult != 6)
                            {
                              var task = Task.Run(() => Timeout(context));
                              {
                                await Task.Delay(5000).ConfigureAwait(false);
                              }
                              if (task.Status == TaskStatus.RanToCompletion)
                              {
                                await validOptionWarn.DeleteAsync().ConfigureAwait(false);
                              }
                            }
                            else
                            {
                              string armorClassReply = arResult == 1 ? "Melee" :
                                                           arResult == 2 ? "Ranged" :
                                                           arResult == 3 ? "Magic" :
                                                           arResult == 4 ? "Summoner" :
                                                           arResult == 5 ? "Cleric" :
                                                           arResult == 6 ? "General" :
                                                           "Error: Undefined";

                              DiscordMessage armorNameQuestion = await context.Channel.SendMessageAsync("Please enter the name of this accessory:").ConfigureAwait(false);
                              InteractivityExtension armorNameInteractivity = context.Client.GetInteractivity();
                              InteractivityResult<DiscordMessage> armorNameResponse = await armorNameInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                              DiscordMessage armorNameMessage = armorNameResponse.Result;
                              await armorNameQuestion.DeleteAsync().ConfigureAwait(false);
                              await armorNameMessage.DeleteAsync().ConfigureAwait(false);

                              DiscordMessage armorDescriptionQuestion = await context.Channel.SendMessageAsync("Please provide a clear and concise description of this accessory:").ConfigureAwait(false);
                              InteractivityExtension armorDescriptionInteractivity = context.Client.GetInteractivity();
                              InteractivityResult<DiscordMessage> armorDescriptionResponse = await armorDescriptionInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                              DiscordMessage armorDescriptionMessage = armorDescriptionResponse.Result;
                              await armorDescriptionQuestion.DeleteAsync().ConfigureAwait(false);
                              await armorDescriptionMessage.DeleteAsync().ConfigureAwait(false);

                              DiscordMessage finalArmorResult = await context.Channel.SendMessageAsync($"{armorNameMessage.Content} | {armorClassReply} | {itemCategoryReply} | {entityGamestageReply} | {entityCategoryReply} | {mainCategoryReply} | {context.User.Mention} | {DateTime.Now.Day} / {DateTime.Now.Month} / {DateTime.Now.Year} | #{0}\n{armorDescriptionMessage.Content}").ConfigureAwait(false);
                            }
                            break;
                          case 4: // Consumable

                            //-----CONSUMABLE CATEGORY-----//

                            suggestionBuilder.ClearFields();
                            suggestionBuilder.WithTitle("Please choose which type of consumeable this is:");
                            suggestionBuilder.WithColor(color);
                            suggestionBuilder.AddField("1. Permanent Health Upgrade", "Like Life Fruits and Life Crystals");
                            suggestionBuilder.AddField("2. Permanent Mana Upgrade", "Like Mana Crystals");
                            suggestionBuilder.AddField("3. Permanent Parity Upgrade", "Permanent capacity upgrade for Order and Chaos stacks");
                            suggestionBuilder.AddField("4. Spawner", "Spawner for bosses, events, etc");
                            suggestionBuilder.AddField("5. Dropped Consumable", "Hearts, Mana Stars, etc");
                            await mainCategoryQuestion.ModifyAsync(msg => msg.Embed = suggestionBuilder.Build()).ConfigureAwait(false);

                            InteractivityExtension consumableInteractivity = context.Client.GetInteractivity();
                            InteractivityResult<DiscordMessage> consumableResponse = await consumableInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                            DiscordMessage consumableMessage = consumableResponse.Result;

                            await consumableMessage.DeleteAsync().ConfigureAwait(false);

                            int conResult = int.Parse(consumableMessage.Content);
                            if (conResult != 1 && conResult != 2 && conResult != 3 && conResult != 4 && conResult != 5)
                            {
                              var task = Task.Run(() => Timeout(context));
                              {
                                await Task.Delay(5000).ConfigureAwait(false);
                              }
                              if (task.Status == TaskStatus.RanToCompletion)
                              {
                                await validOptionWarn.DeleteAsync().ConfigureAwait(false);
                              }
                            }
                            else
                            {
                              string consumableReply = conResult == 1 ? "Permanent Health Upgrade" :
                                                       conResult == 2 ? "Permanent Mana Upgrade" :
                                                       conResult == 3 ? "Permanent Parity Upgrade" :
                                                       conResult == 4 ? "Spawner" :
                                                       conResult == 5 ? "Dropped Consumable" :
                                                           "Error: Undefined";

                              DiscordMessage consumableNameQuestion = await context.Channel.SendMessageAsync("Please enter the name of this consumable:").ConfigureAwait(false);
                              InteractivityExtension consumableNameInteractivity = context.Client.GetInteractivity();
                              InteractivityResult<DiscordMessage> consumableNameResponse = await consumableNameInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                              DiscordMessage consumableNameMessage = consumableNameResponse.Result;
                              await consumableNameQuestion.DeleteAsync().ConfigureAwait(false);
                              await consumableNameMessage.DeleteAsync().ConfigureAwait(false);

                              DiscordMessage consumableDescriptionQuestion = await context.Channel.SendMessageAsync("Please provide a clear and concise description of this consumable:").ConfigureAwait(false);
                              InteractivityExtension consumableDescriptionInteractivity = context.Client.GetInteractivity();
                              InteractivityResult<DiscordMessage> consumableDescriptionResponse = await consumableDescriptionInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                              DiscordMessage consumableDescriptionMessage = consumableDescriptionResponse.Result;
                              await consumableDescriptionQuestion.DeleteAsync().ConfigureAwait(false);
                              await consumableDescriptionMessage.DeleteAsync().ConfigureAwait(false);

                              DiscordMessage finalConsumableResult = await context.Channel.SendMessageAsync($"{consumableNameMessage.Content} | {consumableReply} | {itemCategoryReply} | {entityGamestageReply} | {entityCategoryReply} | {mainCategoryReply} | {context.User.Mention} | {DateTime.Now.Day} / {DateTime.Now.Month} / {DateTime.Now.Year} | #{0}\n{consumableDescriptionMessage.Content}").ConfigureAwait(false);
                            }
                            break;
                          case 5: // Material

                            //-----MATERIAL CATEGORY-----//

                            DiscordMessage materialNameQuestion = await context.Channel.SendMessageAsync("Please enter the name of this material:").ConfigureAwait(false);
                            InteractivityExtension materialNameInteractivity = context.Client.GetInteractivity();
                            InteractivityResult<DiscordMessage> materialNameResponse = await materialNameInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                            DiscordMessage materialNameMessage = materialNameResponse.Result;
                            await materialNameQuestion.DeleteAsync().ConfigureAwait(false);
                            await materialNameMessage.DeleteAsync().ConfigureAwait(false);

                            DiscordMessage materialDescriptionQuestion = await context.Channel.SendMessageAsync("Please provide a clear and concise description of this material:").ConfigureAwait(false);
                            InteractivityExtension materialDescriptionInteractivity = context.Client.GetInteractivity();
                            InteractivityResult<DiscordMessage> materialDescriptionResponse = await materialDescriptionInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                            DiscordMessage materialDescriptionMessage = materialDescriptionResponse.Result;
                            await materialDescriptionQuestion.DeleteAsync().ConfigureAwait(false);
                            await materialDescriptionMessage.DeleteAsync().ConfigureAwait(false);

                            DiscordMessage finalMaterialResult = await context.Channel.SendMessageAsync($"{materialNameMessage.Content} | {itemCategoryReply} | {entityGamestageReply} | {entityCategoryReply} | {mainCategoryReply} | {context.User.Mention} | {DateTime.Now.Day} / {DateTime.Now.Month} / {DateTime.Now.Year} | #{0}\n{materialDescriptionMessage.Content}").ConfigureAwait(false);
                            break;
                          case 6: // Placeable

                            //-----PLACEABLE CATEGORY-----//

                            suggestionBuilder.ClearFields();
                            suggestionBuilder.WithTitle("Please choose which type of Placeable this is:");
                            suggestionBuilder.WithColor(color);
                            suggestionBuilder.AddField("1. Decoration", "Placeables that make things look pretty");
                            suggestionBuilder.AddField("2. Lighting", "Placeables that make things easier to see");
                            suggestionBuilder.AddField("3. Utility", "Placeables that offer a practical use");
                            await mainCategoryQuestion.ModifyAsync(msg => msg.Embed = suggestionBuilder.Build()).ConfigureAwait(false);

                            InteractivityExtension placeableInteractivity = context.Client.GetInteractivity();
                            InteractivityResult<DiscordMessage> placeableResponse = await placeableInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                            DiscordMessage placeableMessage = placeableResponse.Result;

                            await placeableMessage.DeleteAsync().ConfigureAwait(false);

                            int plResult = int.Parse(placeableMessage.Content);
                            if (plResult != 1 && plResult != 2 && plResult != 3 && plResult != 4 && plResult != 5)
                            {
                              var task = Task.Run(() => Timeout(context));
                              {
                                await Task.Delay(5000).ConfigureAwait(false);
                              }
                              if (task.Status == TaskStatus.RanToCompletion)
                              {
                                await validOptionWarn.DeleteAsync().ConfigureAwait(false);
                              }
                            }
                            else
                            {
                              string placeableReply = plResult == 1 ? "Permanent Health Upgrade" :
                                                      plResult == 2 ? "Permanent Mana Upgrade" :
                                                      plResult == 3 ? "Permanent Parity Upgrade" :
                                                      plResult == 4 ? "Spawner" :
                                                      plResult == 5 ? "Dropped Consumable" :
                                                      "Error: Undefined";

                              DiscordMessage placeableNameQuestion = await context.Channel.SendMessageAsync("Please enter the name of this placeable:").ConfigureAwait(false);
                              InteractivityExtension placeableNameInteractivity = context.Client.GetInteractivity();
                              InteractivityResult<DiscordMessage> placeableNameResponse = await placeableNameInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                              DiscordMessage placeableNameMessage = placeableNameResponse.Result;
                              await placeableNameQuestion.DeleteAsync().ConfigureAwait(false);
                              await placeableNameMessage.DeleteAsync().ConfigureAwait(false);

                              DiscordMessage placeableDescriptionQuestion = await context.Channel.SendMessageAsync("Please provide a clear and concise description of this placeable:").ConfigureAwait(false);
                              InteractivityExtension placeableDescriptionInteractivity = context.Client.GetInteractivity();
                              InteractivityResult<DiscordMessage> placeableDescriptionResponse = await placeableDescriptionInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                              DiscordMessage placeableDescriptionMessage = placeableDescriptionResponse.Result;
                              await placeableDescriptionQuestion.DeleteAsync().ConfigureAwait(false);
                              await placeableDescriptionMessage.DeleteAsync().ConfigureAwait(false);

                              DiscordMessage finalPlaceableResult = await context.Channel.SendMessageAsync($"{placeableNameMessage.Content} | {placeableReply} | {itemCategoryReply} | {entityGamestageReply} | {entityCategoryReply} | {mainCategoryReply} | {context.User.Mention} | {DateTime.Now.Day} / {DateTime.Now.Month} / {DateTime.Now.Year} | #{0}\n{placeableDescriptionMessage.Content}").ConfigureAwait(false);
                            }
                            break;
                          case 7:
                            break;
                          case 8:
                            break;
                          case 9:
                            break;
                        }
                      }
                      break;
                    case 2:
                      break;
                    case 3:
                      break;
                    case 4:
                      break;
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
    public async Task Timeout(CommandContext context)
    {
      validOptionWarn = await context.Channel.SendMessageAsync("Please enter a valid option.").ConfigureAwait(false);
    }
  }
}