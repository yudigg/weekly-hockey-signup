using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HockeyLibrary;

namespace MvcApplicationHockeyApp.Controllers
{
    public class AdminController : Controller
    {
       private string _conn = @"Data Source=.\sqlexpress;Initial Catalog=HockeyDb;Integrated Security=True";
        public ActionResult Index()
        {
            HockeyDb db = new HockeyDb(_conn);
          
            return View();
        }
        [HttpPost]
        public ActionResult CreateGame(int players, DateTime dateTime, bool email)
        {
            HockeyDb db = new HockeyDb(_conn);
            Event e = new Event(); 
            e.Players = players;
            e.Dt = dateTime;
          //TempData["Updated"] = true;
            e.EventId = db.CreateEvent(e);
            if (email)
                 {
                   db.SendEmail("The next game will be on" + e.Dt.ToLongDateString() + " at " + e.Dt.ToShortTimeString());
                }
            //{
            //    IEnumerable<Notifications> elist = db.GetEmailList();
            //    foreach (Notifications n in elist)
            //    {
            //        db.SendEmail("ygoldgrab@gmail.com", "Hockey", "The next game will be on" + e.Dt.ToLongDateString() + " at " + e.Dt.ToShortTimeString());
            //    }
            //}
            return RedirectToAction("index","home");
        }
        public ActionResult Edit()
        {
            HockeyDb db = new HockeyDb(_conn);
            var allCurrentSignUps = db.GetCurrentSignUps(db.GetLatestEvent().EventId);
              
            return View(allCurrentSignUps);
        }
        //[HttpPost]
        //public ActionResult Edit()
        //{
        //    HockeyDb db = new HockeyDb(_conn);
          

        //    return View();
        //}
    }
}
