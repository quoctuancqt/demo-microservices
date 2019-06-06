﻿using Core.Extensions;
using Core.Middlewares;
using Demo.Infrastructure.Extensions;
using JwtTokenServer.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Demo.ProductService
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
            services.AddDbContext<ProductContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("ProductDB"));
            });

            services.AddServices();

            services.AddHttpContextAccessor();

            services.AddSwashbuckle();

            services.JWTAddAuthentication();

            services.AddHttpClient("GatewayClient", config =>
            {
                config.BaseAddress = new Uri(Configuration.GetValue<string>("GatewayApi"));
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseSwashbuckle();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}