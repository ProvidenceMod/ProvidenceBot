var fs = require('fs');
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

module.exports = {
  fieldBuilder,
  pluckFirstQuotedString,
  readChangelog,
  writeToLog
};