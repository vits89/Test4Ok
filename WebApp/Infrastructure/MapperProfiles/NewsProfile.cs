using AutoMapper;
using Test4Ok.AppCore.Entities;
using Test4Ok.WebApp.ViewModels;

namespace Test4Ok.WebApp.Infrastructure.MapperProfiles;

public class NewsProfile : Profile
{
    public NewsProfile()
    {
        CreateMap<News, NewsViewModel>()
            .ForMember(dst => dst.NewsSource, opts => opts.MapFrom(src => src.NewsSource.Name));
    }
}
