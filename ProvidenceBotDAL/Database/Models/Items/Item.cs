using System;
using System.Collections.Generic;
using System.Text;

namespace ProvidenceDAL.Database.Models.items
{
  public class Item : Entity
  {
    public string Name { get; set; }
    public string Description { get; set; }
  }
}