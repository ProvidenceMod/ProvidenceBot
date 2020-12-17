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
// Initialize Discord Bot
var bot = new Discord.Client();
const TOKEN = process.env.TOKEN;
bot.username = 'UnbiddenBot';
bot.id =
    bot.on('ready', function (evt) {
        logger.info('Connected');
        logger.info(`Logged in as: ${bot.user.tag}!`);
        logger.info(bot.username + ' - (' + bot.id + ')');
    });
bot.on('message', function (message) {
    // Our bot needs to know if it will execute a command
    // It will listen for messages that will start with `!`

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
            default:
                message.channel.send("Sorry, I'm not sure what you mean by that. Please use \"!help\" to see the full list of commands.");
                break;
            // Just add any case commands if you want to..
        }
    }
});
bot.login(TOKEN);