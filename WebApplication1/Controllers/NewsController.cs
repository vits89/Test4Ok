using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class NewsController : NewsBaseController
    {
        public NewsController(AppDbContext dbContext) : base(dbContext) { }

        public IActionResult Index(int? newsSource, bool? orderByDate, int page = 1)
        {
            newsSource ??= TempData["newsSource"] as int?;
            orderByDate ??= TempData["orderByDate"] as bool?;

            return View(GetNews(newsSource, orderByDate, page));
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
