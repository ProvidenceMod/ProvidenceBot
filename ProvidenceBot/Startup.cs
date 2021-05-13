using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProvidenceBotDAL.Database;

namespace ProvidenceBot
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services) 
    {
      services.AddDbContext<SuggestContext>(options => 
      {
        options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=SuggestContext;Trusted_Connection=True;MultipleActiveResults=true",
          x => x.MigrationsAssembly("ProvidenceBotDAL.Migrations"));
      });
      ServiceProvider serviceProvider = services.BuildServiceProvider();
      Bot bot = new Bot(serviceProvider);
      services.AddSingleton(bot);
    }
    public void Configure(IApplicationBuilder application, IWebHostEnvironment environment)
    {

    }
  }
}