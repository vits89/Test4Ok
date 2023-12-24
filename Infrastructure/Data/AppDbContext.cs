using Microsoft.EntityFrameworkCore;
using Test4Ok.AppCore.Entities;

namespace Test4Ok.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<News> News { get; set; }
    public DbSet<NewsSource> NewsSources { get; set; }

    public AppDbContext(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }
}
