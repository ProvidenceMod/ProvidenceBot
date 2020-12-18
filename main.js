require("dotenv").config();
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
const fieldBuilder = function(name, value) {
    return { name, value };
};
const pluckFirstQuotedString = function(string) {
    let pos1, pos2;
    for (let i = 0; i < string.length; i++) {
        if (pos1 !== undefined && pos2 !== undefined) {
            return string.slice(pos1, pos2 + 1);
        }
        const c = string[i];
        if (c === '"') {
            if (pos1 !== undefined) {
                pos1 = i;
                continue;
            } else if (pos2 !== undefined) {
                pos2 = i;
                continue;
            }
        }
    }
};

let changelogEmbed = new Discord.MessageEmbed()
    .setColor('#6a2aff')
    .setTitle(`Changelog`)
    .setURL('https://github.com/Unbidden-Dev-Team/UnbiddenMod')
    .setAuthor('Unbidden Dev Team', 'https://i.imgur.com/a/nX4113x.png', 'https://github.com/Unbidden-Dev-Team/UnbiddenMod')
	.setDescription('Mod Changes')
	.setThumbnail('https://i.imgur.com/a/nX4113x.png')
	.addFields(
        { name: 'Fire Ancient can now find players', value: 'Done' },
        { name: 'Fire Ancient now has animation', value: 'Done' },
        { name: 'Made Players susceptible to elements', value: 'Done' },
	)
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
                message.channel.send("\`\`\`\nWelcome to UnbiddenBot!\nI am currently a Work In Progress, so some commands will not work. If they don't, we will let you know when you try to run it.\n\nOur current commands:\n- !help: Displays this message.\n- !wiki <entry>: Displays the entry provided.\n- !github: Provides a link to the source code, if you're a little curious of the inner machinations.\n- !ihaveabug: Links you to the Issues page of our code, so you can put your issue out there.\`\`\`");
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
                        changelogEmbed.addFields(fieldBuilder(args[1], args[2]))
                    } else {
                        message.channel.send("You're missing some arguments to add to the changelog!");
                    }
                }
                break;
            default:
                message.channel.send("Sorry, I'm not sure what you mean by that. Please use \"!help\" to see the full list of commands.");
                break;
        }
    }
});
bot.login(TOKEN);