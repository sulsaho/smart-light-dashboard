using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightWebAPI.Controllers;
using LightWebAPI.Models;
using LightWebAPI.Repositories;
using LightWebAPI.ScheduleTask;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace LightWebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public string  MyAllowSpecificOrigins => "_myAllowSpecificOrigins";


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy( 
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000",
                            "http://localhost:5001").AllowAnyHeader().AllowAnyMethod();
                    });
            });
            
            services.AddScoped<ILightStateRepository, LightStateRepository>();
            services.AddScoped<LightStateController>();
            services.AddSingleton<IHostedService, LightOnTask>();
            services.AddSingleton<IHostedService, CaptureSunsetSunriseTimes>();
            services.AddSingleton<IHostedService, ScheduleSunrise>();
            services.AddSingleton<IHostedService, ScheduleSunset>();
            services.AddSingleton<IHostedService, ScheduleStateChange>();
            services.AddDbContext<LightStateContext>(o => o.UseSqlite("Data source=light.db"));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LightWebAPI", Version = "v1" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LightWebAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
