namespace Test4Ok.ConsoleApp.Models
{
    public class NewsSourceModel
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public int ReadOfNews { get; set; }
        public int SavedOfNews { get; set; }

        public override string ToString()
        {
            return $"Источник: {Name}; прочитано новостей: {ReadOfNews}; сохранено новостей: {SavedOfNews}.";
        }
    }
}
