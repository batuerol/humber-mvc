using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Mahc_Final.Models;
using Mahc_Final.DBContext;
using System.Threading.Tasks;
using Stripe;

namespace Mahc_Final.Controllers
{
    public class ViewGiftsController : Controller
    {
        private HospitalContext db = new HospitalContext();

        // GET: ViewGifts
        public ActionResult Index()
        {
            var gifts = db.Gifts.Include(g => g.GiftCat);
            return View(gifts.ToList());
        }

        // GET: ViewGifts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gift gift = db.Gifts.Find(id);
            if (gift == null)
            {
                return HttpNotFound();
            }
            return View(gift);
        }


        /*
        public ActionResult Proceed(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gift gift = db.Gifts.Find(id);
            if (gift == null)
            {
                return HttpNotFound();
            }
            var price = gift.price;
            TempData["price"] = price;
            return RedirectToAction("Pay");



        }*/

        private static async Task<string> GetTokenId(UserInfo ui)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                var myToken = new StripeTokenCreateOptions();
                myToken.Card = new StripeCreditCardOptions()
                {
                    Number = ui.CardNumber,
                    ExpirationYear = ui.ExpYear,
                    ExpirationMonth = ui.ExpMonth,
                    Cvc = ui.Cvc
                };

                var tokenService = new StripeTokenService();
                var stripeToken = tokenService.Create(myToken);

                return stripeToken.Id;
            });
        }


        private static async Task<string> ChargeCustomer(string tokenId, float payAmount)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                var myCharge = new StripeChargeCreateOptions
                {
                    Amount = Convert.ToInt32(payAmount * 100),
                    Currency = "cad",
                    Description = "Gift Payment",
                    SourceTokenOrExistingSourceId = tokenId
                };

                var chargeService = new StripeChargeService();
                var stripeCharge = chargeService.Create(myCharge);

                return stripeCharge.Id;
            });
        }



        public ActionResult Pay(string price)
        {
            //TempData.Keep();

            return View();
        }




        [HttpPost]
        public async Task<ActionResult> Pay(string price, UserInfo ui)
        {
            var errorMessage = String.Empty;
            var chargeId = String.Empty;
            //Gift gft = new Gift();



            try
            {
                var tokenId = await GetTokenId(ui);
                chargeId = await ChargeCustomer(tokenId, (float.Parse(price)+((float.Parse(price)*13)/100)));
            }
            catch (Exception e)
            {
                TempData["msg"] = "Make sure you have entered correct information about your card!";
                return RedirectToAction("Pay");
            }
            TempData["payingstatus"] = "Thank you!Your Payment has been processed successfully";
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return View();
        }













        /* Viewers can not Create or Delete a Gift they can just view the list or details about a particular gift*/

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
