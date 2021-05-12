using DSharpPlus;
using System;
using System.Threading.Tasks;

namespace ProvidenceBot
{
  class Program
  {
    static void Main(string[] args)
    {
      Bot bot = new Bot();
      bot.RunAsync().GetAwaiter().GetResult();
    }


  }
}
