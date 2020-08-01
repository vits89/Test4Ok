using AutoMapper;
using Test4Ok.AppCore.Entities;
using Test4Ok.AppCore.Models;

namespace Test4Ok.AppCore.MapperProfiles
{
    public class NewsProfile : Profile
    {
        public NewsProfile()
        {
            CreateMap<NewsModel, News>();
        }
    }
}
