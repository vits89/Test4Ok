using System.Collections.Generic;
using Test4Ok.AppCore.Models;

namespace Test4Ok.AppCore.Interfaces
{
    public interface INewsReader
    {
        IEnumerable<NewsModel> Read(string url);
    }
}
