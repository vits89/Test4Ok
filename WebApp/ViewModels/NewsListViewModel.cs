using System.Collections.Generic;
using Test4Ok.WebApp.Models;

namespace Test4Ok.WebApp.ViewModels
{
    public class NewsListViewModel
    {
        public IEnumerable<NewsViewModel> News { get; set; }
        public PagingInfoViewModel PagingInfo { get; set; }
        public RequestFormData RequestFormData { get; set; }
    }
}
