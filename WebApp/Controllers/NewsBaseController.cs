using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Test4Ok.Infrastructure.Data;
using Test4Ok.WebApp.Models;
using Test4Ok.WebApp.ViewModels;

namespace Test4Ok.WebApp.Controllers
{
    public abstract class NewsBaseController : Controller
    {
        protected const int NEWS_PER_PAGE = 10;

        protected readonly AppDbContext _dbContext;
        protected readonly IMapper _mapper;

        public NewsBaseController(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        protected async Task<NewsListViewModel> GetNewsAsync(int? newsSource, bool? orderByDate, int page)
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

            var newsListViewModel = new NewsListViewModel
            {
                News = Enumerable.Empty<NewsViewModel>(),
                PagingInfo = pagingInfo,
                RequestFormData = requestFormData
            };

            if (!(await _dbContext.NewsSources.AnyAsync()))
            {
                return newsListViewModel;
            }

            var news = _dbContext.News.Include(n => n.NewsSource).AsQueryable();

            if (requestFormData.NewsSource.HasValue)
            {
                if ((await _dbContext.NewsSources.FindAsync(requestFormData.NewsSource.Value)) == null)
                {
                    return newsListViewModel;
                }

                news = news.Where(n => n.NewsSource.Id == requestFormData.NewsSource.Value);
            }

            pagingInfo.TotalItems = await news.CountAsync();

            news = requestFormData.OrderByDate ?
                news.OrderByDescending(n => n.PublishDate) :
                news.OrderBy(n => n.NewsSource.Name);

            news = news.Skip((pagingInfo.CurrentPage - 1) * NEWS_PER_PAGE).Take(NEWS_PER_PAGE);

            newsListViewModel.News = await news.ProjectTo<NewsViewModel>(_mapper.ConfigurationProvider).ToArrayAsync();

            return newsListViewModel;
        }
    }
}
