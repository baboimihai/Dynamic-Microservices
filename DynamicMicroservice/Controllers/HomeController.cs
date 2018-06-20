
using GlobalServices;
using MicroCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WriteLogToDb;

namespace DynamicMicroservice.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IExamples examples;
        public HomeController(IExamples examples)
        {
            this.examples = examples;
        }
        public ActionResult TestBD(string text)
        {
            return Json(examples.TestBD(text) ? "ok" : "error", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index()
        {
            ViewBag.Logs = WriteLog.GetLogs();
            ViewBag.TaskClients = MicroContainer.GetTaskClients();
            ViewBag.TaskLogs = MicroContainer.GetTaskLogs();
            return View();
        }
        public ActionResult ComputeResult(string input)
        {
            try
            {
                Regex re = new Regex("(\\d+)\\s*([-+*/])\\s*(\\d+)");
                Match m = re.Match(input);

                if (m.Success)
                {
                    var nr1 = Int32.Parse(m.Groups[1].Value);
                    String sign = m.Groups[2].Value;
                    var nr2 = Int32.Parse(m.Groups[3].Value);
                    var result = examples.ComputeNumber(nr1, nr2, sign);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                }
            }
            catch (Exception e)
            {

            }
            return Json("invalid", JsonRequestBehavior.AllowGet);

        }
        public ActionResult StressTest()
        {
            return Json(examples.StressTestSystem(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult StressTestBD()
        {
            return Json(examples.StressTestSystemBD(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult StressTestPDF()
        {
            return Json(examples.StressTestSystemPDF(), JsonRequestBehavior.AllowGet);
        }
    }
}