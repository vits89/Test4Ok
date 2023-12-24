using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Test4Ok.AppCore.Entities;
using Test4Ok.Infrastructure.Data;
using Test4Ok.WebApp.Models;

namespace Test4Ok.WebApp.Components;

public class RequestForm(AppDbContext dbContext) : ViewComponent
{
    public IViewComponentResult Invoke(RequestFormData requestFormData)
    {
        var newsSources = dbContext.NewsSources.OrderBy(ns => ns.Name);

        ViewBag.NewsSources = new SelectList(newsSources, nameof(NewsSource.Id), nameof(NewsSource.Name));

        return View(requestFormData);
    }
}
