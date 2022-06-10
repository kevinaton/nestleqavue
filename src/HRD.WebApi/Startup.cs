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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NLog.Web;
using HRD.WebApi.Logging;
using HRD.WebApi.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using HRD.WebApi.Services;
using Microsoft.AspNetCore.Server.IISIntegration;

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
            // Add application services.
            services.AddTransient<IUserService, UserService>();
            
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IISDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = IISDefaults.AuthenticationScheme;

            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HRD.WebApi", Version = "v1" });
            });

            // Add framework services.
            services.AddDbContext<HRDContext>(options => options.
                UseSqlServer(Configuration.GetConnectionString("Default")));
            ////Authorization
            services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyNames.EditUsers, policy => policy.RequireClaim(ClaimTypes.Role, RoleNames.Admin));
                options.AddPolicy(PolicyNames.EditHRDs, policy => policy.RequireClaim(ClaimTypes.Role, RoleNames.Admin, RoleNames.DataEntry));
                options.AddPolicy(PolicyNames.ViewHRDs, policy => policy.RequireClaim(ClaimTypes.Role, RoleNames.Admin, RoleNames.DataEntry, RoleNames.ReportViewer));
            });
            services.AddScoped<IClaimsTransformation, ClaimsTransformer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            
            logger.LogInformation("Configure Application");
            var builder = WebApplication.CreateBuilder();

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HRD.WebApi v1"));

                app.UseCors(options => options.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader()); //TO DO: to remove
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
