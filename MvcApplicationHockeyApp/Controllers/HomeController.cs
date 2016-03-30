using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HockeyLibrary;
using MvcApplicationHockeyApp.Models;

namespace MvcApplicationHockeyApp.Controllers
{
    public class HomeController : Controller
    {
        private string conn = @"Data Source=.\sqlexpress;Initial Catalog=HockeyDb;Integrated Security=True";
        public ActionResult Index()
        {
            ViewModel vm = new ViewModel();
            if (TempData["Message"] != null)
            {
                vm.Message = (string)TempData["Message"];
            }
            return View(vm);
        }
        public ActionResult AddSignUp()
        {
            HockeyDb db = new HockeyDb(conn);
            Event e = new Event();
            e = db.GetLatestEvent();
            ViewModel vm = new ViewModel();
            vm.Event = e;
            vm.Status = db.GetEventStatus(e);
            return View(vm);
        }
        [HttpPost]
        public ActionResult AddSignUp(string firstName, string lastname, string email, int eventid)
        {
            HockeyDb db = new HockeyDb(conn);
            Event e = new Event();
            e = db.GetLatestEvent();
            ViewModel vm = new ViewModel();
            vm.Event = e;
            vm.Status = db.GetEventStatus(e);
            if (vm.Status == EventStatus.Open)
            // if (db.GetNumberofSignUps(eventid) < db.GetMaxPlayersForEvent(eventid) && e.Dt > DateTime.Now)
            {
                SignUp s = new SignUp();
                s.FirstName = firstName;
                s.LastName = lastname;
                s.Email = email;
                s.EventId = eventid;
                db.CreateSignUps(s);
                TempData["Message"] = "You're successfully signed up!!";
            }
            else
            {
                TempData["Message"] = "YOU are NOT signed up!";
            }
            return RedirectToAction("index");
        }

        public ActionResult Notifications()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddEmail(string email)
        {
            HockeyDb db = new HockeyDb(conn);
            db.AddToEmailList(email);
            TempData["Message"] = "Email-Added!";
            return RedirectToAction("index");
        }

    }
}
