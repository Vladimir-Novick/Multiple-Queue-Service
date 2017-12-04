using Newtonsoft.Json;
using System;
using System.IO;

namespace MQS.NetCore2.Server.Code
{
    public class ClientConfig
    {

        public static ClientConfigData GetConfigData { get; set;  }

        public static void GetConfiguration()
        {
            String configFile = "ClientConfig.json";

            String pathToTheFile = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "config" + Path.DirectorySeparatorChar + configFile;

            GetConfigData = new ClientConfigData();
            using (StreamReader file = File.OpenText(pathToTheFile))
            {
                JsonSerializer serializer = new JsonSerializer();
                GetConfigData = (ClientConfigData)serializer.Deserialize(file, typeof(ClientConfigData));
            }

        }

    }
}
