using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.FileProviders;
using Test4Ok.Infrastructure.Data;

var assembly = Assembly.GetExecutingAssembly();

var builder = WebApplication.CreateBuilder(args);

var index = builder.Configuration.Sources.ToList().FindIndex(s => s is JsonConfigurationSource);

var configurationSource = new JsonConfigurationSource
{
    Path = "sharedappsettings.json",
    Optional = true,
    ReloadOnChange = true,
    FileProvider = new PhysicalFileProvider(Path.GetDirectoryName(assembly.Location)!)
};

builder.Configuration.Sources.Insert(index, configurationSource);

var useSqlServer = builder.Configuration.GetValue<bool>("UseSqlServer");
var connectionString = builder.Configuration.GetConnectionString(useSqlServer ? "SqlServer" : "Sqlite");

builder.Services.AddAutoMapper(assembly);
builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (useSqlServer)
    {
        options.UseSqlServer(connectionString);
    }
    else
    {
        options.UseSqlite(connectionString);
    }
});
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStatusCodePages();
app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: string.Empty,
    pattern: "NewsSource{newsSource:int}/Page{page:int}/{orderByDate:bool?}",
    defaults: new { controller = "News", action = "Index" }
);
app.MapControllerRoute(
    name: string.Empty,
    pattern: "Page{page:int}/{orderByDate:bool?}",
    defaults: new { controller = "News", action = "Index" }
);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=News}/{action=Index}/{page?}"
);

app.Run();
