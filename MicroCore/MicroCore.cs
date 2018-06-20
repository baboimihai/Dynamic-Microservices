using Ionic.Zip;
using Micro.Dto;
using MicroCore;
using MicroCore.Dto;
using MicroCore.Security;
using MicroCore.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Micro
{
    public abstract class MicroCore<I, O>
    {
        public string RunFromMicroservice(string json, byte[] key, string clientToken)
        {
            ClientMicro.Key = key;
            ClientMicro.ClientToken = clientToken;
            var objReceived = JsonConvert.DeserializeObject<I>(json);
            return JsonConvert.SerializeObject(ProcessTask(objReceived));
        }
        protected abstract O ProcessTask(I objReceived);
        public O Run(I obj)
        {
            return TaskManager(obj, ClientMicro.Key, ClientMicro.ClientToken);
        }
        private O TaskManager(I obj, byte[] encriptionKeyForExtendedMicroservices, string clientToken)
        {
            if (encriptionKeyForExtendedMicroservices != null)
            {
                string result = string.Empty;
                try
                {
                    AesManaged aes = new AesManaged();
                    aes.GenerateKey();
                    var temporaryPasswod = aes.Key;
                    var jsonToSend = MicroCripto.Encrypt(JsonConvert.SerializeObject(new ExtendedMicroservice { ContentJson = JsonConvert.SerializeObject(obj), TemporaryPassword = temporaryPasswod, SThis = this.ToString() }), encriptionKeyForExtendedMicroservices) + " " + clientToken;
                    result = MicroClientConnection.POSTJson(new MicroClientInfo
                    {
                        IP = "www.dm.com",
                        Port = "80"
                    }, ClientAction.ExtendedService, jsonToSend);
                    return JsonConvert.DeserializeObject<O>(MicroCripto.Decrypt(result.Substring(1, result.Length - 2), temporaryPasswod));
                }
                catch (Exception e)
                {
                    throw new Exception(string.Concat("AICI:", e.ToString(), e.InnerException, "-----------"));
                }

            }
            else
            {

                var taskToRun = MicroContainer.GetTaskToRun(this.ToString());
                if (taskToRun != null)
                {
                    try
                    {
                        var client = MicroContainer.GetNextClient(taskToRun);
                        if (client != null)
                        {
                            var watch = System.Diagnostics.Stopwatch.StartNew();
                            var postResult = POST(client, ClientAction.Get, JsonConvert.SerializeObject(obj));
                            watch.Stop();
                            MicroContainer.LogTaskExecution(taskToRun, watch.ElapsedMilliseconds);
                            if (postResult == null || postResult.Equals(default(O)))
                            {
                                WriteLog.LogClient(client, false, taskToRun.SThis);
                                WriteLog.LogClient(new MicroClientInfo { ClientToken = "localhost" }, true, taskToRun.SThis);
                                MicroContainer.RecordClientError(client);
                                return ProcessTask(obj);
                            }
                            else
                            {
                                WriteLog.LogClient(client, true, taskToRun.SThis);
                                return postResult;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        WriteLog.LogClient(new MicroClientInfo { ClientToken = "localhost" }, false, taskToRun.SThis);
                    }
                }
                var watch2 = System.Diagnostics.Stopwatch.StartNew();
                var result = ProcessTask(obj);
                watch2.Stop();
                MicroContainer.LogTaskExecution(taskToRun, watch2.ElapsedMilliseconds);
                WriteLog.LogClient(new MicroClientInfo { ClientToken = "localhost" }, true, taskToRun.SThis);
                return result;
            }


        }
        private O POST(MicroClientInfo client, ClientAction action, string jsonContent)
        {

            var message = MicroClientConnection.POSTJson(client, action, MicroCripto.Encrypt(jsonContent, client.ClientKey));
            if (message == "")
            {
                return default(O);
            }
            var jsonResult = MicroCripto.Decrypt(message.Substring(1, message.Length - 2), client.ClientKey);
            try
            {
                var reslt = JsonConvert.DeserializeObject<O>(jsonResult);
                return reslt;
            }
            catch (Exception e)
            {

            }
            return default(O);

        }

    }
}
