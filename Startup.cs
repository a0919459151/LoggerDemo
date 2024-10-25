using LoggerDemo.Clients.Common;
using LoggerDemo.Clients.DogClient;
using LoggerDemo.Loggers;
using LoggerDemo.Middlewares;
using LoggerDemo.Repositories;
using LoggerDemo.UnitOfWorks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Data;

namespace LoggerDemo
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LoggerDemo", Version = "v1" });
            });


            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            var connection = new SqlConnection(connectionString);
            // LoggerManager
            LoggerManager.Instance
                .CreateLogger(Configuration)
                .CreateApiLogger(new ApiLogDatabaseSink(new ApiLogRepository(new SqlConnection(connectionString))))
                .CreateHttpClientLogger(new HttpClientLogDatabaseSink(new HttpClientLogRepository(new SqlConnection(connectionString))));

            // Dapper
            services.AddScoped<IDbConnection>(sp => connection);

            // UnitOfWorks
            services.AddScoped<UnitOfWork>();

            // Repositories
            services.AddScoped<DogRepository>();
            services.AddScoped<CatRepository>();

            // DogClient
            services.AddScoped<HttpClientLogHandler>();
            services.AddHttpClient(
                    DogClient.HttpClientName,
                    client =>
                    {
                        client.BaseAddress = new Uri(DogClient.BaseUrl);
                    }
                )
                .AddHttpMessageHandler<HttpClientLogHandler>();
            services.AddScoped<DogClient>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LoggerDemo v1"));
            }

            // exception handle middleware
            app.UseMiddleware<GlobalExceptionHandleMiddleware>();

            // api logging middleware
            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
