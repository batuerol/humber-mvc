using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace Mahc_Final.Controllers
{
    public class CalendarController : Controller
    {

        static HttpClient client = new HttpClient();
        // GET: Calendar
        public async Task<ActionResult> Index()
        {
            var responseString = await client.GetStringAsync("http://mmgnr1keg7.execute-api.us-east-1.amazonaws.com/prod/api/feature/careers");
            dynamic developer = JObject.Parse(responseString);
            ViewBag.Developer = (string)developer["name"];
            RedirectToAction("Index", "Admin");
            return View("Admin/Index");
        }

        public ActionResult PublicIndex()
        {
            //RedirectToAction("PublicIndex","News");
            return View("Public/Index");// why??? 
        }
    }
}