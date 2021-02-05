var fs = require('fs');
var moment = require('moment')();
const fieldBuilder = function(name, value) {
  const field = { name, value };
  field.name = name.slice(1, field.name.length - 1);
  return field;
};
const pluckFirstQuotedString = function(string) {
  return `${string.split('"')[1]}`;
};

const readChangelog = function() {
  let d = fs.readFileSync('./changelogdata.json');
  return JSON.parse(d);
};

const writeToLog = function(dataPlusChange) {
  fs.writeFileSync('changelogdata.json', dataPlusChange);
};

const readSuggestionList = function() {
  let d = fs.readFileSync('./suggestiondata.json');
  return JSON.parse(d);
};

const writeSuggestion = function(dataPlusChange, justTheChange = null) {
  fs.writeFileSync('suggestiondata.json', dataPlusChange);
  if (justTheChange != null) {
    fs.appendFileSync('suggestionReadable.txt', `\n\n${justTheChange}`);
  }
};

const formatSuggestion = function(ideaName, ideaBody, message, suggestionEmbed) {
  let s = `${ideaName} | ${message.author} | ${moment.format('DD[ - ]MM[ - ]YYYY')}\n${ideaBody}`;
  suggestionEmbed.addField(`${ideaName} | ${message.author} | ${moment.format('DD[ - ]MM[ - ]YYYY')}`, ideaBody);
  return s;
};

const initializeSuggestionLog = function(suggestions, suggestionEmbed) {
  suggestions.forEach(sugg => {
    // This is damn weird, but I only blame myself. -Zack
    let split = sugg.suggestion.split('\n');
    suggestionEmbed.addField(split[0], split[1]);
  });
};

module.exports = {
  fieldBuilder,
  pluckFirstQuotedString,
  readChangelog,
  writeToLog,
  readSuggestionList,
  writeSuggestion,
  formatSuggestion,
  initializeSuggestionLog
};