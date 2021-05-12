using System;

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
