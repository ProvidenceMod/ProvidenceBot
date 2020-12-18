var fs = require('fs');
const fieldBuilder = function(name, value) {
  return { name, value };
};
const pluckFirstQuotedString = function(string) {
  return `${string.split('"')[1]}`;
};

const readChangelog = function() {
  let d = fs.readFileSync('./changelogdata.json');
  return JSON.parse(d);
};

const writeToLog = function(dataPlusChange) {
  let d = fs.writeFileSync('changelogdata.json', dataPlusChange);
  return JSON.parse(d);
};

module.exports = {
  fieldBuilder,
  pluckFirstQuotedString,
  readChangelog,
  writeToLog
};