using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/news")]
    public class NewsApiController : NewsBaseController
    {
        public NewsApiController(AppDbContext dbContext) : base(dbContext) { }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public NewsViewModel Index([FromQuery] RequestData requestData)
        {
            return GetNews(requestData.NewsSource, requestData.OrderByDate, requestData.Page);
        }
    }
}
