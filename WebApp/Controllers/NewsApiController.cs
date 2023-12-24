using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Test4Ok.Infrastructure.Data;
using Test4Ok.WebApp.Models;
using Test4Ok.WebApp.ViewModels;

namespace Test4Ok.WebApp.Controllers;

[ApiController]
[Route("api/news")]
public class NewsApiController(AppDbContext dbContext, IMapper mapper) : NewsBaseController(dbContext, mapper)
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<NewsListViewModel> Index([FromQuery] RequestData requestData)
    {
        return await GetNewsAsync(requestData.NewsSource, requestData.OrderByDate, requestData.Page);
    }
}
