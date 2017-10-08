using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UIA.Models;

namespace UIA.Controllers
{
    public class HomeController : Controller
    {
        private uiaEntities1 db = new uiaEntities1();

        public ActionResult Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                int sessionUserID = Convert.ToInt32(User.Identity.Name.Split('|')[0].ToString());
                var cust_details = (from c in db.Customers where c.cust_id == sessionUserID select c).ToList();
                return View(cust_details);
            }
            return View();
            
        }

        public ActionResult Login()
        {
            return View();
        }

        [Authorize]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public ActionResult Login(Login model, String returnUrl)
        {
            var dataItem = db.Logins.Where(x => x.username == model.username && x.password == model.password).FirstOrDefault();
            if (dataItem != null)
            {
                FormsAuthentication.SetAuthCookie(dataItem.cust_id + "|" + dataItem.Customer.name, false);
                if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                         && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid user/pass");
                return View();
            }
        }
    }
}