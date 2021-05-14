using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProvidenceDAL.Database
{
  public abstract class Entity
  {
    [Key]
    public int ID { get; set;}
  }
}