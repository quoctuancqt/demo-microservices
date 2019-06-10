﻿using System;
using Core.Extensions;
using Demo.EventBus.Abstractions;
using Demo.EventBus.Extensions;
using Demo.Infrastructure.Extensions;
using Demo.Infrastructure.MongoDb;
using Demo.NotificationService.Background;
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

            //var hcBuilder = services.AddHealthChecks();

            //if (Configuration.GetValue<bool>("AzureServiceBusEnabled"))
            //{
            //    hcBuilder
            //        .AddAzureServiceBusTopic(
            //            Configuration["EventBusConnection"],
            //            topicName: "eshop_event_bus",
            //            name: "basket-servicebus-check",
            //            tags: new string[] { "servicebus" });
            //}
            //else
            //{
            //    hcBuilder
            //        .AddRabbitMQ(
            //            Configuration["EventBusConnection"],
            //            name: "basket-rabbitmqbus-check",
            //            tags: new string[] { "rabbitmqbus" });
            //}

            services.AddHttpContextAccessor();

            services.AddSwashbuckle();

            services.JWTAddAuthentication();

            services.AddHttpClient("GatewayClient", config =>
            {
                config.BaseAddress = new Uri(Configuration.GetValue<string>("GatewayApi"));
            });

            services.AddEventBus(Configuration);

            services.AddHostedService<ConsumeRabbitMQHostedService>();

            services.AddTransient<NotificationIntegrationEventHandler>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<NotificationIntegrationEvent, NotificationIntegrationEventHandler>();
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

            app.UseMvc();

            //ConfigureEventBus(app);
        }
    }
}
