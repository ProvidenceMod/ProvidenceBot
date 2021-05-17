using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProvidenceBotDAL
{
  public abstract class Entity
  {
    [Key]
    public DiscordMessage ID { get; set; }
    public string Title { get; set; }
    public string[] Keywords { get; set; }
    public DiscordUser Author { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public int Number { get; set; }
  }
}