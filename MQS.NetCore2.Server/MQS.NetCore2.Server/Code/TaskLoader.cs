﻿using System.Runtime;
using System.Threading;
using System.Threading.Tasks;
using TaskContainerLib;

using System.Linq;
using System;
using MQS.NetCore2.Code;
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
