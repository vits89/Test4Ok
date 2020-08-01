using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Test4Ok.Infrastructure.Data;
using Test4Ok.WebApp.Models;

namespace Test4Ok.WebApp.Controllers
{
    public class NewsController : NewsBaseController
    {
        public NewsController(AppDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

        public async Task<IActionResult> Index(int? newsSource, bool? orderByDate, int page = 1)
        {
            newsSource ??= TempData["newsSource"] as int?;
            orderByDate ??= TempData["orderByDate"] as bool?;

            return View(await GetNewsAsync(newsSource, orderByDate, page));
        }

        [HttpPost]
        public IActionResult Index(RequestFormData requestFormData)
        {
            TempData["newsSource"] = requestFormData.NewsSource;
            TempData["orderByDate"] = requestFormData.OrderByDate;

            RouteData.Values.Clear();

            return RedirectToAction();
        }

        public IActionResult IndexApi()
        {
            return View();
        }
    }
}
