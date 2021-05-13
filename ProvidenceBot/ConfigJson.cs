using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProvidenceBot
{
  public struct ConfigJson
  {
    [JsonProperty("token")]
    public string Token { get; private set; }
    [JsonProperty("prefix")]
    public string Prefix { get; private set; }
    public bool DebugMode { get; private set; }
  }
}
