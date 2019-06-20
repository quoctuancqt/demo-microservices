using System;
using Core.Extensions;
using Demo.EventBus.Abstractions;
using Demo.EventBus.Extensions;
using Demo.Infrastructure.Extensions;
using Demo.Infrastructure.MongoDb;
using Demo.NotificationService.IntegrationEvents.EventHandling;
using Demo.NotificationService.IntegrationEvents.Events;
using JwtTokenServer.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.NotificationService
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
            services.AddScoped(sp => new MongoFactory(Configuration));

            services.AddServices();

            services.AddHttpContextAccessor();

            services.AddSwashbuckle();

            services.JWTAddAuthentication(Configuration);

            services.AddHttpClient("GatewayClient", config =>
            {
                config.BaseAddress = new Uri(Configuration.GetValue<string>("GatewayApi"));
            });

            services.AddEventBus(Configuration);

            services.AddTransient<NotificationIntegrationEventHandler>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwashbuckle();

            app.UseAuthentication();

            app.RegisterEventBusHandler();

            app.UseMvc();
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetService<IEventBus>();

            eventBus.Subscribe<NotificationIntegrationEvent, NotificationIntegrationEventHandler>();
        }
    }
}
