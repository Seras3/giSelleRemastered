using giSelleRemastered.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace giSelleRemastered.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext(); 

        [Authorize(Roles = "User,Partner")]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Include("Cart").Where(i => i.Id == userId).FirstOrDefault();
            Cart cart = db.Carts.Include("Products").Where(i => i.Id == user.CartId).FirstOrDefault();
            ViewBag.Message = TempData["Message"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View(cart);
        }

        [Authorize(Roles = "User,Partner")]
        public ActionResult PlaceOrder(int id)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Include("Cart").Where(i => i.Id == userId).FirstOrDefault();
            Cart cart = db.Carts.Include("Products").Where(i => i.Id == user.CartId).FirstOrDefault();
            if (cart.Id == id)
            {
                if (cart.Products.Count == 0)
                    TempData["ErrorMessage"] = "Your cart is empty, go check some models.";
                else if (TryUpdateModel(cart, null, null, new string[] { "Id" }))
                {
                    cart.Products.Clear();
                    db.SaveChanges();
                    TempData["Message"] = "Stay fresh until you get inked !";
                }
            }
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "User,Partner")]
        public ActionResult Add(int id)
        {
            var userId = User.Identity.GetUserId();
            Product product = db.Products.Find(id);
            if (product != null && userId != product.UserId)
            {
                ApplicationUser user = db.Users.Include("Cart").Where(i => i.Id == userId).FirstOrDefault();
                Cart cart = user.Cart;
                if(TryUpdateModel(cart, null, null, new string[] { "Id" }))
                {
                    cart.Products.Add(product);
                    db.SaveChanges();
                    Session["CartMessage"] = product.Name + " has been added to your cart !";
                    System.Diagnostics.Debug.WriteLine(Session["CartMessage"]);
                }
                else
                {
                    Session["CartErrorMessage"] = "Product failed to add.";
                }
            }
            return Redirect("/Product/Show/"+id);
        }

        [Authorize(Roles = "User,Partner")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var userId = User.Identity.GetUserId();
            Product product = db.Products.Find(id);
            if(product != null)
            {
                ApplicationUser user = db.Users.Include("Cart").Where(i => i.Id == userId).FirstOrDefault();
                Cart cart = user.Cart;
                if(cart.Products.Contains(product) && TryUpdateModel(cart, null, null, new string[] { "Id" }))
                {
                    cart.Products.Remove(product);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
    }
}