using MicroCore.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MicroCore
{
    public static class MicroClientConnection
    {
        public static string POSTJson(MicroClientInfo client, ClientAction action, string jsonContent)
        {
            string controllerName = "/DMicroservice/";
            if (action == ClientAction.ExtendedService && string.IsNullOrWhiteSpace(client.ClientToken))
            {
                controllerName = "/api/DynamicMicros/";
            }
            var url = "http://" + client.IP + ":" + client.Port+ controllerName + EnumUtil.GetDescription(action);
            //if (client.IP == "localhost")
            //{
            //   //url = /*"http://www.dm.com/";//*/ "http://mihaidm.ddns.net";
            //}
            //url += controllerName + EnumUtil.GetDescription(action);
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";

                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                Byte[] byteArray = encoding.GetBytes(jsonContent);

                request.ContentLength = byteArray.Length;
                request.ContentType = @"application/json";

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }
                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (var reader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.ASCII))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
                catch (WebException ex)
                {
                    return string.Empty;
                }
            }
            catch (WebException ex)
            {
                return string.Empty;
            }

        }
    }
}
