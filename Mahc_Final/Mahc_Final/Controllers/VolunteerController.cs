using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mahc_Final.DBContext;
using Mahc_Final.ViewModels;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Mahc_Final.Helpers;

namespace Mahc_Final.Controllers
{
    public class VolunteerController : Controller
    {

        static HttpClient client = new HttpClient();
        private HospitalContext db = new HospitalContext();
        // GET: Volunteer
        public async Task<ActionResult> Index()
        {
            Volunteers TskVol = new Volunteers();

            TskVol.tasks= (from o in db.Tasks
                                    orderby o.Modified_date descending
                                    select o).Take(5).ToList();
            TskVol.volunteers = db.Volunteers.OrderByDescending(a => a.Date).Take(5).ToList();

            var responseString = await client.GetStringAsync("http://mmgnr1keg7.execute-api.us-east-1.amazonaws.com/prod/api/feature/careers");
            dynamic developer = JObject.Parse(responseString);
            ViewBag.Developer = (string)developer["name"];

            return View("Admin/Index",TskVol);
        }
    }
}