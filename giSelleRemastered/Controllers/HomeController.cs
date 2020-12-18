using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace giSelleRemastered.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Let's just talk about tattoos";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Here you can find us";

            return View();
        }

        public ActionResult Links()
        {
            ViewBag.IsAdmin = User.IsInRole("Admin");
            return View();
        }
    }
}