using giSelleRemastered.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace giSelleRemastered.Controllers
{
    public class CommentController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        [Authorize(Roles = "Admin,Partner,User")]
        public ActionResult New(int id, Comment comment)
        {   
            if (db.Products.Find(comment.ProductId) == null)
            {
                return RedirectToAction("Show", "Product", new { id });
            }

            comment.Date = DateTime.Now;
            comment.UserId = User.Identity.GetUserId();

            if (comment.Content == null )
            {
                Session["Message"] = "Comment should not be empty";
                return RedirectToAction("Show", "Product", new { id });
            }

            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                db.SaveChanges();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Invalid comment");
            }

            return RedirectToAction("Show", "Product", new { id });
        }

        [Authorize(Roles = "Admin,Partner,User")]
        public ActionResult Edit(int id)
        {
            Comment comment = db.Comments.Find(id);
            if(comment != null && comment.UserId == User.Identity.GetUserId())
            {
                return View(comment);
            }
            return Redirect("/Product/Index");
        }

        [HttpPut]
        public ActionResult Edit(int id, Comment requestComment)
        {
            Comment comment = db.Comments.Find(id);
            try
            {
                if (TryUpdateModel(comment) && comment.UserId == User.Identity.GetUserId())
                {
                    comment.Content = requestComment.Content;
                    db.SaveChanges();
                }
                return RedirectToAction("Show", "Product", new { id = comment.ProductId });
            }
            catch(Exception)
            { }
            return RedirectToAction("Index", "Product");
        }
        
        // GET: Comment
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var delComm = db.Comments.Find(id);
            int prodId = delComm.ProductId;
            if (delComm != null && (delComm.UserId == User.Identity.GetUserId() || User.IsInRole("Admin")))
            {                
                db.Comments.Remove(delComm);
                db.SaveChanges();
            }
            return Redirect("/Product/Show/" + prodId);
        }
    }
}