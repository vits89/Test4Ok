using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test4Ok.Infrastructure.Data;
using Test4Ok.WebApp.Models;
using Test4Ok.WebApp.ViewModels;

namespace Test4Ok.WebApp.Controllers;

public abstract class NewsBaseController(AppDbContext dbContext, IMapper mapper) : Controller
{
    protected const int newsPerPage = 10;

    protected async Task<NewsListViewModel> GetNewsAsync(int? newsSource, bool? orderByDate, int page)
    {
        var pagingInfo = new PagingInfoViewModel
        {
            CurrentPage = page,
            ItemsPerPage = newsPerPage
        };
        var requestFormData = new RequestFormData
        {
            NewsSource = newsSource
        };

        if (orderByDate.HasValue)
        {
            requestFormData.OrderByDate = orderByDate.Value;
        }

        var newsListViewModel = new NewsListViewModel
        {
            News = Enumerable.Empty<NewsViewModel>(),
            PagingInfo = pagingInfo,
            RequestFormData = requestFormData
        };

        if (!(await dbContext.NewsSources.AnyAsync()))
        {
            return newsListViewModel;
        }

        var news = dbContext.News.Include(n => n.NewsSource).AsQueryable();

        if (requestFormData.NewsSource.HasValue)
        {
            if ((await dbContext.NewsSources.FindAsync(requestFormData.NewsSource.Value)) == null)
            {
                return newsListViewModel;
            }

            news = news.Where(n => n.NewsSource.Id == requestFormData.NewsSource.Value);
        }

        pagingInfo.TotalItems = await news.CountAsync();

        news = requestFormData.OrderByDate ?
            news.OrderByDescending(n => n.PublishDate) :
            news.OrderBy(n => n.NewsSource.Name);

        news = news.Skip((pagingInfo.CurrentPage - 1) * newsPerPage).Take(newsPerPage);

        newsListViewModel.News = await news.ProjectTo<NewsViewModel>(mapper.ConfigurationProvider).ToArrayAsync();

        return newsListViewModel;
    }
}
