using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Test4Ok.Infrastructure.Data;
using Test4Ok.WebApp.Models;
using Test4Ok.WebApp.ViewModels;

namespace Test4Ok.WebApp.Controllers
{
    [ApiController]
    [Route("api/news")]
    public class NewsApiController : NewsBaseController
    {
        public NewsApiController(AppDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<NewsListViewModel> Index([FromQuery]RequestData requestData)
        {
            return await GetNewsAsync(requestData.NewsSource, requestData.OrderByDate, requestData.Page);
        }
    }
}
