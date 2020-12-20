using giSelleRemastered.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace giSelleRemastered.Controllers
{
    public class RatingController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        public ActionResult New(int id, Rating rating)
        {
            int productId = id;
            rating.UserId = User.Identity.GetUserId();
            rating.ProductId = productId;
            // For not passing UserId as hidden input
            ModelState.Clear();
            Rating dbRating = db.Ratings.Where(i => i.UserId == rating.UserId &&
                                                 i.ProductId == rating.ProductId).FirstOrDefault();

            if (Enumerable.Range(1, 5).Contains(rating.Value))
            {
                if (dbRating == null)
                {
                    db.Ratings.Add(rating);
                    db.SaveChanges();   
                }
                else if(dbRating.Value == rating.Value)
                {
                    db.Ratings.Remove(dbRating);
                    db.SaveChanges();
                }
                else if (TryUpdateModel(dbRating, null, new string[] { "Value" }))
                {
                    dbRating.Value = rating.Value;
                    db.SaveChanges();   
                }
            }
            return Redirect("/Product/Show/"+ productId);
        }
    }
}