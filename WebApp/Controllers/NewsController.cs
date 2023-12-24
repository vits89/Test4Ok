using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Test4Ok.Infrastructure.Data;
using Test4Ok.WebApp.Models;

namespace Test4Ok.WebApp.Controllers;

public class NewsController(AppDbContext dbContext, IMapper mapper) : NewsBaseController(dbContext, mapper)
{
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
