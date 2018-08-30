using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class AppDbContext : ClassLibrary1.Models.AppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) => Database.EnsureCreated();
    }
}
