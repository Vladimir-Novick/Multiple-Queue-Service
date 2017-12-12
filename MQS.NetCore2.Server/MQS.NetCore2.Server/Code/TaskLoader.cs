using System.Runtime;
using System.Threading;
using System.Threading.Tasks;
using TaskContainerLib;

using System.Linq;
using System;
using MQS.NetCore2.Code;

namespace MQS.NetCore2.Server.Code
{
    public class TaskLoader
    {
        private const string CACHE_CLEANER = "Clean Cache process";
        private const string GC_COMPACT_PROCESS = "Memory Compact process";

        public static void Start()
        {
            GCConfig();
            LoadCacheCleaner();

        }

        private static void LoadCacheCleaner()
        {

                var task = new Task(() =>
                {
                    try
                    {
                        while (true)
                        {
                            try
                            {
                                QueueMainObject item;
                                QueueMainObject delItem;
                                bool ok;
                                foreach (var key in  DataCache.UserDataCache.Keys.ToList())
                                {
                                    ok = DataCache.UserDataCache.TryGetValue(key, out item);
                                    if (ok)
                                    {
                                        int s = - ClientConfig.GetConfigData.WaitObjectTime;

                                        var t2 = DateTime.Now;
                                        var t = DateTime.Now.AddSeconds(s);

                                        if ( t > item.DateC)
                                        {
                                            DataCache.UserDataCache.TryRemove(key, out delItem);
                                        }
                                       
                                    }

                                }
                            }
                            catch (Exception ex)
                            {
                                AppLogger.LogInformation($"Error Memory Cache Cleane {ex.Message}");
                            }

                            Thread.Sleep((ClientConfig.GetConfigData.WaitObjectTime/30) * 1000);
                        }

                    }
                    catch (Exception) { }
                });
                DataCache.TaskContainer.TryAdd(task, CACHE_CLEANER, $"Memory Cache Cleaner .  {ClientConfig.GetConfigData.WaitObjectTime} sec.  ");
           
        }

        private static void GCConfig()
            {
                DataCache.TaskContainer.Option = TaskContainerManager.Options.GCCollect | TaskContainerManager.Options.LargeObjectHeapCompactionMode;

                if (ClientConfig.GetConfigData.Memory_Compact)
                {
                    var task = new Task(() =>
                    {
                        try
                        {
                            while (true)
                            {
                                try
                                {
                                    GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
                                    GC.Collect();
                                }
                                catch (Exception ex)
                                {
                                    AppLogger.LogInformation($"Error GC Collect {ex.Message}");
                                }

                                Thread.Sleep(ClientConfig.GetConfigData.Memory_Compact_Time * 60000);
                            }

                        }
                        catch (Exception) { }
                    });
                    DataCache.TaskContainer.TryAdd(task, GC_COMPACT_PROCESS, $"Compact memory every  {ClientConfig.GetConfigData.Memory_Compact_Time} min.  ");
                }
            }

        }
    }
