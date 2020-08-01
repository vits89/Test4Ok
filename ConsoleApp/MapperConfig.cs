using AutoMapper;
using Test4Ok.AppCore.Entities;
using Test4Ok.AppCore.MapperProfiles;
using Test4Ok.ConsoleApp.Models;

namespace Test4Ok.ConsoleApp
{
    public class MapperConfig
    {
        public static MapperConfiguration Configuration { get; private set; }

        public static void Initialize()
        {
            Configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<NewsProfile>();

                cfg.CreateMap<NewsSourceModel, NewsSource>();
            });
        }
    }
}
