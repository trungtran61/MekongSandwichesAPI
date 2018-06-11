using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MekongSandwichesAPI
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
            AppSettings settings;
            settings = GetAppSettings();
            services.AddSingleton<AppSettings>(settings);
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {     
            AppSettings settings;
            settings = GetAppSettings();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

 app.UseCors(
              options => options.WithOrigins(
                settings.ClientUrl).AllowAnyMethod().AllowAnyHeader()
            );
            app.UseMvc();
        }

        public AppSettings GetAppSettings()
        {
            AppSettings settings = new AppSettings();
            settings.MSConnectionString = Configuration["ConnectionStrings:MekongSandwiches"];
            settings.ClientUrl = Configuration["AppSettings:ClientUrl"];
            return settings;
        }
    }
}
