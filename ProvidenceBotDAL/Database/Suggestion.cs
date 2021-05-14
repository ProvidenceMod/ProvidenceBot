using System;
using System.ComponentModel.DataAnnotations;

namespace ProvidenceDAL.Database
{
  public partial class Suggestion
  {
    [Key]
    public int ID { get; set; }
    public int Number { get; set; }
    public string Author { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string[] Keywords { get; set; }
    public DateTime Date { get; set; }
  }
}