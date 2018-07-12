using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MQS.NetCore2.Server.Code;
/*

Copyright (C) 2016-2018 by Vladimir Novick http://www.linkedin.com/in/vladimirnovick ,

    vlad.novick@gmail.com , http://www.sgcombo.com , https://github.com/Vladimir-Novick
	

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/
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
