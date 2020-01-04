using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public abstract class NewsBaseController : Controller
    {
        protected const int NEWS_PER_PAGE = 10;

        protected readonly AppDbContext _dbContext;

        public NewsBaseController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected NewsViewModel GetNews(int? newsSource, bool? orderByDate, int page)
        {
            var pagingInfo = new PagingInfoViewModel
            {
                CurrentPage = page,
                ItemsPerPage = NEWS_PER_PAGE
            };
            var requestFormData = new RequestFormData
            {
                NewsSource = newsSource
            };

            if (orderByDate.HasValue)
            {
                requestFormData.OrderByDate = orderByDate.Value;
            }

            var newsViewModel = new NewsViewModel
            {
                News = Array.Empty<ClassLibrary1.Models.News>(),
                PagingInfo = pagingInfo,
                RequestFormData = requestFormData
            };

            if (!_dbContext.NewsSources.Any())
            {
                return newsViewModel;
            }

            var news = _dbContext.News.Include(n => n.NewsSource).AsQueryable();

            if (requestFormData.NewsSource.HasValue)
            {
                if (_dbContext.NewsSources.Find(requestFormData.NewsSource.Value) == null)
                {
                    return newsViewModel;
                }

                news = news.Where(n => n.NewsSource.Id == requestFormData.NewsSource.Value);
            }

            pagingInfo.TotalItems = news.Count();

            news = requestFormData.OrderByDate ?
                news.OrderByDescending(n => n.PublishDate) :
                news.OrderBy(n => n.NewsSource.Name);

            news = news.Skip((pagingInfo.CurrentPage - 1) * NEWS_PER_PAGE).Take(NEWS_PER_PAGE);

            newsViewModel.News = news.ToArray();

            return newsViewModel;
        }
    }
}
