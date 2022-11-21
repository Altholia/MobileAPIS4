using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MobileAPIS4.Entities;
using MobileAPIS4.Helpers;
using MobileAPIS4.Services;
using Newtonsoft.Json.Serialization;

namespace MobileAPIS4
{
    public class Startup
    {
        private readonly ILoggerFactory _logger = LoggerFactory.Create(option =>
        {
            option.AddFilter((category, level) =>
                category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information);
            option.AddConsole();
        });

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers(setUp =>
            {
                setUp.ReturnHttpNotAcceptable = true;
            }).AddNewtonsoftJson(setUp =>
            {
                setUp.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }).ConfigureApiBehaviorOptions(setUp =>
            {
                setUp.InvalidModelStateResponseFactory = context =>
                {
                    var problem = new ValidationProblemDetails(context.ModelState)
                    {
                        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                        Title = "模型验证未通过",
                        Status = StatusCodes.Status422UnprocessableEntity,
                        Detail = "详情请看“error”",
                        Instance = context.HttpContext.Request.Path
                    };
                    problem.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);
                    return new UnprocessableEntityObjectResult(problem)
                    {
                        ContentTypes = { "application/problemDetails+json" }
                    };
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MobileAPIS4", Version = "v1" });
            });

            services.AddDbContext<S4Context>(setUp =>
            {
                setUp.UseSqlServer(Configuration.GetConnectionString("ConnString"));
                setUp.UseLoggerFactory(_logger);
            });

            services.AddLogging();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddTransient<IStaffControllerService,StaffControllerService>();
            services.AddTransient<ITransporationTaskControllerService,TransporationTaskControllerService>();
            services.AddTransient<IProvinceControllerService, ProvinceControllerService>();
            services.AddTransient<IVehicleControllerService,VehicleControllerService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MobileAPIS4 v1"));
            }

            ServiceLocator.Instance = app.ApplicationServices;

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
