using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Test4Ok.AppCore.Interfaces;
using Test4Ok.AppCore.Models;

namespace Test4Ok.Infrastructure.Services
{
    public class NewsFeedReader : INewsReader
    {
        public IEnumerable<NewsModel> Read(string url)
        {
            var document = XDocument.Load(url);

            foreach (var item in document.Descendants("item"))
            {
                yield return new NewsModel
                {
                    Title = item.Element("title").Value,
                    PublishDate = DateTime.Parse(item.Element("pubDate").Value),
                    Description = item.Element("description").Value,
                    Url = item.Element("link").Value
                };
            }
        }
    }
}
