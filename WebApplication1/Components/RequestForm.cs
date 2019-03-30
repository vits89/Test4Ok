using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Models;

namespace WebApplication1.Components
{
    public class RequestForm : ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public RequestForm(AppDbContext dbContext) => _dbContext = dbContext;

        public IViewComponentResult Invoke(RequestFormData requestFormData)
        {
            var newsSources = _dbContext.NewsSources.OrderBy(ns => ns.Name);

            ViewBag.NewsSources = new SelectList(newsSources,
                nameof(ClassLibrary1.Models.NewsSource.Id),
                nameof(ClassLibrary1.Models.NewsSource.Name));

            return View(requestFormData);
        }
    }
}
