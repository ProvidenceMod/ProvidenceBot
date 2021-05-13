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

      DiscordEmoji starEmoji = DiscordEmoji.FromName(context.Client, ":star:");
      DiscordEmoji checkEmoji = DiscordEmoji.FromGuildEmote(context.Client, 785370805093269514);
      DiscordEmoji crossEmoji = DiscordEmoji.FromGuildEmote(context.Client, 801764305464852481);

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
              await mainCategoryQuestion.DeleteAsync().ConfigureAwait(false);

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

              DiscordMessage finalGUIResult = await context.Channel.SendMessageAsync($"{guiNameMessage.Content} | {mainCategoryReply} | {context.User.Mention} | {DateTime.Now.Day} / {DateTime.Now.Month} / {DateTime.Now.Year} | #{0}\n{guiDescriptionMessage.Content}").ConfigureAwait(false);
              await finalGUIResult.CreateReactionAsync(starEmoji).ConfigureAwait(false);
              await finalGUIResult.CreateReactionAsync(checkEmoji).ConfigureAwait(false);
              await finalGUIResult.CreateReactionAsync(crossEmoji).ConfigureAwait(false);
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

              InteractivityExtension featureCategoryInteractivity = context.Client.GetInteractivity();
              InteractivityResult<DiscordMessage> featureCategoryResponse = await featureCategoryInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
              DiscordMessage featureCategoryMessage = featureCategoryResponse.Result;
              await featureCategoryMessage.DeleteAsync().ConfigureAwait(false);

              int fResult = int.Parse(featureCategoryMessage.Content);
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
                await mainCategoryQuestion.DeleteAsync().ConfigureAwait(false);
                string featureCategoryReply = fResult == 1 ? "Recipe" :
                                              fResult == 2 ? "Environmental Change" :
                                              fResult == 3 ? "NPC Change" :
                                              fResult == 4 ? "Player Change" :
                                              fResult == 5 ? "Hazard Addition" :
                                              fResult == 6 ? "Modifier" :
                                              "Error: Undefined";

                DiscordMessage featureNameQuestion = await context.Channel.SendMessageAsync("Please enter the name of this " + (fResult == 1 ? "recipe:" : fResult == 2 ? "environmental change:" : fResult == 3 ? "NPC change:" : fResult == 4 ? "player change:" : fResult == 5 ? "hazard addition:" : fResult == 6 ? "modifier" : "Error: Undefined")).ConfigureAwait(false);
                InteractivityExtension featureNameInteractivity = context.Client.GetInteractivity();
                InteractivityResult<DiscordMessage> featureNameResponse = await featureNameInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                DiscordMessage featureNameMessage = featureNameResponse.Result;
                await featureNameMessage.DeleteAsync().ConfigureAwait(false);
                await featureNameQuestion.DeleteAsync().ConfigureAwait(false);

                DiscordMessage featureDescriptionQuestion = await context.Channel.SendMessageAsync("Please provide a clear and concise description of this " + (fResult == 1 ? "recipe:" : fResult == 2 ? "environmental change:" : fResult == 3 ? "NPC change:" : fResult == 4 ? "player change:" : fResult == 5 ? "hazard addition:" : fResult == 6 ? "modifier" : "Error: Undefined")).ConfigureAwait(false);
                InteractivityExtension featureDescriptionInteractivity = context.Client.GetInteractivity();
                InteractivityResult<DiscordMessage> featureDescriptionResponse = await featureDescriptionInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                DiscordMessage featureDescriptionMessage = featureDescriptionResponse.Result;
                await featureDescriptionMessage.DeleteAsync().ConfigureAwait(false);
                await featureDescriptionQuestion.DeleteAsync().ConfigureAwait(false);

                DiscordMessage finalFeatureResult = await context.Channel.SendMessageAsync($"{featureNameMessage.Content} | {featureCategoryReply} | {mainCategoryReply} | {context.User.Mention} | {DateTime.Now.Day} / {DateTime.Now.Month} / {DateTime.Now.Year} | #{0}\n{featureDescriptionMessage.Content}").ConfigureAwait(false);
                await finalFeatureResult.CreateReactionAsync(starEmoji).ConfigureAwait(false);
                await finalFeatureResult.CreateReactionAsync(checkEmoji).ConfigureAwait(false);
                await finalFeatureResult.CreateReactionAsync(crossEmoji).ConfigureAwait(false);
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
              suggestionBuilder.AddField("5. Dust", "Dust, spawned in environments, around the player, on weapons, etc");
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
                                             eResult == 5 ? "Dust" :
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

                int egResult = int.Parse(entityGamestageMessage.Content);
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
                  string entityGamestageReply = egResult == 1 ? "Pre-bosses" :
                                                egResult == 2 ? "Pre-Hardmode" :
                                                egResult == 3 ? "Pre-mechanical bosses" :
                                                egResult == 4 ? "Pre-Plantera" :
                                                egResult == 5 ? "Pre-Golem" :
                                                egResult == 6 ? "Pre-Lunar Events" :
                                                egResult == 7 ? "Post-Moonlord" :
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
                      suggestionBuilder.AddField("2. Ammo", "Ammo for ranged weapons");
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

                      int icResult = int.Parse(itemCategoryMessage.Content);
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

                            InteractivityExtension accessoryClassInteractivity = context.Client.GetInteractivity();
                            InteractivityResult<DiscordMessage> accessoryClassResponse = await accessoryClassInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                            DiscordMessage accessoryClassMessage = accessoryClassResponse.Result;
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
                              await mainCategoryQuestion.DeleteAsync().ConfigureAwait(false);
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

                              await finalAccessoryResult.CreateReactionAsync(starEmoji).ConfigureAwait(false);
                              await finalAccessoryResult.CreateReactionAsync(checkEmoji).ConfigureAwait(false);
                              await finalAccessoryResult.CreateReactionAsync(crossEmoji).ConfigureAwait(false);
                            }
                            break;

                          case 2: // Ammo

                            //-----AMMO CATEGORY-----//

                            await mainCategoryQuestion.DeleteAsync().ConfigureAwait(false);

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

                            DiscordMessage finalAmmoResult = await context.Channel.SendMessageAsync($"{ammoNameMessage.Content} | {itemCategoryReply} | {entityGamestageReply} | {entityCategoryReply} | {mainCategoryReply} | {context.User.Mention} | {DateTime.Now.Day} / {DateTime.Now.Month} / {DateTime.Now.Year} | #{0}\n{ammoDescriptionMessage.Content}").ConfigureAwait(false);
                            await finalAmmoResult.CreateReactionAsync(starEmoji).ConfigureAwait(false);
                            await finalAmmoResult.CreateReactionAsync(checkEmoji).ConfigureAwait(false);
                            await finalAmmoResult.CreateReactionAsync(crossEmoji).ConfigureAwait(false);
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
                              await mainCategoryQuestion.DeleteAsync().ConfigureAwait(false);
                              string armorClassReply = arResult == 1 ? "Melee" :
                                                       arResult == 2 ? "Ranged" :
                                                       arResult == 3 ? "Magic" :
                                                       arResult == 4 ? "Summoner" :
                                                       arResult == 5 ? "Cleric" :
                                                       arResult == 6 ? "General" :
                                                       "Error: Undefined";

                              DiscordMessage armorNameQuestion = await context.Channel.SendMessageAsync("Please enter the name of this armor:").ConfigureAwait(false);
                              InteractivityExtension armorNameInteractivity = context.Client.GetInteractivity();
                              InteractivityResult<DiscordMessage> armorNameResponse = await armorNameInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                              DiscordMessage armorNameMessage = armorNameResponse.Result;
                              await armorNameQuestion.DeleteAsync().ConfigureAwait(false);
                              await armorNameMessage.DeleteAsync().ConfigureAwait(false);

                              DiscordMessage armorDescriptionQuestion = await context.Channel.SendMessageAsync("Please provide a clear and concise description of this armor:").ConfigureAwait(false);
                              InteractivityExtension armorDescriptionInteractivity = context.Client.GetInteractivity();
                              InteractivityResult<DiscordMessage> armorDescriptionResponse = await armorDescriptionInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                              DiscordMessage armorDescriptionMessage = armorDescriptionResponse.Result;
                              await armorDescriptionQuestion.DeleteAsync().ConfigureAwait(false);
                              await armorDescriptionMessage.DeleteAsync().ConfigureAwait(false);

                              DiscordMessage finalArmorResult = await context.Channel.SendMessageAsync($"{armorNameMessage.Content} | {armorClassReply} | {itemCategoryReply} | {entityGamestageReply} | {entityCategoryReply} | {mainCategoryReply} | {context.User.Mention} | {DateTime.Now.Day} / {DateTime.Now.Month} / {DateTime.Now.Year} | #{0}\n{armorDescriptionMessage.Content}").ConfigureAwait(false);
                              await finalArmorResult.CreateReactionAsync(starEmoji).ConfigureAwait(false);
                              await finalArmorResult.CreateReactionAsync(checkEmoji).ConfigureAwait(false);
                              await finalArmorResult.CreateReactionAsync(crossEmoji).ConfigureAwait(false);
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
                              await mainCategoryQuestion.DeleteAsync().ConfigureAwait(false);
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
                              await finalConsumableResult.CreateReactionAsync(starEmoji).ConfigureAwait(false);
                              await finalConsumableResult.CreateReactionAsync(checkEmoji).ConfigureAwait(false);
                              await finalConsumableResult.CreateReactionAsync(crossEmoji).ConfigureAwait(false);
                            }
                            break;
                          case 5: // Material

                            //-----MATERIAL CATEGORY-----//

                            await mainCategoryQuestion.DeleteAsync().ConfigureAwait(false);

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
                            await finalMaterialResult.CreateReactionAsync(starEmoji).ConfigureAwait(false);
                            await finalMaterialResult.CreateReactionAsync(checkEmoji).ConfigureAwait(false);
                            await finalMaterialResult.CreateReactionAsync(crossEmoji).ConfigureAwait(false);
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
                              await mainCategoryQuestion.DeleteAsync().ConfigureAwait(false);
                              string placeableReply = plResult == 1 ? "Decoration" :
                                                      plResult == 2 ? "Lighting" :
                                                      plResult == 3 ? "Utility" :
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
                              await finalPlaceableResult.CreateReactionAsync(starEmoji).ConfigureAwait(false);
                              await finalPlaceableResult.CreateReactionAsync(checkEmoji).ConfigureAwait(false);
                              await finalPlaceableResult.CreateReactionAsync(crossEmoji).ConfigureAwait(false);
                            }
                            break;
                          case 7: // Potions

                            //-----POTIONS CATEGORY-----//

                            await mainCategoryQuestion.DeleteAsync().ConfigureAwait(false);

                            DiscordMessage potionNameQuestion = await context.Channel.SendMessageAsync("Please enter the name of this potion:").ConfigureAwait(false);
                            InteractivityExtension potionNameInteractivity = context.Client.GetInteractivity();
                            InteractivityResult<DiscordMessage> potionNameResponse = await potionNameInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                            DiscordMessage potionNameMessage = potionNameResponse.Result;
                            await potionNameQuestion.DeleteAsync().ConfigureAwait(false);
                            await potionNameMessage.DeleteAsync().ConfigureAwait(false);

                            DiscordMessage potionDescriptionQuestion = await context.Channel.SendMessageAsync("Please provide a clear and concise description of this potion:").ConfigureAwait(false);
                            InteractivityExtension potionDescriptionInteractivity = context.Client.GetInteractivity();
                            InteractivityResult<DiscordMessage> potionDescriptionResponse = await potionDescriptionInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                            DiscordMessage potionDescriptionMessage = potionDescriptionResponse.Result;
                            await potionDescriptionQuestion.DeleteAsync().ConfigureAwait(false);
                            await potionDescriptionMessage.DeleteAsync().ConfigureAwait(false);

                            DiscordMessage finalPotionResult = await context.Channel.SendMessageAsync($"{potionNameMessage.Content} | {itemCategoryReply} | {entityGamestageReply} | {entityCategoryReply} | {mainCategoryReply} | {context.User.Mention} | {DateTime.Now.Day} / {DateTime.Now.Month} / {DateTime.Now.Year} | #{0}\n{potionDescriptionMessage.Content}").ConfigureAwait(false);
                            await finalPotionResult.CreateReactionAsync(starEmoji).ConfigureAwait(false);
                            await finalPotionResult.CreateReactionAsync(checkEmoji).ConfigureAwait(false);
                            await finalPotionResult.CreateReactionAsync(crossEmoji).ConfigureAwait(false);
                            break;
                          case 8: // Tools 

                            //-----TOOLS CATEGORY------//

                            suggestionBuilder.ClearFields();
                            suggestionBuilder.WithTitle("Please choose which type of tool this is:");
                            suggestionBuilder.WithColor(color);
                            suggestionBuilder.AddField("1. Pickaxe", "For mining normal tiles");
                            suggestionBuilder.AddField("2. Axe", "For cutting down trees");
                            suggestionBuilder.AddField("3. Pickaxe Axe", "A tool with the functionality of a Pickaxe and an Axe");
                            suggestionBuilder.AddField("4. Hammer", "For breaking walls");
                            await mainCategoryQuestion.ModifyAsync(msg => msg.Embed = suggestionBuilder.Build()).ConfigureAwait(false);

                            InteractivityExtension toolInteractivity = context.Client.GetInteractivity();
                            InteractivityResult<DiscordMessage> toolResponse = await toolInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                            DiscordMessage toolMessage = toolResponse.Result;

                            await toolMessage.DeleteAsync().ConfigureAwait(false);

                            int tResult = int.Parse(toolMessage.Content);
                            if (tResult != 1 && tResult != 2 && tResult != 3 && tResult != 4 && tResult != 5)
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
                              await mainCategoryQuestion.DeleteAsync().ConfigureAwait(false);
                              string toolReply = tResult == 1 ? "Pickaxe" :
                                                 tResult == 2 ? "Axe" :
                                                 tResult == 3 ? "Pickaxe Axe" :
                                                 tResult == 4 ? "Hammer" :
                                                 "Error: Undefined";

                              DiscordMessage toolNameQuestion = await context.Channel.SendMessageAsync("Please enter the name of this tool:").ConfigureAwait(false);
                              InteractivityExtension toolNameInteractivity = context.Client.GetInteractivity();
                              InteractivityResult<DiscordMessage> toolNameResponse = await toolNameInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                              DiscordMessage toolNameMessage = toolNameResponse.Result;
                              await toolNameQuestion.DeleteAsync().ConfigureAwait(false);
                              await toolNameMessage.DeleteAsync().ConfigureAwait(false);

                              DiscordMessage toolDescriptionQuestion = await context.Channel.SendMessageAsync("Please provide a clear and concise description of this tool:").ConfigureAwait(false);
                              InteractivityExtension toolDescriptionInteractivity = context.Client.GetInteractivity();
                              InteractivityResult<DiscordMessage> toolDescriptionResponse = await toolDescriptionInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                              DiscordMessage toolDescriptionMessage = toolDescriptionResponse.Result;
                              await toolDescriptionQuestion.DeleteAsync().ConfigureAwait(false);
                              await toolDescriptionMessage.DeleteAsync().ConfigureAwait(false);

                              DiscordMessage finalToolResult = await context.Channel.SendMessageAsync($"{toolNameMessage.Content} | {toolReply} | {itemCategoryReply} | {entityGamestageReply} | {entityCategoryReply} | {mainCategoryReply} | {context.User.Mention} | {DateTime.Now.Day} / {DateTime.Now.Month} / {DateTime.Now.Year} | #{0}\n{toolDescriptionMessage.Content}").ConfigureAwait(false);
                              await finalToolResult.CreateReactionAsync(starEmoji).ConfigureAwait(false);
                              await finalToolResult.CreateReactionAsync(checkEmoji).ConfigureAwait(false);
                              await finalToolResult.CreateReactionAsync(crossEmoji).ConfigureAwait(false);
                            }
                            break;
                          case 9: // Weapon

                            //-----WEAPON CATEGORY-----//

                            suggestionBuilder.ClearFields();
                            suggestionBuilder.WithTitle("Please choose which class this weapon should be aimed at:");
                            suggestionBuilder.WithColor(color);
                            suggestionBuilder.AddField("1. Melee", "Suitable for Melee builds");
                            suggestionBuilder.AddField("2. Ranged", "Suitable for Ranged builds");
                            suggestionBuilder.AddField("3. Magic", "Suitable for Mage builds");
                            suggestionBuilder.AddField("4. Summon", "Suitable for Summoner builds");
                            suggestionBuilder.AddField("5. Cleric", "Suitable for CLeric builds");
                            suggestionBuilder.AddField("6. Typeless", "Usable by every class");
                            await mainCategoryQuestion.ModifyAsync(msg => msg.Embed = suggestionBuilder.Build()).ConfigureAwait(false);

                            InteractivityExtension weaponClassInteractivity = context.Client.GetInteractivity();
                            InteractivityResult<DiscordMessage> weaponClassResponse = await weaponClassInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                            DiscordMessage weaponClassMessage = weaponClassResponse.Result;
                            await weaponClassMessage.DeleteAsync().ConfigureAwait(false);

                            int wResult = int.Parse(weaponClassMessage.Content);
                            if (wResult != 1 && wResult != 2 && wResult != 3 && wResult != 4 && wResult != 5 && wResult != 6)
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
                              await mainCategoryQuestion.DeleteAsync().ConfigureAwait(false);
                              string weaponClassReply = wResult == 1 ? "Melee" :
                                                        wResult == 2 ? "Ranged" :
                                                        wResult == 3 ? "Magic" :
                                                        wResult == 4 ? "Summoner" :
                                                        wResult == 5 ? "Cleric" :
                                                        wResult == 6 ? "Typeless" :
                                                        "Error: Undefined";

                              DiscordMessage weaponNameQuestion = await context.Channel.SendMessageAsync("Please enter the name of this weapon:").ConfigureAwait(false);
                              InteractivityExtension weaponNameInteractivity = context.Client.GetInteractivity();
                              InteractivityResult<DiscordMessage> weaponNameResponse = await weaponNameInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                              DiscordMessage weaponNameMessage = weaponNameResponse.Result;
                              await weaponNameQuestion.DeleteAsync().ConfigureAwait(false);
                              await weaponNameMessage.DeleteAsync().ConfigureAwait(false);

                              DiscordMessage weaponDescriptionQuestion = await context.Channel.SendMessageAsync("Please provide a clear and concise description of this weapon:").ConfigureAwait(false);
                              InteractivityExtension weaponDescriptionInteractivity = context.Client.GetInteractivity();
                              InteractivityResult<DiscordMessage> weaponDescriptionResponse = await weaponDescriptionInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                              DiscordMessage weaponDescriptionMessage = weaponDescriptionResponse.Result;
                              await weaponDescriptionQuestion.DeleteAsync().ConfigureAwait(false);
                              await weaponDescriptionMessage.DeleteAsync().ConfigureAwait(false);

                              DiscordMessage finalWeaponResult = await context.Channel.SendMessageAsync($"{weaponNameMessage.Content} | {weaponClassReply} | {itemCategoryReply} | {entityGamestageReply} | {entityCategoryReply} | {mainCategoryReply} | {context.User.Mention} | {DateTime.Now.Day} / {DateTime.Now.Month} / {DateTime.Now.Year} | #{0}\n{weaponDescriptionMessage.Content}").ConfigureAwait(false);
                              await finalWeaponResult.CreateReactionAsync(starEmoji).ConfigureAwait(false);
                              await finalWeaponResult.CreateReactionAsync(checkEmoji).ConfigureAwait(false);
                              await finalWeaponResult.CreateReactionAsync(crossEmoji).ConfigureAwait(false);
                            }
                            break;
                        }
                      }
                      break;
                    case 2: // NPC

                      //-----NPC CATEGORY-----//

                      DiscordEmbedBuilder suggestionBuilder2 = new DiscordEmbedBuilder();
                      DiscordMessage mainCategoryQuestion2 = null;
                      if (egResult < 3)
                      {
                        suggestionBuilder.ClearFields();
                        suggestionBuilder.WithTitle("Please choose the biome that this NPC will spawn in:");
                        suggestionBuilder.WithColor(color);
                        suggestionBuilder.AddField("1. Surface", "Surface of the world", true);
                        suggestionBuilder.AddField("2. Underground", "Just below the surface", true);
                        suggestionBuilder.AddField("3. Purity", "Green forest", true);
                        suggestionBuilder.AddField("4. Snow", "Lots of snow", true);
                        suggestionBuilder.AddField("5. Desert", "Lots of sand", true);
                        suggestionBuilder.AddField("6. Corruption", "Cursed decay scourge", true);
                        suggestionBuilder.AddField("7. Crimson", "Festering blood scourge", true);
                        suggestionBuilder.AddField("8. Jungle", "Dangerous dense floral expanse", true);
                        suggestionBuilder.AddField("9. Dungeon", "The home of the Order of the Eclipse", true);
                        suggestionBuilder.AddField("10. Ocean", "Endless expanse of water", true);
                        suggestionBuilder.AddField("11. Mushroom", "Strange expanse of oddly glowing mushrooms", true);
                        suggestionBuilder.AddField("12. Cavern", "The deep underground", true);
                        suggestionBuilder.AddField("13. Underground Desert", "Forgotten sandy tombs", true);
                        suggestionBuilder.AddField("14. Underground Jungle", "Endless viney caves", true);
                        suggestionBuilder.AddField("15. Underground Mushroom", "Strange cavern of oddly glowing mushrooms", true);
                        suggestionBuilder.AddField("16. Ice Biome", "Caverns of glittering ice crystals", true);
                        suggestionBuilder.AddField("17. Glowing Moss Biome", "Expanse of pretty glowing moss", true);
                        suggestionBuilder.AddField("18. Underworld", "The depths of the underground, filled with brimstone flames", true);
                        suggestionBuilder.AddField("19. Granite Cave", "Dark cavern of glittering granite", true);
                        suggestionBuilder.AddField("20. Marble Cave", "Bright cavern of smooth, polished marble", true);
                        suggestionBuilder.AddField("21. Graveyard", "Where the dead have been laid to rest", true);
                        suggestionBuilder.AddField("22. Oasis", "Paradise, in miniature form", true);
                        suggestionBuilder.AddField("23. Meteorite", "A strange material, with strange guests", true);
                        suggestionBuilder.AddField("24. Spider Cave", "A terrifying cavern of webs and eight-legged beasts", true);
                        await mainCategoryQuestion.ModifyAsync(msg => msg.Embed = suggestionBuilder.Build()).ConfigureAwait(false);
                      }
                      else if (egResult >= 3 && egResult <= 6)
                      {
                        suggestionBuilder.ClearFields();
                        suggestionBuilder.WithTitle("Please choose the biome that this NPC will spawn in:");
                        suggestionBuilder.WithColor(color);
                        suggestionBuilder.AddField("1. Surface", "Surface of the world", true);
                        suggestionBuilder.AddField("2. Underground", "Just below the surface", true);
                        suggestionBuilder.AddField("3. Purity", "Green forest", true);
                        suggestionBuilder.AddField("4. Snow", "Lots of snow", true);
                        suggestionBuilder.AddField("5. Desert", "Lots of sand", true);
                        suggestionBuilder.AddField("6. Corruption", "Cursed decay scourge", true);
                        suggestionBuilder.AddField("7. Crimson", "Festering blood scourge", true);
                        suggestionBuilder.AddField("8. Jungle", "Dangerous dense floral expanse", true);
                        suggestionBuilder.AddField("9. Dungeon", "The home of the Order of the Eclipse", true);
                        suggestionBuilder.AddField("10. Ocean", "Endless expanse of water", true);
                        suggestionBuilder.AddField("11. Mushroom", "Strange expanse of oddly glowing mushrooms", true);
                        suggestionBuilder.AddField("12. Cavern", "The deep underground", true);
                        suggestionBuilder.AddField("13. Underground Desert", "Forgotten sandy tombs", true);
                        suggestionBuilder.AddField("14. Underground Jungle", "Endless viney caves", true);
                        suggestionBuilder.AddField("15. Underground Mushroom", "Strange cavern of oddly glowing mushrooms", true);
                        suggestionBuilder.AddField("16. Ice Biome", "Caverns of glittering ice crystals", true);
                        suggestionBuilder.AddField("17. Glowing Moss Biome", "Expanse of pretty glowing moss", true);
                        suggestionBuilder.AddField("18. Underworld", "The depths of the underground, filled with brimstone flames", true);
                        suggestionBuilder.AddField("19. Granite Cave", "Dark cavern of glittering granite", true);
                        suggestionBuilder.AddField("20. Marble Cave", "Bright cavern of smooth, polished marble", true);
                        suggestionBuilder.AddField("21. Graveyard", "Where the dead have been laid to rest", true);
                        suggestionBuilder.AddField("22. Oasis", "Paradise, in miniature form", true);
                        suggestionBuilder.AddField("23. Meteorite", "A strange material, with strange guests", true);
                        suggestionBuilder.AddField("24. Spider Cave", "A terrifying cavern of webs and eight-legged beasts", true);

                        suggestionBuilder2.ClearFields();
                        suggestionBuilder2.WithColor(color);
                        suggestionBuilder2.AddField("25. Hallow Desert", "Sanctified sands", true);
                        suggestionBuilder2.AddField("26. Granite Cave", "Dark cavern of glittering granite", true);
                        suggestionBuilder2.AddField("27. Marble Cave", "Bright cavern of smooth, polished marble", true);
                        suggestionBuilder2.AddField("28. Graveyard", "Where the dead have been laid to rest", true);
                        suggestionBuilder2.AddField("29. Oasis", "Paradise, in miniature form", true);
                        suggestionBuilder2.AddField("30. Jungle Temple", "The ancient home of the Lihzahrd", true);
                        suggestionBuilder2.AddField("31. Meteorite", "A strange material, with strange guests", true);
                        suggestionBuilder2.AddField("32. Spider Cave", "A terrifying cavern of webs and eight-legged beasts", true);
                        suggestionBuilder2.AddField("33. Endless Sea", "The endless expanse of ocean", true);
                        suggestionBuilder2.AddField("34. Forgotten Depths", "Deep ocean", true);

                        await mainCategoryQuestion.ModifyAsync(msg => msg.Embed = suggestionBuilder.Build()).ConfigureAwait(false);
                        mainCategoryQuestion2 = await context.Channel.SendMessageAsync(suggestionBuilder2.Build()).ConfigureAwait(false);
                      }
                      else if (egResult == 7)
                      {
                        suggestionBuilder.ClearFields();
                        suggestionBuilder.WithTitle("Please choose the biome that this NPC will spawn in:");
                        suggestionBuilder.WithColor(color);
                        suggestionBuilder.AddField("1. Surface", "Surface of the world", true);
                        suggestionBuilder.AddField("2. Underground", "Just below the surface", true);
                        suggestionBuilder.AddField("3. Purity", "Green forest", true);
                        suggestionBuilder.AddField("4. Snow", "Lots of snow", true);
                        suggestionBuilder.AddField("5. Desert", "Lots of sand", true);
                        suggestionBuilder.AddField("6. Corruption", "Cursed decay scourge", true);
                        suggestionBuilder.AddField("7. Crimson", "Festering blood scourge", true);
                        suggestionBuilder.AddField("8. Jungle", "Dangerous dense floral expanse", true);
                        suggestionBuilder.AddField("9. Dungeon", "The home of the Order of the Eclipse", true);
                        suggestionBuilder.AddField("10. Ocean", "Endless expanse of water", true);
                        suggestionBuilder.AddField("11. Mushroom", "Strange expanse of oddly glowing mushrooms", true);
                        suggestionBuilder.AddField("12. Cavern", "The deep underground", true);
                        suggestionBuilder.AddField("13. Underground Desert", "Forgotten sandy tombs", true);
                        suggestionBuilder.AddField("14. Underground Jungle", "Endless viney caves", true);
                        suggestionBuilder.AddField("15. Underground Mushroom", "Strange cavern of oddly glowing mushrooms", true);
                        suggestionBuilder.AddField("16. Ice Biome", "Caverns of glittering ice crystals", true);
                        suggestionBuilder.AddField("17. Glowing Moss Biome", "Expanse of pretty glowing moss", true);
                        suggestionBuilder.AddField("18. Underworld", "The depths of the underground, filled with brimstone flames", true);
                        suggestionBuilder.AddField("19. Granite Cave", "Dark cavern of glittering granite", true);
                        suggestionBuilder.AddField("20. Marble Cave", "Bright cavern of smooth, polished marble", true);
                        suggestionBuilder.AddField("21. Graveyard", "Where the dead have been laid to rest", true);
                        suggestionBuilder.AddField("22. Oasis", "Paradise, in miniature form", true);
                        suggestionBuilder.AddField("23. Meteorite", "A strange material, with strange guests", true);
                        suggestionBuilder.AddField("24. Spider Cave", "A terrifying cavern of webs and eight-legged beasts", true);

                        suggestionBuilder2.ClearFields();
                        suggestionBuilder2.WithColor(color);
                        suggestionBuilder2.AddField("25. Hallow Desert", "Sanctified sands", true);
                        suggestionBuilder2.AddField("26. Granite Cave", "Dark cavern of glittering granite", true);
                        suggestionBuilder2.AddField("27. Marble Cave", "Bright cavern of smooth, polished marble", true);
                        suggestionBuilder2.AddField("28. Graveyard", "Where the dead have been laid to rest", true);
                        suggestionBuilder2.AddField("29. Oasis", "Paradise, in miniature form", true);
                        suggestionBuilder2.AddField("30. Jungle Temple", "The ancient home of the Lihzahrd", true);
                        suggestionBuilder2.AddField("31. Meteorite", "A strange material, with strange guests", true);
                        suggestionBuilder2.AddField("32. Spider Cave", "A terrifying cavern of webs and eight-legged beasts", true);
                        suggestionBuilder2.AddField("33. Endless Sea", "The endless expanse of ocean", true);
                        suggestionBuilder2.AddField("34. Forgotten Depths", "Deep ocean", true);
                        suggestionBuilder2.AddField("35. Luna", "The barren surface of Luna", true);
                        suggestionBuilder2.AddField("36. Ruins of Luna", "The once beautiful paradise of Luna", true);

                        await mainCategoryQuestion.ModifyAsync(msg => msg.Embed = suggestionBuilder.Build()).ConfigureAwait(false);
                        mainCategoryQuestion2 = await context.Channel.SendMessageAsync(suggestionBuilder2.Build()).ConfigureAwait(false);
                      }

                      InteractivityExtension npcCategoryInteractivity = context.Client.GetInteractivity();
                      InteractivityResult<DiscordMessage> npcCategoryResponse = await npcCategoryInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                      DiscordMessage npcCategoryMessage = npcCategoryResponse.Result;
                      await npcCategoryMessage.DeleteAsync().ConfigureAwait(false);
                      if (mainCategoryQuestion2 != null)
                      {
                        await mainCategoryQuestion2.DeleteAsync().ConfigureAwait(false);
                      }

                      int nResult = int.Parse(npcCategoryMessage.Content);
                      if (nResult != 1 && nResult != 2 && nResult != 3 && nResult != 4 && nResult != 5 && nResult != 6 && nResult != 7 && nResult != 8 && nResult != 9 && nResult != 10
                       && nResult != 11 && nResult != 12 && nResult != 13 && nResult != 14 && nResult != 15 && nResult != 16 && nResult != 17 && nResult != 18 && nResult != 19 && nResult != 20
                       && nResult != 21 && nResult != 22 && nResult != 23 && nResult != 24 && nResult != 25 && nResult != 26 && nResult != 27 && nResult != 28 && nResult != 29 && nResult != 30
                       && nResult != 31 && nResult != 32 && nResult != 33 && nResult != 34 && nResult != 35 && nResult != 36)
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
                        string npcCategoryReply = nResult == 1 ? "Surface" :
                                                  nResult == 2 ? "Underground" :
                                                  nResult == 3 ? "Purity" :
                                                  nResult == 4 ? "Snow" :
                                                  nResult == 5 ? "Desert" :
                                                  nResult == 6 ? "Corruption" :
                                                  nResult == 7 ? "Crimson" :
                                                  nResult == 8 ? "Jungle" :
                                                  nResult == 9 ? "Dungeon" :
                                                  nResult == 10 ? "Ocean" :
                                                  nResult == 11 ? "Mushroom" :
                                                  nResult == 12 ? "Cavern" :
                                                  nResult == 13 ? "Underground Desert" :
                                                  nResult == 14 ? "Underground Jungle" :
                                                  nResult == 15 ? "Underground Mushroom" :
                                                  nResult == 16 ? "Ice Biome" :
                                                  nResult == 17 ? "Glowing Moss Biome" :
                                                  nResult == 18 ? "Underworld" :
                                                  nResult == 19 ? "The Hallow" :
                                                  nResult == 20 ? "Underground Hallow" :
                                                  nResult == 21 ? "Underground Corruption" :
                                                  nResult == 22 ? "Underground Crimson" :
                                                  nResult == 23 ? "Corrupted Desert" :
                                                  nResult == 24 ? "Crimson Desert" :
                                                  nResult == 25 ? "Hallow Desert" :
                                                  nResult == 26 ? "Granite Cave" :
                                                  nResult == 27 ? "Marble Cave" :
                                                  nResult == 28 ? "Graveyard" :
                                                  nResult == 29 ? "Oasis" :
                                                  nResult == 30 ? "Jungle Temple" :
                                                  nResult == 31 ? "Meteorite" :
                                                  nResult == 32 ? "Spider Cave" :
                                                  nResult == 33 ? "Endless Sea" :
                                                  nResult == 34 ? "Forgotten Depths" :
                                                  nResult == 35 ? "Luna" :
                                                  nResult == 36 ? "Ruins of Luna" :
                                                  "Error: Undefined";

                        //-----TYPE-----//

                        suggestionBuilder.ClearFields();
                        suggestionBuilder.WithTitle("Please choose the type of NPC you will be suggesting:");
                        suggestionBuilder.WithColor(color);
                        suggestionBuilder.AddField("1. Critter", "Tiny creature, harmless");
                        suggestionBuilder.AddField("2. Town", "Town NPC, vendor, etc");
                        suggestionBuilder.AddField("3. Enemy", "Normal hostile NPC");
                        suggestionBuilder.AddField("4. Miniboss", "Harder than a normal hostile NPC, weaker than a Boss");
                        suggestionBuilder.AddField("5. Boss", "Boss, entailing a boss fight, boss loot, purpose, gamestage, and design");
                        suggestionBuilder.AddField("6. Environmental", "Portals, effects, lunar pillars, etc");
                        await mainCategoryQuestion.ModifyAsync(msg => msg.Embed = suggestionBuilder.Build()).ConfigureAwait(false);

                        InteractivityExtension npcTypeInteractivity = context.Client.GetInteractivity();
                        InteractivityResult<DiscordMessage> npcTypeResponse = await npcTypeInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                        DiscordMessage npcTypeMessage = npcTypeResponse.Result;

                        await npcTypeMessage.DeleteAsync().ConfigureAwait(false);

                        int ntResult = int.Parse(npcTypeMessage.Content);
                        if (ntResult != 1 && ntResult != 2 && ntResult != 3 && ntResult != 4 && ntResult != 5 && ntResult != 6)
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
                          await mainCategoryQuestion.DeleteAsync().ConfigureAwait(false);
                          string npcTypeReply = ntResult == 1 ? "Critter" :
                                                ntResult == 2 ? "Town" :
                                                ntResult == 3 ? "Enemy" :
                                                ntResult == 4 ? "Miniboss" :
                                                ntResult == 5 ? "Boss" :
                                                ntResult == 6 ? "Environmental" :
                                                "Error: Undefined";
                          DiscordMessage npcNameQuestion = await context.Channel.SendMessageAsync("Please enter the name of this NPC:").ConfigureAwait(false);
                          InteractivityExtension npcNameInteractivity = context.Client.GetInteractivity();
                          InteractivityResult<DiscordMessage> npcNameResponse = await npcNameInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                          DiscordMessage npcNameMessage = npcNameResponse.Result;
                          await npcNameQuestion.DeleteAsync().ConfigureAwait(false);
                          await npcNameMessage.DeleteAsync().ConfigureAwait(false);

                          DiscordMessage npcDescriptionQuestion = await context.Channel.SendMessageAsync("Please provide a clear and concise description of this NPC:").ConfigureAwait(false);
                          InteractivityExtension npcDescriptionInteractivity = context.Client.GetInteractivity();
                          InteractivityResult<DiscordMessage> npcDescriptionResponse = await npcDescriptionInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                          DiscordMessage npcDescriptionMessage = npcDescriptionResponse.Result;
                          await npcDescriptionQuestion.DeleteAsync().ConfigureAwait(false);
                          await npcDescriptionMessage.DeleteAsync().ConfigureAwait(false);

                          DiscordMessage finalNPCResult = await context.Channel.SendMessageAsync($"{npcNameMessage.Content} | {npcTypeReply} | {npcCategoryReply} | {entityGamestageReply} | {entityCategoryReply} | {mainCategoryReply} | {context.User.Mention} | {DateTime.Now.Day} / {DateTime.Now.Month} / {DateTime.Now.Year} | #{0}\n{npcDescriptionMessage.Content}").ConfigureAwait(false);
                          await finalNPCResult.CreateReactionAsync(starEmoji).ConfigureAwait(false);
                          await finalNPCResult.CreateReactionAsync(checkEmoji).ConfigureAwait(false);
                          await finalNPCResult.CreateReactionAsync(crossEmoji).ConfigureAwait(false);
                        }
                      }
                      break;
                      // case 3: // Projectile
                      //   break;
                      // case 4: // Tile
                      //   break;
                      // case 5: // Dust
                      //   break;
                  }
                }
              }
              break;
            case 4: // Mechanic

              //-----MECHANIC CATEGORY-----//

              suggestionBuilder.ClearFields();
              suggestionBuilder.WithTitle("Please choose which type of mechanic this is:");
              suggestionBuilder.WithColor(color);
              suggestionBuilder.AddField("1. Combat", "Any mechanical addition that affects the way combat is played");
              suggestionBuilder.AddField("2. Convenience", "Any mechanical addition that serves to simplify an otherwise more complicated process");
              await mainCategoryQuestion.ModifyAsync(msg => msg.Embed = suggestionBuilder.Build()).ConfigureAwait(false);

              InteractivityExtension mechanicInteractivity = context.Client.GetInteractivity();
              InteractivityResult<DiscordMessage> mechanicResponse = await mechanicInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
              DiscordMessage mechanicMessage = mechanicResponse.Result;

              await mechanicMessage.DeleteAsync().ConfigureAwait(false);

              int mechResult = int.Parse(mechanicMessage.Content);
              if (mechResult != 1 && mechResult != 2)
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
                await mainCategoryQuestion.DeleteAsync().ConfigureAwait(false);
                string mechanicReply = mechResult == 1 ? "Combat" :
                                       mechResult == 2 ? "Convenience" :
                                       "Error: Undefined";

                DiscordMessage mechanicNameQuestion = await context.Channel.SendMessageAsync("Please enter the name of this mechanic:").ConfigureAwait(false);
                InteractivityExtension mechanicNameInteractivity = context.Client.GetInteractivity();
                InteractivityResult<DiscordMessage> mechanicNameResponse = await mechanicNameInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                DiscordMessage mechanicNameMessage = mechanicNameResponse.Result;
                await mechanicNameQuestion.DeleteAsync().ConfigureAwait(false);
                await mechanicNameMessage.DeleteAsync().ConfigureAwait(false);

                DiscordMessage mechanicDescriptionQuestion = await context.Channel.SendMessageAsync("Please provide a clear and concise description of this mechanic:").ConfigureAwait(false);
                InteractivityExtension mechanicDescriptionInteractivity = context.Client.GetInteractivity();
                InteractivityResult<DiscordMessage> mechanicDescriptionResponse = await mechanicDescriptionInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                DiscordMessage mechanicDescriptionMessage = mechanicDescriptionResponse.Result;
                await mechanicDescriptionQuestion.DeleteAsync().ConfigureAwait(false);
                await mechanicDescriptionMessage.DeleteAsync().ConfigureAwait(false);

                DiscordMessage finalMechanicResult = await context.Channel.SendMessageAsync($"{mechanicNameMessage.Content} | {mechanicReply} | {mainCategoryReply} | {context.User.Mention} | {DateTime.Now.Day} / {DateTime.Now.Month} / {DateTime.Now.Year} | #{0}\n{mechanicDescriptionMessage.Content}").ConfigureAwait(false);
                await finalMechanicResult.CreateReactionAsync(starEmoji).ConfigureAwait(false);
                await finalMechanicResult.CreateReactionAsync(checkEmoji).ConfigureAwait(false);
                await finalMechanicResult.CreateReactionAsync(crossEmoji).ConfigureAwait(false);
              }
              break;
            case 5: // Worldgen

              //-----WORLDGEN CATEGORY-----//

              suggestionBuilder.ClearFields();
              suggestionBuilder.WithTitle("Please choose which type of mechanic this is:");
              suggestionBuilder.WithColor(color);
              suggestionBuilder.AddField("1. Addition", "Any addition to the process that Worldgen follows");
              suggestionBuilder.AddField("2. Modification", "Any modification to the existing Worldgen process");
              suggestionBuilder.AddField("3. Subtraction", "Amy subtraction from the existing Worldgen process");
              suggestionBuilder.AddField("4. Tile", "A tile that is added to a certain part of the Worldgen");
              await mainCategoryQuestion.ModifyAsync(msg => msg.Embed = suggestionBuilder.Build()).ConfigureAwait(false);

              InteractivityExtension worldgenInteractivity = context.Client.GetInteractivity();
              InteractivityResult<DiscordMessage> worldgenResponse = await worldgenInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
              DiscordMessage worldgenMessage = worldgenResponse.Result;

              await worldgenMessage.DeleteAsync().ConfigureAwait(false);

              int worResult = int.Parse(worldgenMessage.Content);
              if (worResult != 1 && worResult != 2 && worResult != 3 && worResult != 4)
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
                string worldgenReply = worResult == 1 ? "Addition" :
                                       worResult == 2 ? "Modification" :
                                       worResult == 3 ? "Subtraction" :
                                       worResult == 4 ? "Tile" :
                                       "Error: Undefined";

                if (worResult == 4)
                {
                  suggestionBuilder.ClearFields();
                  suggestionBuilder.WithTitle("Please choose the biome that this tile will spawn in:");
                  suggestionBuilder.WithColor(color);
                  suggestionBuilder.AddField("1. Surface", "Surface of the world", true);
                  suggestionBuilder.AddField("2. Underground", "Just below the surface", true);
                  suggestionBuilder.AddField("3. Purity", "Green forest", true);
                  suggestionBuilder.AddField("4. Snow", "Lots of snow", true);
                  suggestionBuilder.AddField("5. Desert", "Lots of sand", true);
                  suggestionBuilder.AddField("6. Corruption", "Cursed decay scourge", true);
                  suggestionBuilder.AddField("7. Crimson", "Festering blood scourge", true);
                  suggestionBuilder.AddField("8. Jungle", "Dangerous dense floral expanse", true);
                  suggestionBuilder.AddField("9. Dungeon", "The home of the Order of the Eclipse", true);
                  suggestionBuilder.AddField("10. Ocean", "Endless expanse of water", true);
                  suggestionBuilder.AddField("11. Mushroom", "Strange expanse of oddly glowing mushrooms", true);
                  suggestionBuilder.AddField("12. Cavern", "The deep underground", true);
                  suggestionBuilder.AddField("13. Underground Desert", "Forgotten sandy tombs", true);
                  suggestionBuilder.AddField("14. Underground Jungle", "Endless viney caves", true);
                  suggestionBuilder.AddField("15. Underground Mushroom", "Strange cavern of oddly glowing mushrooms", true);
                  suggestionBuilder.AddField("16. Ice Biome", "Caverns of glittering ice crystals", true);
                  suggestionBuilder.AddField("17. Glowing Moss Biome", "Expanse of pretty glowing moss", true);
                  suggestionBuilder.AddField("18. Underworld", "The depths of the underground, filled with brimstone flames", true);
                  suggestionBuilder.AddField("19. The Hallow", "Sanctified lands", true);
                  suggestionBuilder.AddField("20. Undergrund Hallow", "Sanctified caverns", true);
                  suggestionBuilder.AddField("21. Underground Corruption", "Decaying cursed caverns", true);
                  suggestionBuilder.AddField("22. Underground Crimson", "Festering blood caverns", true);
                  suggestionBuilder.AddField("23. Corrupted Desert", "Cursed sands", true);
                  suggestionBuilder.AddField("24. Crimson Desert", "Blood sands", true);
                  DiscordEmbedBuilder suggestionBuilder2 = new DiscordEmbedBuilder();
                  suggestionBuilder2.WithColor(color);
                  suggestionBuilder2.AddField("25. Hallow Desert", "Sanctified sands", true);
                  suggestionBuilder2.AddField("26. Granite Cave", "Dark cavern of glittering granite", true);
                  suggestionBuilder2.AddField("27. Marble Cave", "Bright cavern of smooth, polished marble", true);
                  suggestionBuilder2.AddField("28. Graveyard", "Where the dead have been laid to rest", true);
                  suggestionBuilder2.AddField("29. Oasis", "Paradise, in miniature form", true);
                  suggestionBuilder2.AddField("30. Jungle Temple", "The ancient home of the Lihzahrd", true);
                  suggestionBuilder2.AddField("31. Meteorite", "A strange material, with strange guests", true);
                  suggestionBuilder2.AddField("32. Spider Cave", "A terrifying cavern of webs and eight-legged beasts", true);
                  suggestionBuilder2.AddField("33. Endless Sea", "The endless expanse of ocean", true);
                  suggestionBuilder2.AddField("34. Forgotten Depths", "Deep ocean", true);
                  suggestionBuilder2.AddField("35. Luna", "The barren surface of Luna", true);
                  suggestionBuilder2.AddField("36. Ruins of Luna", "The once beautiful paradise of Luna", true);
                  await mainCategoryQuestion.ModifyAsync(msg => msg.Embed = suggestionBuilder.Build()).ConfigureAwait(false);
                  DiscordMessage mainCategoryQuestion2 = await context.Channel.SendMessageAsync(suggestionBuilder2.Build()).ConfigureAwait(false);

                  InteractivityExtension worldgenTileCategoryInteractivity = context.Client.GetInteractivity();
                  InteractivityResult<DiscordMessage> worldgenTileCategoryResponse = await worldgenTileCategoryInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                  DiscordMessage worldgenTileCategoryMessage = worldgenTileCategoryResponse.Result;
                  await worldgenTileCategoryMessage.DeleteAsync().ConfigureAwait(false);

                  int wtResult = int.Parse(worldgenTileCategoryMessage.Content);
                  if (wtResult != 1 && wtResult != 2 && wtResult != 3 && wtResult != 4 && wtResult != 5 && wtResult != 6 && wtResult != 7 && wtResult != 8 && wtResult != 9 && wtResult != 10
                   && wtResult != 11 && wtResult != 12 && wtResult != 13 && wtResult != 14 && wtResult != 15 && wtResult != 16 && wtResult != 17 && wtResult != 18 && wtResult != 19 && wtResult != 20
                   && wtResult != 21 && wtResult != 22 && wtResult != 23 && wtResult != 24 && wtResult != 25 && wtResult != 26 && wtResult != 27 && wtResult != 28 && wtResult != 29 && wtResult != 30
                   && wtResult != 31 && wtResult != 32 && wtResult != 33 && wtResult != 34 && wtResult != 35 && wtResult != 36)
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
                    await mainCategoryQuestion.DeleteAsync().ConfigureAwait(false);
                    await mainCategoryQuestion2.DeleteAsync().ConfigureAwait(false);

                    string worldgenTileCategoryReply = wtResult == 1 ? "Surface" :
                                                   wtResult == 2 ? "Underground" :
                                                   wtResult == 3 ? "Purity" :
                                                   wtResult == 4 ? "Snow" :
                                                   wtResult == 5 ? "Desert" :
                                                   wtResult == 6 ? "Corruption" :
                                                   wtResult == 7 ? "Crimson" :
                                                   wtResult == 8 ? "Jungle" :
                                                   wtResult == 9 ? "Dungeon" :
                                                   wtResult == 10 ? "Ocean" :
                                                   wtResult == 11 ? "Mushroom" :
                                                   wtResult == 12 ? "Cavern" :
                                                   wtResult == 13 ? "Underground Desert" :
                                                   wtResult == 14 ? "Underground Jungle" :
                                                   wtResult == 15 ? "Underground Mushroom" :
                                                   wtResult == 16 ? "Ice Biome" :
                                                   wtResult == 17 ? "Glowing Moss Biome" :
                                                   wtResult == 18 ? "Underworld" :
                                                   wtResult == 19 ? "The Hallow" :
                                                   wtResult == 20 ? "Underground Hallow" :
                                                   wtResult == 21 ? "Underground Corruption" :
                                                   wtResult == 22 ? "Underground Crimson" :
                                                   wtResult == 23 ? "Corrupted Desert" :
                                                   wtResult == 24 ? "Crimson Desert" :
                                                   wtResult == 25 ? "Hallow Desert" :
                                                   wtResult == 26 ? "Granite Cave" :
                                                   wtResult == 27 ? "Marble Cave" :
                                                   wtResult == 28 ? "Graveyard" :
                                                   wtResult == 29 ? "Oasis" :
                                                   wtResult == 30 ? "Jungle Temple" :
                                                   wtResult == 31 ? "Meteorite" :
                                                   wtResult == 32 ? "Spider Cave" :
                                                   wtResult == 33 ? "Endless Sea" :
                                                   wtResult == 34 ? "Forgotten Depths" :
                                                   wtResult == 35 ? "Luna" :
                                                   wtResult == 36 ? "Ruins of Luna" :
                                                   "Error: Undefined";
                    DiscordMessage worldgenTileNameQuestion = await context.Channel.SendMessageAsync("Please enter the name of this Tile:").ConfigureAwait(false);
                    InteractivityExtension worldgenTileNameInteractivity = context.Client.GetInteractivity();
                    InteractivityResult<DiscordMessage> worldgenTileNameResponse = await worldgenTileNameInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                    DiscordMessage worldgenTileNameMessage = worldgenTileNameResponse.Result;
                    await worldgenTileNameQuestion.DeleteAsync().ConfigureAwait(false);
                    await worldgenTileNameMessage.DeleteAsync().ConfigureAwait(false);

                    DiscordMessage worldgenTileDescriptionQuestion = await context.Channel.SendMessageAsync("Please provide a clear and concise description of this Tile:").ConfigureAwait(false);
                    InteractivityExtension worldgenTileDescriptionInteractivity = context.Client.GetInteractivity();
                    InteractivityResult<DiscordMessage> worldgenTileDescriptionResponse = await worldgenTileDescriptionInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                    DiscordMessage worldgenTileDescriptionMessage = worldgenTileDescriptionResponse.Result;
                    await worldgenTileDescriptionQuestion.DeleteAsync().ConfigureAwait(false);
                    await worldgenTileDescriptionMessage.DeleteAsync().ConfigureAwait(false);

                    DiscordMessage finalWorldgenTileResult = await context.Channel.SendMessageAsync($"{worldgenTileNameMessage.Content} | {worldgenTileCategoryReply} | {worldgenReply} | {mainCategoryReply} | {context.User.Mention} | {DateTime.Now.Day} / {DateTime.Now.Month} / {DateTime.Now.Year} | #{0}\n{worldgenTileDescriptionMessage.Content}").ConfigureAwait(false);
                    await finalWorldgenTileResult.CreateReactionAsync(starEmoji).ConfigureAwait(false);
                    await finalWorldgenTileResult.CreateReactionAsync(checkEmoji).ConfigureAwait(false);
                    await finalWorldgenTileResult.CreateReactionAsync(crossEmoji).ConfigureAwait(false);
                  }
                }
                else
                {
                  await mainCategoryQuestion.DeleteAsync().ConfigureAwait(false);

                  DiscordMessage worldgenNameQuestion = await context.Channel.SendMessageAsync("Please give this Worldgen change a short title:").ConfigureAwait(false);
                  InteractivityExtension worldgenNameInteractivity = context.Client.GetInteractivity();
                  InteractivityResult<DiscordMessage> worldgenNameResponse = await worldgenNameInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                  DiscordMessage worldgenNameMessage = worldgenNameResponse.Result;
                  await worldgenNameQuestion.DeleteAsync().ConfigureAwait(false);
                  await worldgenNameMessage.DeleteAsync().ConfigureAwait(false);

                  DiscordMessage worldgenDescriptionQuestion = await context.Channel.SendMessageAsync("Please provide a clear and concise description of this Worldgen change:").ConfigureAwait(false);
                  InteractivityExtension worldgenDescriptionInteractivity = context.Client.GetInteractivity();
                  InteractivityResult<DiscordMessage> worldgenDescriptionResponse = await worldgenDescriptionInteractivity.WaitForMessageAsync(x => x.Channel.Name == "suggestions" && x.Channel.Name == context.Channel.Name && x.Author.Id == authorID).ConfigureAwait(false);
                  DiscordMessage worldgenDescriptionMessage = worldgenDescriptionResponse.Result;
                  await worldgenDescriptionQuestion.DeleteAsync().ConfigureAwait(false);
                  await worldgenDescriptionMessage.DeleteAsync().ConfigureAwait(false);

                  DiscordMessage finalWorldgenResult = await context.Channel.SendMessageAsync($"{worldgenNameMessage.Content} | {worldgenReply} | {mainCategoryReply} | {context.User.Mention} | {DateTime.Now.Day} / {DateTime.Now.Month} / {DateTime.Now.Year} | #{0}\n{worldgenDescriptionMessage.Content}").ConfigureAwait(false);
                  await finalWorldgenResult.CreateReactionAsync(starEmoji).ConfigureAwait(false);
                  await finalWorldgenResult.CreateReactionAsync(checkEmoji).ConfigureAwait(false);
                  await finalWorldgenResult.CreateReactionAsync(crossEmoji).ConfigureAwait(false);
                }
              }
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