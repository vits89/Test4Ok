using Microsoft.EntityFrameworkCore;

namespace ClassLibrary1.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<News> News { get; set; }
        public DbSet<NewsSource> NewsSources { get; set; }

        public AppDbContext(DbContextOptions options) : base(options) { }
    }
}
