require("dotenv").config();
const { fieldBuilder, pluckFirstQuotedString, readChangelog, writeToLog, readSuggestionList, writeSuggestion, formatSuggestion, initializeSuggestionLog, splitWrapped } = require('./helpers');
var Discord = require('discord.js');
var logger = require('winston');
var auth = require('./auth.json');

// MAKE SURE TO TURN THIS ON WHEN TESTING.
const debug = false;
// Configure logger settings
logger.remove(logger.transports.Console);
logger.add(new logger.transports.Console, {
    colorize: true
});
logger.level = 'debug';
var bot = new Discord.Client();
const TOKEN = process.env.TOKEN;
const changelog = readChangelog(debug).contents;
const suggestions = readSuggestionList(debug).contents;
let changelogEmbed = new Discord.MessageEmbed()
.setColor('#6a2aff')
.setTitle(`Changelog`)
.setURL('https://github.com/Unbidden-Dev-Team/UnbiddenMod')
.setAuthor('Unbidden Dev Team', 'https://i.imgur.com/a/nX4113x.png', 'https://github.com/Unbidden-Dev-Team/UnbiddenMod')
.setDescription('Mod Changes')
.setThumbnail('https://i.imgur.com/a/nX4113x.png')
.addFields(...changelog)
.setImage('https://i.imgur.com/a/nX4113x.png')
.setTimestamp()
.setFooter('UnbiddenMod Dev Team', '');

let suggestionsEmbed = new Discord.MessageEmbed()
.setColor('#6a2aff')
.setTitle(`Suggestions`)
.setAuthor('Unbidden Dev Team', 'https://i.imgur.com/a/nX4113x.png', 'https://github.com/Unbidden-Dev-Team/UnbiddenMod')
.setThumbnail('https://i.imgur.com/a/nX4113x.png')
.setTimestamp()
.setFooter('UnbiddenMod Dev Team', '');
initializeSuggestionLog(suggestions, suggestionsEmbed);

bot.username = 'UnbiddenBot';
bot.on('ready', function (evt) {
    logger.info('Connected');
    logger.info(`Logged in as: ${bot.user.tag}!`);
    if (debug) logger.info('DEBUG MODE IS ON.');
});
bot.on('message', function (message) {
    const user = message.author,
        userID = user.id,
        channelID = message.channel.id,
        content = message.content;
    if (content.substring(0, 1) == '!') {
        var args = content.substring(1).split(' ');
        var cmd = args[0];
        args = args.splice(1);
        var postCommand = args.join(' ');
        switch (cmd) {
            // !ping
            case 'help':
                message.channel.send("\`\`\`\nWelcome to UnbiddenBot!\nI am currently a Work In Progress, so some commands will not work. If they don't, we will let you know when you try to run it.\n\nOur current commands:\n- !help: Displays this message.\n- !wiki <entry>: Displays the entry provided.\n- !github: Provides a link to the source code, if you're a little curious of the inner machinations.\n- !ihaveabug: Links you to the Issues page of our code, so you can put your issue out there.\n- !changelog: View the changelog, to see what we've done so far.\n- !suggest [<name>] [<idea>]: Formats a suggestion automatically for you. Put down just \"!suggest\" to see the list of suggestions. Put down \"!suggest help\" for more details.\n\".\`\`\`");
                break;
            case 'wiki':
                message.channel.send("Sorry, this command isn't set up yet!");
                break;
            case 'github':
                message.reply(`https://github.com/Unbidden-Dev-Team/UnbiddenMod`);
                break;
            case 'ihaveabug':
                message.reply(`Post an issue here! Remember to be descriptive!\nhttps://github.com/Unbidden-Dev-Team/UnbiddenMod/issues`);
                break;
            case 'changelog':
                if (args.length === 0) {
                    message.channel.send(changelogEmbed);
                } else if (args[0] === 'add') {
                    if (args[1] !== undefined && args[2] !== undefined) {
                        const field = fieldBuilder(args.splice(1).join(" "), "Done");
                        changelog.push(field);
                        writeToLog(JSON.stringify({ contents: changelog }, null, 2), debug);

                        changelogEmbed.addFields(field);
                        message.channel.send("Done!");
                        message.channel.send(changelogEmbed);
                    } else {
                        message.channel.send("You're missing some arguments to add to the changelog!");
                    }
                } else {
                    message.reply("I don't have that functionality for the changelog!");
                }
                break;

            case 'suggest':
                if (message.channel.name.toLowerCase() != 'suggestions') {
                    message.channel.send(`Sorry, you are not in the suggestions channel! Please input your request at the suggestions channel.`);
                } else {
                    if (args.length === 0) {
                        message.channel.send(suggestionsEmbed);
                        break;
                    }
                    if (args[0] === 'help')
                    {
                        message.channel.send("\`\`\`Suggest formatting: You must wrap your name and body in [], {}, or (), and both the name and body must have the same wrapper. ex. \"!suggest [ideaName] [ideaBody]\". \"!suggest [ideaName] {ideaBody} will not work.\"\`\`\`");
                        break;
                    }
                    let wrappedArgs;
                    switch (postCommand[0]) {
                        case '(':
                            wrappedArgs = splitWrapped(postCommand, '()', false);
                            break;
                        case '[':
                            wrappedArgs = splitWrapped(postCommand, "[]", false);
                            break;
                        case '{':
                            wrappedArgs = splitWrapped(postCommand, "{}", false);
                            break;
                        default:
                            wrappedArgs = null;
                            break;
                    }
                    if (wrappedArgs === null) {
                        message.channel.send("I think you might have some formatting wrong. Try again!");
                        break;
                    }
                    console.log(wrappedArgs);
                    const name = wrappedArgs[0];
                    if (name === undefined)
                    {
                        message.channel.send("Oops, something went wrong on my end!");
                        break;
                    } else if (name.length > 40) {
                        message.channel.send("Your name is fairly long. Make sure it's actually your name, and if it is, concise it!");
                        break;
                    }
                    const body = wrappedArgs[1];
                    if (body === undefined) {
                        message.channel.send("You're missing a description!");
                        break;
                    }
                    const suggestion = formatSuggestion(name, body, message, suggestionsEmbed, suggestions);
                    suggestions.push({ suggestion, id: suggestions.length + 1 });
                    message.channel.send(suggestion);
                    writeSuggestion(JSON.stringify({ contents: suggestions }), suggestion, debug);
                    message.delete();
                }
                break;
            default:
                message.channel.send("Sorry, I'm not sure what you mean by that. Please use \"!help\" to see the full list of commands.");
                break;
        }
    }
});
bot.login(TOKEN);