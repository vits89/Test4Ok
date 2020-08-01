using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Test4Ok.AppCore.Entities;
using Test4Ok.AppCore.Interfaces;
using Test4Ok.ConsoleApp.Models;
using Test4Ok.Infrastructure.Data;
using Test4Ok.Infrastructure.Services;

namespace Test4Ok.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var newsSourceModels = FormNewsSources(args).ToArray();

            MapperConfig.Initialize();

            var mapper = MapperConfig.Configuration.CreateMapper();

            INewsReader newsReader = new NewsFeedReader();

            using (var dbContext = new AppDbContext(GetDbContextOptions()))
            {
                var newsSourceNames = newsSourceModels.Select(ns => ns.Name);
                var newsSources = dbContext.NewsSources.Where(ns => newsSourceNames.Contains(ns.Name)).ToList();

                foreach (var newsSourceModel in newsSourceModels)
                {
                    try
                    {
                        var newsModels = newsReader.Read(newsSourceModel.Url);

                        foreach (var newsModel in newsModels)
                        {
                            newsSourceModel.ReadOfNews++;

                            var alreadyExists = dbContext.News
                                .Any(n => (n.Title == newsModel.Title) && (n.PublishDate == newsModel.PublishDate));

                            if (alreadyExists) continue;

                            var news = mapper.Map<News>(newsModel);
                            var newsSource = newsSources.FirstOrDefault(ns => ns.Name == newsSourceModel.Name);

                            if (newsSource == null)
                            {
                                newsSource = mapper.Map<NewsSource>(newsSourceModel);

                                newsSources.Add(newsSource);
                            }

                            news.NewsSource = newsSource;

                            dbContext.News.Add(news);

                            newsSourceModel.SavedOfNews++;
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
            }
        }

        private static IEnumerable<NewsSourceModel> FormNewsSources(IList<string> values)
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

        private static DbContextOptions<AppDbContext> GetDbContextOptions()
        {
            var confBuilder = new ConfigurationBuilder();

            confBuilder.AddJsonFile("sharedappsettings.json", optional: true)
                .AddJsonFile("appsettings.json", optional: true);

            var conf = confBuilder.Build();

            var useSqlServer = conf.GetValue<bool>("UseSqlServer");
            var connString = conf.GetConnectionString(useSqlServer ? "SqlServer" : "Sqlite");

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            if (useSqlServer)
            {
                optionsBuilder.UseSqlServer(connString);
            }
            else
            {
                optionsBuilder.UseSqlite(connString);
            }

            return optionsBuilder.Options;
        }
    }
}
