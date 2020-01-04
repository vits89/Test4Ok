using System.Collections.Generic;
using ClassLibrary1.Models;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class NewsViewModel
    {
        public IEnumerable<News> News { get; set; }
        public PagingInfoViewModel PagingInfo { get; set; }
        public RequestFormData RequestFormData { get; set; }
    }
}
