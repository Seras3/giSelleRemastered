using giSelleRemastered.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace giSelleRemastered.Controllers
{
    public class RatingController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Rating
        public ActionResult Index()
        {
            if (TempData.ContainsKey("Message"))
            {
                ViewBag.Message = TempData["Message"].ToString();
            }

            var ratings = from rating in db.Ratings
                           select rating;

            ViewBag.Ratings = ratings;
            return View();
        }

        public ActionResult Show(int id)
        {
            Rating rating = db.Ratings.Find(id);
            return View(rating);
        }

        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public ActionResult New(Rating rating)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Ratings.Add(rating);
                    db.SaveChanges();
                    TempData["Message"] = "Rating has been successfully added";
                    return RedirectToAction("Index");
                }
                TempData["Message"] = "Rating adding has been failed";
                return View(rating);
            }
            catch (Exception e)
            {
                TempData["Message"] = "Rating adding has been failed";
                return View(rating);
            }
        }

        public ActionResult Edit(int id)
        {
            Rating rating = db.Ratings.Find(id);
            return View(rating);
        }

        [HttpPut]
        public ActionResult Edit(int id, Rating requestRating)
        {
            try
            {
                Rating rating = db.Ratings.Find(id);
                if (TryUpdateModel(rating))
                {
                    db.SaveChanges();
                    TempData["Message"] = "Rating has been successfully changed";
                    return RedirectToAction("Index");
                }

                TempData["Message"] = "Rating editing has been failed";
                return View(requestRating);

            }
            catch (Exception e)
            {
                TempData["Message"] = "Rating editing has been failed";
                return View(requestRating);
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Rating rating = db.Ratings.Find(id);
            db.Ratings.Remove(rating);
            TempData["Message"] = "Rating has been removed";
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}