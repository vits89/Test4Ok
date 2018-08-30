using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleApp1.Models
{
    public class NewsSource : ClassLibrary1.Models.NewsSource
    {
        [NotMapped]
        public int ReadOfNews { get; set; }

        [NotMapped]
        public int SavedOfNews { get; set; }

        public override string ToString() => $"Источник: {Name}; прочитано новостей: {ReadOfNews}; сохранено новостей: {SavedOfNews}.";
    }
}
