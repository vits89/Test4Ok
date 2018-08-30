using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ConsoleApp1.Models;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var newsSources = FormNewsSources(args);

            using (var dbContext = new AppDbContext(GetDbContextOptions()))
            {
                foreach (var source in newsSources)
                {
                    var newsSource = dbContext.NewsSources.FirstOrDefault(ns => ns.Name == source.Name);

                    if (newsSource != null)
                    {
                        source.Id = newsSource.Id;
                    }

                    try
                    {
                        var document = XDocument.Load(source.Url);

                        foreach (var item in document.Descendants("item"))
                        {
                            source.ReadOfNews++;

                            var news = new ClassLibrary1.Models.News
                            {
                                Title = item.Element("title").Value,
                                PublishDate = DateTime.Parse(item.Element("pubDate").Value),
                                Description = item.Element("description").Value,
                                Url = item.Element("link").Value,
                                NewsSource = source
                            };

                            var alreadyExists = dbContext.News.Any(n => (n.Title == news.Title) && (n.PublishDate == news.PublishDate));

                            if (!alreadyExists)
                            {
                                dbContext.News.Add(news);

                                source.SavedOfNews++;
                            }
                        }

                        if (source.SavedOfNews > 0)
                        {
                            dbContext.SaveChanges();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    finally
                    {
                        Console.WriteLine(source);
                    }
                }
            }
        }

        private static NewsSource[] FormNewsSources(string[] args)
        {
            var newsSources = new List<NewsSource>();

            for (var i = 0; i <= Math.Floor((decimal) args.Length / 2); i += 2)
            {
                newsSources.Add(new NewsSource
                {
                    Name = args[i],
                    Url = args[i + 1]
                });
            }

            return newsSources.ToArray();
        }

        private static DbContextOptions<AppDbContext> GetDbContextOptions()
        {
            var confBuilder = new ConfigurationBuilder();

            confBuilder.AddJsonFile("sharedappsettings.json", optional: true);
            confBuilder.AddJsonFile("appsettings.json", optional: true);

            var conf = confBuilder.Build();

            var useSqlServer = conf.GetValue<bool>("UseSqlServer");
            var connString = conf.GetConnectionString(useSqlServer ? "SqlServer" : "SqliteServer");

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
