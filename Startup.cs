using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace aspnet_webapi
{
    public static class Extensions
    {
        // helper to validate IOption and log its ToString() if valid
        public static bool IsValid<T>(this IOptions<T> options, ILogger<Startup> logger) where T : class
        {
            try
            {
                logger.LogInformation($"{typeof(T).Name}{Environment.NewLine}{options.Value}");
                return true;
            }
            catch (OptionsValidationException ex)
            {
                logger.LogInformation($"Error validating {typeof(T).Name}");
                foreach (var failure in ex.Failures)
                {
                    logger.LogError(failure);
                }
                return false;
            }
        }

    }
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            // added for this test
            var lf = LoggerFactory.Create(builder => { builder.AddConsole(); } );
            _logger = lf.CreateLogger<Startup>();
        }

        public IConfiguration Configuration { get; }
        private readonly ILogger<Startup> _logger;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "aspnet_webapi", Version = "v1" });
            });

            // added for this test
            services.AddOptions<OneTimeOptions>()
                .Bind(Configuration.GetSection(OneTimeOptions.Config))
                .ValidateDataAnnotations();
            services.AddOptions<SnapshotOptions>()
                .Bind(Configuration.GetSection(SnapshotOptions.Config))
                .ValidateDataAnnotations();
            services.AddOptions<MonitorOptions>()
                .Bind(Configuration.GetSection(MonitorOptions.Config))
                .ValidateDataAnnotations();

            // https://stackoverflow.com/questions/52258443/how-to-get-ioptions-in-configureservices-method-or-pass-ioptions-into-extension/52268943
            // Option 1 for getting IOption into singleton, can only use the class
            var oneTime = new OneTimeOptions();
            Configuration.GetSection(OneTimeOptions.Config).Bind(oneTime);

            // Option 2, can use IOptionsMonitor<MonitorOptions> or MonitorOptions as parameter
            // if use .Value, it will validate here.
            // but this generates ASP0000 warning and is not recommended by MS
            // see https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-5.0#recommendations
#pragma warning disable ASP0000
            using var serviceProvider = services.BuildServiceProvider();
#pragma warning restore ASP0000

            // need to validate here since calling .Value later
            var monitor = serviceProvider.GetRequiredService<IOptions<MonitorOptions>>();
            if (!monitor.IsValid(_logger))
            {
                throw new ArgumentException("One or more Options are invalid");
            }

            services.AddSingleton<ITestService>(
                new TestService(_logger, oneTime, monitor.Value) // constructed now
            );
            services.AddSingleton<ITestServiceInject,TestServiceInject>(); // constructed on first call so Configure can validate
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
                 IWebHostEnvironment env,
                 IOptions<OneTimeOptions> oneTime,
                 IOptions<SnapshotOptions> snapshot, // doc doesn't mention using this here, but it works
                 IOptions<MonitorOptions> monitor)
        {
            // added for this test
            // Validate here since otherwise it's when injected into controller or
            // service which could be hours later
            if (!oneTime.IsValid(_logger) ||
                !snapshot.IsValid(_logger) ||
                !monitor.IsValid(_logger))
            {
                throw new ArgumentException("One or more Options are invalid");
            }
            // end of code added for this test

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "aspnet_webapi v1"));
            }

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
