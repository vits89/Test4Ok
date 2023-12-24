using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Test4Ok.AppCore.Entities;
using Test4Ok.ConsoleApp;
using Test4Ok.ConsoleApp.Models;
using Test4Ok.Infrastructure.Data;
using Test4Ok.Infrastructure.Services;

var newsSourceModels = FormNewsSources(args).ToArray();

MapperConfig.Initialize();

var mapper = MapperConfig.Configuration!.CreateMapper();

var newsReader = new NewsFeedReader();

using var dbContext = new AppDbContext(GetDbContextOptions());

var newsSourceNames = newsSourceModels.Select(ns => ns.Name);
var newsSources = dbContext.NewsSources.Where(ns => newsSourceNames.Contains(ns.Name)).ToDictionary(ns => ns.Name);

foreach (var newsSourceModel in newsSourceModels)
{
    try
    {
        var newsModels = newsReader.Read(newsSourceModel.Url);

        foreach (var newsModel in newsModels)
        {
            newsSourceModel.NewsRead++;

            var alreadyExists = dbContext.News
                .Any(n => n.Title == newsModel.Title && n.PublishDate == newsModel.PublishDate);

            if (alreadyExists)
            {
                continue;
            }

            var news = mapper.Map<News>(newsModel);
            var newsSource = newsSources.GetValueOrDefault(newsSourceModel.Name);

            if (newsSource is null)
            {
                newsSource = mapper.Map<NewsSource>(newsSourceModel);

                newsSources.Add(newsSource.Name, newsSource);
            }

            news.NewsSource = newsSource;

            dbContext.News.Add(news);

            newsSourceModel.NewsSaved++;
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
    finally
    {
        Console.WriteLine(newsSourceModel);
    }
}

dbContext.SaveChanges();

static IEnumerable<NewsSourceModel> FormNewsSources(IList<string> values)
{
    for (var i = 0; i <= (int)Math.Floor((double)values.Count / 2); i += 2)
    {
        yield return new NewsSourceModel
        {
            Name = values[i],
            Url = values[i + 1]
        };
    }
}

static DbContextOptions<AppDbContext> GetDbContextOptions()
{
    var configurationBuilder = new ConfigurationBuilder();

    configurationBuilder
        .AddJsonFile("sharedappsettings.json", optional: true)
        .AddJsonFile("appsettings.json", optional: true);

    var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

    if (!string.IsNullOrWhiteSpace(env))
    {
        configurationBuilder.AddJsonFile($"appsettings.{env}.json", optional: true);
    }

    var configuration = configurationBuilder.Build();

    var useSqlServer = configuration.GetValue<bool>("UseSqlServer");
    var connectionString = configuration.GetConnectionString(useSqlServer ? "SqlServer" : "Sqlite");

    var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

    if (useSqlServer)
    {
        optionsBuilder.UseSqlServer(connectionString);
    }
    else
    {
        optionsBuilder.UseSqlite(connectionString);
    }

    return optionsBuilder.Options;
}
