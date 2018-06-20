using ClassLibraryMath;
using GlobalServices;
using PdfDiploma;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using WriteLogToDb;

namespace DynamicMicroservice.Api
{
    public class MathController : ApiController
    {
        [HttpGet]
        public int AddNumbers(int a, int b)
        {
            LibraryMath mathLib = new LibraryMath();
            return mathLib.SimpleMath(a, b, "+");
        }
        [HttpGet]
        public string TestBD(string text)
        {
            return new ClientWriteLog().WriteLog(text) ? "ok" : "error";
        }
        [HttpGet]
        public HttpResponseMessage WriteDiploma(string name)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest);


            //get buffer
            var buffer = new GenerateDiploma().WriteDiploma(name);
            //content length for use in header
            var contentLength = buffer.Length;
            //200
            //successful
            var statuscode = HttpStatusCode.OK;
            response = Request.CreateResponse(statuscode);
            response.Content = new StreamContent(new MemoryStream(buffer));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            response.Content.Headers.ContentLength = contentLength;
            ContentDispositionHeaderValue contentDisposition = null;
            if (ContentDispositionHeaderValue.TryParse("inline; filename=" + name + ".pdf", out contentDisposition))
            {
                response.Content.Headers.ContentDisposition = contentDisposition;
            }
            return response;

            //var contentLength = buffer.Length;
            //var statuscode = HttpStatusCode.OK;
            //response = Request.CreateResponse(statuscode);
            //response.Content = new StreamContent(new MemoryStream(buffer));
            //response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            //response.Content.Headers.ContentLength = contentLength;
            //ContentDispositionHeaderValue contentDisposition = null;
            //if (ContentDispositionHeaderValue.TryParse("inline; filename=Diploma.pdf", out contentDisposition))
            //{
            //    response.Content.Headers.ContentDisposition = contentDisposition;
            //}
        }
    }
}
