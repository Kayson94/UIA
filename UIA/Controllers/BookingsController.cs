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
    public class BookingsController : Controller
    {
        private uiaEntities1 db = new uiaEntities1();

        // GET: Bookings
        [Authorize]
        public ActionResult Index()
        {
            int sessionUserID = Convert.ToInt32(User.Identity.Name.Split('|')[0].ToString());
            var booking_details = (from f in db.Flights
                                   join b in db.Bookings on f.flight_id equals b.flight_id
                                   join c in db.Customers on b.cust_id equals c.cust_id
                                   where c.cust_id == sessionUserID
                                   select new ShowBookingViewModel
                                   {
                                       booking_id = b.booking_id,
                                       cust_id = c.cust_id,
                                       name = c.name,
                                       flight_id = f.flight_id,
                                       flight_date = f.flight_date,
                                       flight_time = f.flight_time,
                                       duration = f.duration,
                                       destination = f.destination,
                                       plane_name = f.plane_name,
                                       plane_company = f.plane_company
                                   }).ToList();

            return View(booking_details);
        }


        // GET: Bookings/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // GET: Bookings/Create
        [Authorize]
        public ActionResult Create(int id)
        {// want get customer name form customer table
            int sessionUserID = Convert.ToInt32(User.Identity.Name.Split('|')[0].ToString());
            var booking_details = (from f in db.Flights 
                                   where f.flight_id == id
                                   select new ShowBookingViewModel
                                   {
                                       cust_id = sessionUserID,
                                       flight_id = f.flight_id,
                                       flight_date = f.flight_date,
                                       flight_time = f.flight_time,
                                       duration = f.duration,
                                       destination = f.destination,
                                       plane_name = f.plane_name,
                                       plane_company = f.plane_company
                                   }).FirstOrDefault();
            return View(booking_details);
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "booking_id,flight_id,cust_id")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Bookings.Add(booking);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(booking);
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
