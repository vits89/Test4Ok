using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsApiController : NewsBaseController
    {
        public NewsApiController(AppDbContext dbContext) : base(dbContext) { }

        [HttpGet]
        [ProducesResponseType(200)]
        public NewsViewModel Index([FromQuery] RequestData requestData)
        {
            return GetNews(requestData.NewsSource, requestData.OrderByDate, requestData.Page);
        }
    }
}
