using Microsoft.Extensions.Logging;
using System;

namespace MQS.NetCore2.Server.Code
{
    public class AppLogger
    {
        public static ILogger Logger { get; set; }

        public static void LogInformation(String message)
        {
            if (Logger == null)
            {
                Console.WriteLine($"{DateTime.Now.ToString("dd/MM/yyyy HH:mm")} - {message}");
            }
            else
            {
                Logger?.LogInformation(message);
            }
        }

        public static void LogDebug(string v)
        {
            Logger?.LogInformation(v);
        }
    }
}
