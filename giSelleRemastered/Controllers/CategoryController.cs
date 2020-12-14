using giSelleRemastered.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace giSelleRemastered.Controllers
{
    public class CategoryController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Category
        public ActionResult Index()
        {
            if(TempData.ContainsKey("Message"))
            {
                ViewBag.Message = TempData["Message"].ToString();
            }

            var categories = from category in db.Categories
                             orderby category.Name
                             select category;

            ViewBag.Categories = categories;
            ViewBag.IsAdmin = User.IsInRole("Admin");
            return View();
        }

        public ActionResult Show(int id)
        {
            Category category = db.Categories.Find(id);
            ViewBag.IsAdmin = User.IsInRole("Admin");
            return View(category);
        }
    }
}