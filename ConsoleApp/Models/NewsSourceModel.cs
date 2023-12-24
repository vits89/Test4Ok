namespace Test4Ok.ConsoleApp.Models;

public class NewsSourceModel
{
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;

    public int NewsRead { get; set; }
    public int NewsSaved { get; set; }

    public override string ToString()
    {
        return $"Источник: {Name}; прочитано новостей: {NewsRead}; сохранено новостей: {NewsSaved}.";
    }
}
