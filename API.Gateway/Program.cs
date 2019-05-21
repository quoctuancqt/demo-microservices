﻿using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace API.Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {

            IWebHostBuilder builder = new WebHostBuilder();

            builder.ConfigureServices(s =>
            {
                s.AddSingleton(builder);
            });

            builder.UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseUrls("http://*:9000")
                .UseStartup<Startup>();

            var host = builder.Build();

            host.Run();
        }
    }
}
