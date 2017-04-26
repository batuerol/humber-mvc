using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Mahc_Final.DBContext;
using Mahc_Final.ViewModels;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Mahc_Final.Controllers
{
    public class CareersController : Controller
    {
        static HttpClient client = new HttpClient();
        string developer = "";
        private HospitalContext db = new HospitalContext();
        // GET: Careers
        public  ActionResult Index()
        {
            Careers OppApp = new Careers() ;

            OppApp.opportunities = (from o in db.Jobs
                                    orderby o.Date_last_modified descending
                                    select o).Take(5).ToList();
            OppApp.applications = db.Job_applications.OrderByDescending(a => a.Date).Take(5).ToList();
            //GetDeveloper("Careers").Wait();
            return View("Admin/Index",OppApp);
        }

        public async System.Threading.Tasks.Task GetDeveloper(string feature)
        {
            client.BaseAddress = new Uri("https://mmgnr1keg7.execute-api.us-east-1.amazonaws.com/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                developer = await GetDeveloperAsync(feature);
                Console.WriteLine(developer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        static async Task<string> GetDeveloperAsync(string feature="Careers")
        {
            string developer = null;
            HttpResponseMessage response = await client.GetAsync("Prod/api/feature/"+feature);
            if (response.IsSuccessStatusCode)
            {
                developer = await response.Content.ReadAsAsync<string>();
            }
            return developer;
        }
    }
}