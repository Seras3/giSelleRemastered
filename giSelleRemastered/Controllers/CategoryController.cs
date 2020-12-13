using giSelleRemastered.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace giSelleRemastered.Controllers
{
    [Authorize(Roles = "Admin")]
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
            return View();
        }

        public ActionResult Show(int id)
        {
            Category category = db.Categories.Find(id);    
            return View(category);
        }

        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public ActionResult New(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Categories.Add(category);
                    db.SaveChanges();
                    TempData["Message"] = "Category has been successfully added";
                    return RedirectToAction("Index");
                }
                TempData["Message"] = "Category adding has been failed";
                return View(category);
            }
            catch (Exception e)
            {
                TempData["Message"] = "Category adding has been failed";
                return View(category);
            }
        }

        public ActionResult Edit(int id)
        {
            Category category = db.Categories.Find(id);
            return View(category);
        }

        [HttpPut]
        public ActionResult Edit(int id, Category requestCategory)
        {
            try
            {
                Category category = db.Categories.Find(id);
                if(TryUpdateModel(category))
                {
                    category.Name = requestCategory.Name;
                    db.SaveChanges();
                    TempData["Message"] = "Category has been successfully changed";
                    return RedirectToAction("Index");
                }
                
                TempData["Message"] = "Category editing has been failed";
                return View(requestCategory);

            } 
            catch (Exception e)
            {
                TempData["Message"] = "Category editing has been failed";
                return View(requestCategory);
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            TempData["Message"] = "Category has been removed";
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}