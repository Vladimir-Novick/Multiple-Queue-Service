
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using System.Runtime;
using Newtonsoft.Json;
using MQS.NetCore2.Server.Code;

using Microsoft.AspNetCore.Authentication.Cookies;
using PaulMiami.AspNetCore.Mvc.Recaptcha;
using Newtonsoft.Json.Converters;

namespace MQS.NetCore2.Server
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            })
          .AddCookie(options =>
          {
              options.LogoutPath = "/Account/Logout";
              options.LogoutPath = "/Account/Login";
          });

            services.AddMvcCore().
                     AddDataAnnotations().
                     AddJsonFormatters();

            services.Configure<GzipCompressionProviderOptions>
                  (options => options.Level = CompressionLevel.Fastest);
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
            });

            services.AddMvc((options) =>
            {
                options.CacheProfiles.Add("default", new CacheProfile()
                {
                    Duration = 0,
                    Location = ResponseCacheLocation.None
                });
                options.CacheProfiles.Add("MyCache", new CacheProfile()
                {
                    Duration = 0,
                    Location = ResponseCacheLocation.None
                });
            }).AddJsonOptions(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
            });

            //services.AddRecaptcha(new RecaptchaOptions
            //{
            //    SiteKey = "6Lc73WuxoG-y0ACG",//Configuration["Recaptcha:SiteKey"],
            //    SecretKey = "6Ldi3yYUAlIk94pb0sXoUq",//Configuration["Recaptcha:SecretKey"],
            //    ValidationMessage = "Are you a robot?"
            //});

        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {

            app.UseAuthentication();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

           var logger = loggerFactory.CreateLogger("MQS.NetCore2.Server");
            AppLogger.Logger = logger;

            AppLogger.LogInformation($"- Application started  ");

            string strGName = "";
            if (GCSettings.IsServerGC == true)
                strGName = "server";
            else
                strGName = "workstation";
            AppLogger.LogInformation($"The {strGName} garbage collector is running.");

            app.UseResponseCompression();
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
