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
    console.log(user, userID, channelID, content);
    if (content.substring(0, 1) == '!') {
        var args = content.substring(1).split(' ');
        var cmd = args[0];
        args = args.splice(1);
        switch (cmd) {
            // !ping
            case 'ping':
                message.reply('Pong!')
                break;
            // Just add any case commands if you want to..
        }
    }
});
bot.login(TOKEN);