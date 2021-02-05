var fs = require('fs');
var moment = require('moment')();
const fieldBuilder = function(name, value) {
  const field = { name, value };
  field.name = name.slice(1, field.name.length - 1);
  return field;
};
const pluckFirstQuotedString = function(string) {
  return `${string.split('"')[0]}`;
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

const writeSuggestion = function(dataPlusChange) {
  fs.writeFileSync('suggestiondata.json', dataPlusChange);
};

const formatSuggestion = function(ideaName, ideaBody, message) {
  let s = `${ideaName} | ${message.author} | ${moment.format('DD[ - ]MM[ - ]YYYY')}\n${ideaBody}`;
  return s;
};

module.exports = {
  fieldBuilder,
  pluckFirstQuotedString,
  readChangelog,
  writeToLog,
  readSuggestionList,
  writeSuggestion,
  formatSuggestion
};