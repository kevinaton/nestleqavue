using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HRD.WebApi.Data;

namespace HRD.WebApi
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HRD.WebApi", Version = "v1" });
            });

            var corsOrigins = GetCorsOrigins();
            if (corsOrigins.Any())
            {
                services.AddCors(options =>
                {
                    options.AddPolicy("default", policy =>
                    {
                        policy.WithOrigins(corsOrigins)
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
                });
            }

            // Add framework services.
            services.AddDbContext<HRDContext>(options => options.
                UseSqlServer(Configuration.GetConnectionString("Default")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HRD.WebApi v1"));

            }

            app.UseCors("default");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private string[] GetCorsOrigins()
        {
            return Configuration["CorsOrigins"]?.Split(";").Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim()).ToArray() ?? new string[0];
        }
    }
}
