using Ionic.Zip;
using Micro.Dto;
using MicroCore.Security;
using MicroCore.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MicroCore
{
    public static class MicroContainer
    {
        private static TaskDistributionStrategy strategy = TaskDistributionStrategy.ExpensiveFirsts;
        private static int LastTaskInstalled = 0;
        private static List<MicroTaskInfo> listOfTasks = new List<MicroTaskInfo>();
        private static List<MicroClientInfo> listOfClients = new List<MicroClientInfo>();
        private static IDictionary<string, List<string>> taskClients = new Dictionary<string, List<string>>();

        public static IDictionary<string, string> GetTaskClients()
        {
            return taskClients.ToDictionary(x => x.Key, x => string.Join(" ,", x.Value));
        }
        public static void RegisterMicro<T>()
        {
            var type = typeof(T);
            string nameSpace = string.Concat(type.Namespace, ".", type.Name);
            listOfTasks.Add(new MicroTaskInfo
            {
                Type = type,
                SThis = nameSpace,
                Logs = new List<TaskLogRecord>()
            });
            taskClients.Add(nameSpace, new List<string>());
        }
        public static bool InstallService(MicroTaskInfo task, MicroClientInfo client)
        {
            try
            {
                var path = System.IO.Directory.GetParent(System.AppDomain.CurrentDomain.BaseDirectory).Parent.FullName;
                var tenporaryZipFile = Guid.NewGuid().ToString().Substring(0, 10) + ".zip";
                using (ZipFile zip = new ZipFile())
                {
                    zip.AddDirectory(@path + "/" + task.Type.Namespace + "/bin/Debug");
                    zip.Save(path + "/" + tenporaryZipFile);
                }
                var byteArray = File.ReadAllBytes(path + "/" + tenporaryZipFile);
                var str = System.Text.Encoding.ASCII.GetString(byteArray);
                var ceva = (from x in byteArray select x).ToList();
                var Json = JsonConvert.SerializeObject(ceva);
                var objToSend = new InstallServiceDto
                {
                    Namespace = task.Type.Namespace,
                    ClassName = task.Type.Name,
                    ByteArray = Json
                };
                var dataToBeSend = MicroCripto.Encrypt(JsonConvert.SerializeObject(objToSend), client.ClientKey);
                var result = MicroClientConnection.POSTJson(client, ClientAction.InstallService, dataToBeSend);
                var resultJson = result.Replace("\\", "").Replace("\"", "") == "Ok";
                if (resultJson)
                {
                    taskClients[task.SThis].Add(client.ClientToken);
                }
                return resultJson;
            }
            catch (Exception e)
            {

            }
            return false;

        }
        public static MicroClientInfo GetNextClient(MicroTaskInfo task)
        {
            try
            {
                if (taskClients.ContainsKey(task.SThis))
                {
                    var nextClientToken = taskClients[task.SThis].OrderBy(x => Guid.NewGuid()).FirstOrDefault();
                    if (nextClientToken != null)
                    {
                        return listOfClients.SingleOrDefault(x => x.ClientToken == nextClientToken);
                    }
                    //var client = listOfClients.SingleOrDefault(x => x.ClientToken == nextClientToken);
                    //if (client.LastKeepAlive > DateTime.Now.AddMinutes(-2))
                    //{
                    //    nextClientToken = taskClients[task.SThis].Where(x => !x.Contains(nextClientToken)).OrderBy(x => Guid.NewGuid()).FirstOrDefault();
                    //    if (nextClientToken != null)
                    //    {
                    //        client = listOfClients.SingleOrDefault(x => x.ClientToken == nextClientToken);
                    //        if (client.LastKeepAlive > DateTime.Now.AddMinutes(-2))
                    //        {
                    //        }
                    //        else
                    //        {
                    //            return client;
                    //        }

                    //    }
                    //}
                    //else
                    //{
                    //    return client;
                    // }
                    //}
                }

            }
            catch (Exception e)
            {

            }
            return null;
        }
        async public static Task RecordClientError(MicroClientInfo client)
        {
            var removeClient = false;
            var result = MicroClientConnection.POSTJson(client, ClientAction.CheckClient, "");
            if (!string.IsNullOrWhiteSpace(result))
            {
                result = result.Substring(1, result.Length - 2);
                if (result != client.ClientToken)
                {
                    removeClient = true;
                    MicroClientConnection.POSTJson(client, ClientAction.RestartServer, "");
                }
            }
            else
            {
                removeClient = true;

            }
            if (removeClient)
            {
                var clientTasks = taskClients.Where(x => x.Value.Contains(client.ClientToken)).ToList();
                clientTasks.ForEach(x =>
                {
                    x.Value.Remove(client.ClientToken);
                });

                listOfClients.Remove(client);
            }

        }
        public static MicroTaskInfo GetTaskToRun(string sthis)
        {
            return listOfTasks.SingleOrDefault(x => x.SThis == sthis);
        }
        public static MicroClientInfo GetClientTyToken(string token)
        {
            return listOfClients.FirstOrDefault(x => x.ClientToken == token);
        }
        public static bool ClientKeepAlive(string token)
        {
            var client = listOfClients.FirstOrDefault(x => x.ClientToken == token);
            if (client != null)
                client.LastKeepAlive = DateTime.Now.Date;
            return client != null;
        }
        public static bool AddNewClient(string clientToken, byte[] key, string ip, string port)
        {
            if (listOfClients.Any(x => x.ClientToken == clientToken || (x.IP == ip && x.Port == port)))
            {
                var updateElement = listOfClients.FirstOrDefault(x => x.ClientToken == clientToken || (x.IP == ip && x.Port == port));
                if (updateElement != null)
                {
                    updateElement.ClientToken = clientToken;
                    updateElement.ClientKey = key;
                    updateElement.IP = ip;
                    updateElement.Port = port;
                    updateElement.LastKeepAlive = DateTime.Now;
                    return true;
                }
                return false;
            }
            else
            {
                listOfClients.Add(new MicroClientInfo
                {
                    ClientToken = clientToken,
                    ClientKey = key,
                    IP = ip,
                    Port = port,
                    LastKeepAlive = DateTime.Now
                });
                return true;
            }
        }
        public static void LogTaskExecution(MicroTaskInfo task, long miliseconds)
        {
            var currentTask = listOfTasks.SingleOrDefault(x => task != null && x.SThis == task.SThis);
            if (currentTask != null)
            {
                currentTask.Logs.Add(new TaskLogRecord
                {
                    Date = DateTime.Now,
                    TimeExecution = miliseconds
                });
            }

        }
        public static IDictionary<string, string> GetTaskLogs()
        {
            return listOfTasks.ToDictionary(x => x.SThis, x => string.Join(" <br>", x.Logs.Select(t => string.Concat("(", t.Date.ToLongTimeString(), " ,", t.TimeExecution, ")"))));
        }
        public static MicroTaskInfo GetNextTaskToInstall()
        {
            try
            {
                if (listOfTasks.Any() && LastTaskInstalled >= 0)
                {
                    if (LastTaskInstalled + 1 <= listOfTasks.Count)
                    {
                        var selectedElement = listOfTasks[LastTaskInstalled];
                        LastTaskInstalled += 1;
                        return selectedElement;
                    }
                    else
                    {
                        LastTaskInstalled = -1;//on auto mode
                    }
                }
                // all tasks has clients
                var taskWithNoClient = taskClients.FirstOrDefault(x => !x.Value.Any());
                if (taskWithNoClient.Key != null)
                {
                    return listOfTasks.First(x => x.SThis == taskWithNoClient.Key);
                }
                var currentItemStrategy = TaskDistributionStrategy.Random;
                if (strategy == TaskDistributionStrategy.MixedFrequentlyExpensive)
                {
                    var random = new Random();
                    var s = random.Next(0, 2);
                    switch (s)
                    {
                        case 1:
                            currentItemStrategy = TaskDistributionStrategy.FrequentlyFirsts;
                            break;
                        case 2:
                            currentItemStrategy = TaskDistributionStrategy.ExpensiveFirsts;
                            break;
                    }
                }
                else
                {
                    currentItemStrategy = strategy;
                }
                var now = DateTime.Now;
                switch (currentItemStrategy)
                {
                    case TaskDistributionStrategy.FrequentlyFirsts:
                        var frequentCalledServices = listOfTasks.Where(x => x.Logs.Any()).ToDictionary(x => x.SThis, x => x.Logs.Count(s => s.Date <= now)).OrderByDescending(x => x.Value)
    .Take(listOfTasks.Count / 2).OrderBy(x => Guid.NewGuid());
                        if (frequentCalledServices != null && frequentCalledServices.Any())
                        {
                            return listOfTasks.First(x => x.SThis == frequentCalledServices.First().Key);
                        }
                        break;
                    case TaskDistributionStrategy.ExpensiveFirsts:
                        var expensiveCalledServices = listOfTasks.Where(x => x.Logs.Any()).ToDictionary(x => x.SThis, x => x.Logs.Where(s => s.Date <= now).Average(t => t.TimeExecution))
    .OrderByDescending(x => x.Value).Take(listOfTasks.Count / 2).OrderBy(x => Guid.NewGuid());
                        if (expensiveCalledServices != null && expensiveCalledServices.Any())
                        {
                            return listOfTasks.First(x => x.SThis == expensiveCalledServices.First().Key);
                        }
                        break;
                    case TaskDistributionStrategy.Random:
                        listOfTasks.OrderBy(x => Guid.NewGuid()).FirstOrDefault(); //random
                        break;
                }
                return null;
            }
            catch (Exception e)
            {

            }
            return null;
        }
    }
}
