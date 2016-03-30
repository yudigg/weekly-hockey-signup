using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HockeyLibrary;

namespace MvcApplicationHockeyApp.Models
{
    public class ViewModel
    {
        public EventStatus Status { get; set; }
        public Event Event { get; set; }
        public string Message { get; set; }
    }
}