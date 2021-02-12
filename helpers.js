var fs = require('fs');
var moment = require('moment');
const fieldBuilder = function(name, value) {
  const field = { name, value };
  field.name = name.slice(1, field.name.length - 1);
  return field;
};
const pluckFirstQuotedString = function(string) {
  return `${string.split('"')[1]}`;
};

const splitWrapped = function(string, wrappableIndicator, includeWrappings = false) {
  // Preliminary check if it's a valid indicator
  if (wrappableIndicator !==  '{}' && wrappableIndicator !==  '[]' && wrappableIndicator !== '()') return null;
  // Definitions
  const split = [];
  let open = false;
  let s = ``;
  // Loop through string, with preliminary shorthand assign
  for (let i = 0; i < string.length; i++) {
    const c = string[i];
    // If it's an opening wrap, start including
    if (c === wrappableIndicator[0]) {
      open = true;
      s += c;
      continue;
    }
    // If open, include everything
    if (open) {
      s += c;
      // If what we just included is a closer, push it up, reset and move on
      if (c === wrappableIndicator[1]) {
        open = false;
        split.push(s);
        s = ``;
        continue;
      }
    }
  }
  if (!includeWrappings) split.forEach((section, i) => { split[i] = section.slice(1, section.length - 1); });

  return split;
};

const readChangelog = function(debug = false) {
  let d = fs.readFileSync(debug ? './changelogdataDebug.json' : './changelogdata.json');
  return JSON.parse(d);
};

const writeToLog = function(dataPlusChange, debug = false) {
  fs.writeFileSync(debug ? 'changelogdataDebug.json' : 'changelogdata.json', dataPlusChange);
};

const readSuggestionList = function(debug = false) {
  let d = fs.readFileSync(debug ? './suggestiondataDebug.json' : './suggestiondata.json');
  return JSON.parse(d);
};

const writeSuggestion = function(dataPlusChange, justTheChange = null, debug = false) {
  fs.writeFileSync(debug ? './suggestiondataDebug.json' : './suggestiondata.json', dataPlusChange);
  if (justTheChange != null) {
    fs.appendFileSync(debug ? './suggestionReadableDebug.txt' : './suggestionReadable.txt', `\n\n${justTheChange}`);
  }
};

const formatSuggestion = function(ideaName, ideaBody, message, suggestionEmbed, suggestions) {
  let s = `${ideaName} | ${message.author} | ${moment().format('DD[ - ]MM[ - ]YYYY')} | ${suggestions.length + 1}\n${ideaBody}`;
  suggestionEmbed.addField(`${ideaName} | ${message.author} | ${moment().format('DD[ - ]MM[ - ]YYYY')}`, ideaBody);
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
  initializeSuggestionLog,
  splitWrapped
};