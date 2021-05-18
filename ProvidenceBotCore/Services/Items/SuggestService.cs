using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;
using ProvidenceBotDAL;
using ProvidenceBotDAL.Models.Item;
using DSharpPlus.CommandsNext;

namespace ProvidenceBotCore.Services.Items
{
  public interface ISuggestService
  {
    Task AddSuggestion(Suggestion suggestion);
    Task RemoveSuggestion(Suggestion suggestion);
    Task ListSuggestion();
    Task<SortedList<Suggestion, int>> FindSuggestionByRelativity(DiscordUser author = null, string title = null, string keywords = null, int number = 0);
    Task<List<Suggestion>> FindSuggestionByAuthor(DiscordUser author);
    Task<Suggestion> FindSuggestionByNumber(CommandContext context, int number);
    Task<List<Suggestion>> FindSuggestionByTitle(string title);
    Task<List<Suggestion>> FindSuggestionByKeyword(string keyword);
    Task<int> FindSuggestionNumber();
  }
  public class SuggestService : ISuggestService
  {
    private readonly SuggestContext suggestContext;
    public SuggestService(SuggestContext _suggestContext)
    {
      suggestContext = _suggestContext;
    }
    public async Task AddSuggestion(Suggestion suggestion)
    {
      await suggestContext.AddAsync(suggestion).ConfigureAwait(false);
      await suggestContext.SaveChangesAsync().ConfigureAwait(false);
    }
    public async Task RemoveSuggestion(Suggestion suggestion)
    {
      int number = suggestion.Number;
      suggestContext.Remove(suggestion);
      await suggestContext.SaveChangesAsync().ConfigureAwait(false);
      //await suggestContext.DisposeAsync().ConfigureAwait(false);
      suggestContext.Update(suggestContext.Suggestion.FirstOrDefault(x => x.Number == number));
    }
    public async Task ListSuggestion()
    {
      foreach (Suggestion suggestion in suggestContext.Suggestion)
      {
        Console.WriteLine($"{suggestion.ID}");
        Console.WriteLine($"{suggestion.Title}");
        Console.WriteLine($"{suggestion.Keywords}");
        Console.WriteLine($"{suggestion.Author}");
        Console.WriteLine($"{suggestion.Date}");
        Console.WriteLine($"{suggestion.Description}");
        Console.WriteLine($"{suggestion.Number}");
      }
    }
    public async Task<SortedList<Suggestion, int>> FindSuggestionByRelativity(DiscordUser author = null, string title = null, string keywords = null, int number = 0)
    {
      title = title.ToLower();
      List<Suggestion> mainList = await suggestContext.Suggestion.ToListAsync();
      SortedList<Suggestion, int> secondaryList = new SortedList<Suggestion, int>();
      string[] keywordArray = keywords.Split(",");

      foreach (Suggestion suggestion in mainList)
      {
        string[] keywordArray2 = suggestion.Keywords.Split(",");
        int relativityScore = 0;
        if (author != null && suggestion.Author == author.Id)
        {
          relativityScore += 3;
        }
        if (title != null && suggestion.Title.ToLower() == title)
        {
          relativityScore += 10;
        }
        if (number != 0 && suggestion.Number == number)
        {
          relativityScore += 10;
        }
        if (keywords != null)
        {
          foreach (string keyword in keywordArray2)
          {
            foreach (string keyword2 in keywordArray)
            {
              if (keyword == keyword2)
                relativityScore++;
            }
          }
        }
        secondaryList.Add(suggestion, relativityScore);
      }
      return secondaryList;
    }
    public async Task<List<Suggestion>> FindSuggestionByAuthor(DiscordUser author)
    {
      List<Suggestion> mainList = await suggestContext.Suggestion.ToListAsync();
      List<Suggestion> secondaryList = new List<Suggestion>();
      foreach (Suggestion suggestion in mainList)
      {
        if (suggestion.Author == author.Id)
        {
          secondaryList.Add(suggestion);
        }
      }
      return secondaryList;
    }
    public async Task<Suggestion> FindSuggestionByNumber(CommandContext context, int number)
    {
      //List<Suggestion> mainList = await suggestContext.Suggestion.ToListAsync();
      //Suggestion suggestion = null;
      //foreach (Suggestion suggestion2 in mainList)
      //{
      //  if (suggestion.Number == number)
      //  {
      //    suggestion = suggestion2;
      //  }
      //}
      //return suggestion != null ? suggestion : null;
      return await suggestContext.Suggestion.FirstOrDefaultAsync(x => x.Number == number).ConfigureAwait(false);
    }
    public async Task<List<Suggestion>> FindSuggestionByTitle(string title)
    {
      List<Suggestion> mainList = await suggestContext.Suggestion.ToListAsync();
      List<Suggestion> secondaryList = new List<Suggestion>();
      foreach (Suggestion suggestion in mainList)
      {
        if (string.Equals(suggestion.Title, title, StringComparison.OrdinalIgnoreCase))
        {
          secondaryList.Add(suggestion);
        }
      }
      return secondaryList;
    }
    public async Task<List<Suggestion>> FindSuggestionByKeyword(string keyword)
    {
      List<Suggestion> mainList = await suggestContext.Suggestion.ToListAsync();
      List<Suggestion> secondaryList = new List<Suggestion>();

      foreach (Suggestion suggestion in mainList)
      {
        foreach (string keyword2 in suggestion.Keywords.Split(","))
        {
          if (keyword == keyword2)
          {
            secondaryList.Add(suggestion);
          }
        }
      }
      return secondaryList;
    }
    public async Task<int> FindSuggestionNumber()
    {
      //IOrderedQueryable<Suggestion> data = suggestContext.Suggestion.OrderBy(x => x.Number);
      //Suggestion suggestion = await data.LastOrDefaultAsync().ConfigureAwait(false);
      int number = 0;
      foreach (Suggestion suggestion in suggestContext.Suggestion)
      {
        number++;
      }
      return number + 1;
      //DbSet<Suggestion> numbers = (DbSet<Suggestion>)suggestContext.Suggestions
      //                            .Where(x => x.Number > 0)
      //                            .Select(x => new { x.Number });
      //Suggestion suggestion = await numbers.OrderByDescending(x => x.Number).FirstOrDefaultAsync().ConfigureAwait(false);
      //int number = suggestion.Number;
      ////Suggestion suggestion = await suggestContext.Suggestions.LastOrDefaultAsync();
      //if (suggestion == null)
      //{
      //  Console.WriteLine("Error: Undefined Suggestion Query");
      //  return 0;
      //}
      //return suggestion.Number;
    }
    //Suggestion item = await suggestService.Items.FirstOrDefaultAsync(x => x.Title.Equals(title, StringComparison.OrdinalIgnoreCase)).ConfigureAwait(false);
    //return await 
  }
}
