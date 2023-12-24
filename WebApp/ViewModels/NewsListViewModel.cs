using Test4Ok.WebApp.Models;

namespace Test4Ok.WebApp.ViewModels;

public class NewsListViewModel
{
    public IEnumerable<NewsViewModel> News { get; set; } = Enumerable.Empty<NewsViewModel>();
    public PagingInfoViewModel PagingInfo { get; set; } = new();
    public RequestFormData RequestFormData { get; set; } = new();
}
