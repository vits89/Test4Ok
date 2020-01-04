using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApplication1.Models;

namespace WebApplication1
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var useSqlServer = Configuration.GetValue<bool>("UseSqlServer");
            var connString = Configuration.GetConnectionString(useSqlServer ? "SqlServer" : "Sqlite");

            services.AddDbContext<AppDbContext>(options =>
            {
                if (useSqlServer)
                {
                    options.UseSqlServer(connString);
                }
                else
                {
                    options.UseSqlite(connString);
                }
            });
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: null,
                    pattern: "NewsSource{newsSource:int}/Page{page:int}/{orderByDate:bool?}",
                    defaults: new { controller = "News", action = "Index" }
                );
                endpoints.MapControllerRoute(
                    name: null,
                    pattern: "Page{page:int}/{orderByDate:bool?}",
                    defaults: new { controller = "News", action = "Index" }
                );
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=News}/{action=Index}/{page?}"
                );
            });
        }
    }
}
