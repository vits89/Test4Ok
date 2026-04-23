using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Test4Ok.AppCore.Entities;
using Test4Ok.AppCore.MapperProfiles;
using Test4Ok.ConsoleApp.Models;

namespace Test4Ok.ConsoleApp;

public static class MapperConfig
{
    public static MapperConfiguration? Configuration { get; private set; }

    public static void Initialize(IConfiguration configuration, ILoggerFactory loggerFactory)
    {
        Configuration = new MapperConfiguration(
            cfg =>
            {
                cfg.LicenseKey = configuration["AutoMapperLicenseKey"];

                cfg.AddProfile<NewsProfile>();

                cfg.CreateMap<NewsSourceModel, NewsSource>();
            },
            loggerFactory);
    }
}
