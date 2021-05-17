using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProvidenceBotDAL.Models.Item
{
  public class Suggestion : Entity
  {
    public DiscordMessage ID { get; set; }
    public string Title { get; set; }
    public string[] Keywords { get; set; }
    public DiscordUser Author { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public int Number { get; set; }
  }
}