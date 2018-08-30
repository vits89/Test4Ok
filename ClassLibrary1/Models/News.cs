using System;
using System.ComponentModel.DataAnnotations;

namespace ClassLibrary1.Models
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? PublishDate { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }

        [Required]
        public NewsSource NewsSource { get; set; }
    }
}
