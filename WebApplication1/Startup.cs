using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Models;

namespace WebApplication1
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration conf) => Configuration = conf;

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
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: null,
                    template: "NewsSource{newsSource:int}/Page{page:int}/{orderByDate:bool?}",
                    defaults: new { controller = "News", action = "Index" }
                );
                routes.MapRoute(
                    name: null,
                    template: "Page{page:int}/{orderByDate:bool?}",
                    defaults: new { controller = "News", action = "Index" }
                );
                routes.MapRoute(
                    name: "default",
                    template: "{controller=News}/{action=Index}/{page?}"
                );
            });
        }
    }
}
