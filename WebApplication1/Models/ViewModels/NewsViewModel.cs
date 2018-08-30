using ClassLibrary1.Models;

namespace WebApplication1.Models.ViewModels
{
    public class NewsViewModel
    {
        public News[] News { get; set; }
        public PagingInfoViewModel PagingInfo { get; set; }
        public RequestFormData RequestFormData { get; set; }
    }
}
