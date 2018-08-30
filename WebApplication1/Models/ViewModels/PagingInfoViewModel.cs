using System;

namespace WebApplication1.Models.ViewModels
{
    public class PagingInfoViewModel
    {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalItems { get; set; }

        public int TotalPages => (int) Math.Ceiling((decimal) TotalItems / ItemsPerPage);
    }
}
