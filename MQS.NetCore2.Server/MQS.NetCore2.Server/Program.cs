using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MQS.NetCore2.Server.Code;

namespace MQS.NetCore2.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {

            ClientConfig.GetConfiguration();
            AppConfig();

            IConfigurationRoot config;

            string baseDir = Directory.GetCurrentDirectory();

                config = new ConfigurationBuilder()
                .SetBasePath(baseDir)
                .AddJsonFile("config" + Path.DirectorySeparatorChar + "hosting.json", true)
                .Build();

            string url = config.GetValue<string>("server.urls");

            var host = new WebHostBuilder()
               .UseKestrel()
               .UseLibuv(options =>
               {
                   options.ThreadCount = 10;
               })
                 .UseUrls(url)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();

            host.Run();

        }

        private static void AppConfig()
        {
        
            TaskLoader.Start();

        }

    }
}
