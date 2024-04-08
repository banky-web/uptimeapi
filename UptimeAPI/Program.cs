using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace UptimeAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("test");
            CreateHostBuilder(args).UseIISIntegration().Build().Run();
        }

        public static IWebHostBuilder CreateHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
               /* .ConfigureWebHostDefaults(args)webBuilder =>
                {
                    webBuilder.UseKestrel()
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseIISIntegration()
                        //.UseUrls("http://0.0.0.0:7012", "https://0.0.0.0:40100")
                        .UseStartup<Startup>();
                });*/
    }
 }
