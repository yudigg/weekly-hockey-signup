using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplicationCookie.Controllers
{
    public class CookieController : Controller
    {
        //
        // GET: /Cookie/

        public ActionResult Index()
        {
            int value = 0;
        
            if (Request.Cookies["test"] != null)
            {
                value = int.Parse(Request.Cookies["test"].Value);
            }         
                HttpCookie cookie = new HttpCookie("test", (value +1).ToString());           
            Response.Cookies.Add(cookie);
            return View(value);
        }

    }
}
