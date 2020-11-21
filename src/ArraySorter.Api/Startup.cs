using ArraySorter.Api.Middleware;
using ArraySorter.Domain;
using ArraySorter.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ArraySorter.ReadModels;
using ArraySorter.Infrastructure;
using System.Text.Json.Serialization;
using ArraySorter.Api.HostedService;
using ArraySorter.Api.Configuration;
using ArraySorter.Infrastructure.Database;

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
            var options = Configuration.Get<ApplicationOptions>();
            services.Configure<ApplicationOptions>(Configuration);

            services
                .AddControllers()
                .AddJsonOptions(options => {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ArraySorter.Api", Version = "v1" });
            });

            services.AddScoped<ISystemTime, SystemTimeProvider>();

            services.AddScoped<ICommandHandler<EnqueueCommand>, EnqueueCommandHandler>();

            services.AddScoped<IJobQueries, JobQueries>();

            services.AddScoped<IJobRepository, InMemoryJobRepository>();

            services.AddScoped<IJobProcessingService, JobProcessingService>();

            services.AddSingleton<ApplicationContext>(); // Mock Context

            services.AddHostedService<JobHostedService>();
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

            app.UseExceptionHandling();

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
