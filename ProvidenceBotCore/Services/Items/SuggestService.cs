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
    Task<SortedList<Suggestion, int>> FindSuggestionByRelativity(DiscordUser author = null, string title = null, string[] keywords = null, int number = 0);
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
    public async Task<SortedList<Suggestion, int>> FindSuggestionByRelativity(DiscordUser author = null, string title = null, string[] keywords = null, int number = 0)
    {
      title = title.ToLower();
      List<Suggestion> mainList = await suggestContext.Suggestions.ToListAsync();
      SortedList<Suggestion, int> secondaryList = new SortedList<Suggestion, int>();

      foreach (Suggestion suggestion in mainList)
      {
        int relativityScore = 0;
        if (author != null && suggestion.Author == author)
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
          foreach (string keyword in suggestion.Keywords)
          {
            foreach (string keyword2 in keywords)
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
      List<Suggestion> mainList = await suggestContext.Suggestions.ToListAsync();
      List<Suggestion> secondaryList = new List<Suggestion>();
      foreach (Suggestion suggestion in mainList)
      {
        if (suggestion.Author == author)
        {
          secondaryList.Add(suggestion);
        }
      }
      return secondaryList;
    }
    public async Task<Suggestion> FindSuggestionByNumber(CommandContext context, int number)
    {
      List<Suggestion> mainList = await suggestContext.Suggestions.ToListAsync();
      Suggestion suggestion = null;
      foreach (Suggestion suggestion2 in mainList)
      {
        if (suggestion.Number == number)
        {
          suggestion = suggestion2;
        }
      }
      return suggestion != null ? suggestion : null;
    }
    public async Task<List<Suggestion>> FindSuggestionByTitle(string title)
    {
      List<Suggestion> mainList = await suggestContext.Suggestions.ToListAsync();
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
      List<Suggestion> mainList = await suggestContext.Suggestions.ToListAsync();
      List<Suggestion> secondaryList = new List<Suggestion>();
      foreach (Suggestion suggestion in mainList)
      {
        for (int i = 0; i <= suggestion.Keywords.Length - 1; i++)
        {
          if (keyword == suggestion.Keywords[i])
          {
            secondaryList.Add(suggestion);
          }
        }
      }
      return secondaryList;
    }
    public async Task<int> FindSuggestionNumber()
    {
      Suggestion suggestion = await suggestContext.Suggestions.LastOrDefaultAsync();
      if (suggestion == null)
      {
        return 0;
      }
      else
      {
        int number = suggestion.Number;
        return number;
      }
    }
    //Suggestion item = await suggestService.Items.FirstOrDefaultAsync(x => x.Title.Equals(title, StringComparison.OrdinalIgnoreCase)).ConfigureAwait(false);
    //return await 
  }
}
