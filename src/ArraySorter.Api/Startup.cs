using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArraySorter.Api.Middleware;
using ArraySorter.Domain;
using ArraySorter.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Data.Common;
using ArraySorter.ReadModels;
using ArraySorter.Infrastructure;
using System.Text.Json.Serialization;

namespace ArraySorter.Api
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
            services
                .AddControllers()
                .AddJsonOptions(options => {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ArraySorter.Api", Version = "v1" });
            });

            services.AddScoped<ISystemDateTime, SystemTimeProvider>();

            services.AddScoped<ICommandHandler<EnqueueCommand>, EnqueueCommandHandler>();

            services.AddScoped<IJobQueries, JobQueries>();

            services.AddScoped<IJobRepository, InMemoryJobRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ArraySorter.Api v1"));
            }

            app.UseAmIUp();

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
