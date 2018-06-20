using Micro;
using MicroCore;
using MicroCore.Dto;
using MicroCore.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;


namespace DynamicMicroservice.api
{
    public class DynamicMicrosController : ApiController
    {
        // GET api/<controller>
        [HttpPost]
        public string StartSession()
        {
            var input = Request.Content.ReadAsStringAsync().Result;
            var ike = new IKEClient();
            var parts = input.Split(' ');
            var respons = ike.GenerateResponse(parts[0], parts[1]);
            MicroContainer.AddNewClient(parts[3], ike.key, parts[4], parts[2]);
            return respons;
        }
        [HttpPost]
        public string KeepAlive()
        {
            var input = Request.Content.ReadAsStringAsync().Result;
            return MicroContainer.ClientKeepAlive(input)?"Ok":"No";
        }
        [HttpPost]
        public bool Ready()
        {
            var input = Request.Content.ReadAsStringAsync().Result;
            var client = MicroContainer.GetClientTyToken(input);
            MicroContainer.InstallService(MicroContainer.GetNextTaskToInstall(), client);
            return true;
        }
        [HttpPost]
        public string ExtendedService()
        {
            var input = Request.Content.ReadAsStringAsync().Result;
            var parts = input.Split(' ');
            var extendedService = JsonConvert.DeserializeObject<ExtendedMicroservice>(MicroCripto.Decrypt(parts[0], MicroContainer.GetClientTyToken(parts[1]).ClientKey));
            try
            {
                var taskToRun = MicroContainer.GetTaskToRun(extendedService.SThis);
                if(taskToRun!= null)
                {
                    var client = MicroContainer.GetNextClient(taskToRun);
                    if (client != null)
                    {
                        var watch = System.Diagnostics.Stopwatch.StartNew();
                        WriteLog.LogClient(client, true, extendedService.SThis + "_extendedService");
                        var message = MicroClientConnection.POSTJson(client, ClientAction.ExtendedService, MicroCripto.Encrypt(JsonConvert.SerializeObject(extendedService), client.ClientKey));
                        watch.Stop();
                        MicroContainer.LogTaskExecution(taskToRun, watch.ElapsedMilliseconds);
                        return message.Substring(1, message.Length - 2);
                    }
                }
            }
            catch (Exception e)
            {

            }
            return string.Empty;
        }

    }
}