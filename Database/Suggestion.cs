using System;
using System.ComponentModel.DataAnnotations;

namespace csharpi.Database
{
  public partial class Suggestion
  {
    [Key]
    public int ID { get; set; }
    public string Author { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
  }
}