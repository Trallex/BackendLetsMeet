using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendLetsMeet.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BackendLetsMeet
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("LetsMeetDBConnection")));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AppDBContext>();

            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IGroupRepositoryp, GroupRepository>();
            services.AddScoped<IDaysRepository, DaysRepository>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDBContext>();
                context.Database.EnsureCreated();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Values}/{action=Tos}");
            });
        }
    }
}
