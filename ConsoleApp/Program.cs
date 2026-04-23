using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Test4Ok.AppCore.Entities;
using Test4Ok.ConsoleApp;
using Test4Ok.ConsoleApp.Models;
using Test4Ok.Infrastructure.Data;
using Test4Ok.Infrastructure.Services;

var newsSourceModels = args
    .Chunk(2)
    .Select(values => new NewsSourceModel
    {
        Name = values[0],
        Url = values[1]
    })
    .ToList();

var configuration = BuildConfiguration();

MapperConfig.Initialize(configuration, LoggerFactory.Create(builder => builder.AddConsole()));

var mapper = MapperConfig.Configuration!.CreateMapper();

var newsReader = new NewsFeedReader();

using var dbContext = new AppDbContext(GetDbContextOptions(configuration));

var newsSourceNames = newsSourceModels.Select(ns => ns.Name).ToList();

var newsSources =
    await dbContext.NewsSources.Where(ns => newsSourceNames.Contains(ns.Name)).ToDictionaryAsync(ns => ns.Name);

foreach (var newsSourceModel in newsSourceModels)
{
    try
    {
        var newsModels = newsReader.Read(newsSourceModel.Url);

        foreach (var newsModel in newsModels)
        {
            newsSourceModel.NewsRead++;

            var alreadyExists = await dbContext.News.AnyAsync(
                n => n.Title == newsModel.Title && n.PublishDate == newsModel.PublishDate);

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
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
    finally
    {
        Console.WriteLine(newsSourceModel);
    }
}

await dbContext.SaveChangesAsync();

static IConfiguration BuildConfiguration()
{
    var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

    var configurationBuilder = new ConfigurationBuilder();

    configurationBuilder
        .AddJsonFile("sharedappsettings.json", optional: true)
        .AddJsonFile("appsettings.json", optional: true);

    if (!string.IsNullOrWhiteSpace(env))
    {
        configurationBuilder.AddJsonFile($"appsettings.{env}.json", optional: true);
    }

    return configurationBuilder.Build();
}

static DbContextOptions<AppDbContext> GetDbContextOptions(IConfiguration configuration)
{
    var useSqlite = configuration.GetValue<bool>("UseSqlite");
    var connectionString = configuration.GetConnectionString(useSqlite ? "Sqlite" : "SqlServer");

    var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

    if (useSqlite)
    {
        optionsBuilder.UseSqlite(connectionString);
    }
    else
    {
        optionsBuilder.UseSqlServer(connectionString);
    }

    return optionsBuilder.Options;
}
