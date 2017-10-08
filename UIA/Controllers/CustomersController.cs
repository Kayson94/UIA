using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UIA.Models;
using UIA.ViewModels;

namespace UIA.Controllers
{
    public class CustomersController : Controller
    {
        private uiaEntities1 db = new uiaEntities1();

        // GET: Customers
        [Authorize]
        public ActionResult Index()
        {
            int sessionUserID = Convert.ToInt32(User.Identity.Name.Split('|')[0].ToString());
            var cust_details = (from c in db.Customers where c.cust_id == sessionUserID select c).ToList();
            return View(cust_details);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "cust_id,name,age,email,addresss,phone,username,password")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();

                Login a = new Login()
                {
                    username = customer.username,
                    password = customer.password
                };

                db.Logins.Add(a);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(customer);
        }

        public ActionResult EditCustomer(int id) 
        {
            var customerInfo = (from c in db.Customers
                                join l in db.Logins on c.cust_id equals l.cust_id
                                where c.cust_id == id//joining customer table and login table on same customerID, get info of specific customer
                                select new EditCustomerViewModel //put the info retrieved into EditCustomerViewModel
                                {
                                    name = c.name,
                                    age = c.age,
                                    cust_id = c.cust_id,
                                    username = l.username,
                                    password = l.password,
                                    email = c.email,
                                    addresss = c.addresss,
                                    phone = c.phone

                                }).FirstOrDefault();
            return View(customerInfo); //we are returning EditCustomerViewModel
        }


        [HttpPost]
        public ActionResult EditCustomer(EditCustomerViewModel model) // we are getting input from the view
        {
            var oldCustomerInfo = (from c in db.Customers where c.cust_id == model.cust_id select c).FirstOrDefault(); // get from db where customer id equals to the customer's ID
            var oldLoginInfo = (from l in db.Logins where l.cust_id == model.cust_id select l).FirstOrDefault();

            oldCustomerInfo.name = model.name; //update old info to new, from user's input from view
            oldLoginInfo.username = model.username; // "
            oldLoginInfo.password = model.password;// "

            db.SaveChanges(); //commit changes

            return RedirectToAction("Index");
        }

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
