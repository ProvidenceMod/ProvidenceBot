require("dotenv").config();
const { fieldBuilder, pluckFirstQuotedString, readChangelog, writeToLog } = require('./helpers');
var Discord = require('discord.js');
var logger = require('winston');
var auth = require('./auth.json');

// Configure logger settings
logger.remove(logger.transports.Console);
logger.add(new logger.transports.Console, {
    colorize: true
});
logger.level = 'debug';
var bot = new Discord.Client();
const TOKEN = process.env.TOKEN;
const logAtBootup = readChangelog().contents;
let changelogEmbed = new Discord.MessageEmbed()
    .setColor('#6a2aff')
    .setTitle(`Changelog`)
    .setURL('https://github.com/Unbidden-Dev-Team/UnbiddenMod')
    .setAuthor('Unbidden Dev Team', 'https://i.imgur.com/a/nX4113x.png', 'https://github.com/Unbidden-Dev-Team/UnbiddenMod')
	.setDescription('Mod Changes')
	.setThumbnail('https://i.imgur.com/a/nX4113x.png')
	.addFields(...logAtBootup)
	.setImage('https://i.imgur.com/a/nX4113x.png')
	.setTimestamp()
	.setFooter('UnbiddenMod Dev Team', '');
bot.username = 'UnbiddenBot';
    bot.on('ready', function (evt) {
        logger.info('Connected');
        logger.info(`Logged in as: ${bot.user.tag}!`);
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
        switch (cmd) {
            // !ping
            case 'help':
                message.channel.send("\`\`\`\nWelcome to UnbiddenBot!\nI am currently a Work In Progress, so some commands will not work. If they don't, we will let you know when you try to run it.\n\nOur current commands:\n- !help: Displays this message.\n- !wiki <entry>: Displays the entry provided.\n- !github: Provides a link to the source code, if you're a little curious of the inner machinations.\n- !ihaveabug: Links you to the Issues page of our code, so you can put your issue out there.\n- !changelog: View the changelog, to see what we've done so far.\`\`\`");
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
                        logAtBootup.push(field);
                        writeToLog(JSON.stringify({contents: logAtBootup}, null, 2));

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
            default:
                message.channel.send("Sorry, I'm not sure what you mean by that. Please use \"!help\" to see the full list of commands.");
                break;
        }
    }
});
bot.login(TOKEN);