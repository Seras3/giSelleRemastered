using giSelleRemastered.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace giSelleRemastered.Controllers
{
    public class ProductController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Product
        public ActionResult Index()
        {
            if (TempData.ContainsKey("Message"))
            {
                ViewBag.Message = TempData["Message"].ToString();
            }

            var products = from product in db.Products
                             orderby product.Name
                             select product;

            ViewBag.Products = products;
            return View();
        }

        public ActionResult Show(int id)
        {
            Product product = db.Products.Find(id);
            return View(product);
        }

        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public ActionResult New(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Products.Add(product);
                    db.SaveChanges();
                    TempData["Message"] = "Product has been successfully added";
                    return RedirectToAction("Index");
                }
                TempData["Message"] = "Product adding has been failed";
                return View(product);
            }
            catch (Exception e)
            {
                TempData["Message"] = "Product adding has been failed";
                return View(product);
            }
        }

        public ActionResult Edit(int id)
        {
            Product product = db.Products.Find(id);
            return View(product);
        }

        [HttpPut]
        public ActionResult Edit(int id, Product requestProduct)
        {
            try
            {
                Product product = db.Products.Find(id);
                if (TryUpdateModel(product))
                {
                    product.Name = requestProduct.Name;
                    product.Description = requestProduct.Description;
                    product.Currency = requestProduct.Currency;
                    product.HasQuantity = requestProduct.HasQuantity;
                    product.Quantity = requestProduct.Quantity;
                    product.PriceInMu = requestProduct.PriceInMu;

                    db.SaveChanges();
                    TempData["Message"] = "Product has been successfully changed";
                    return RedirectToAction("Index");
                }

                TempData["Message"] = "Product editing has been failed";
                return View(requestProduct);

            }
            catch (Exception e)
            {
                TempData["Message"] = "Product editing has been failed";
                return View(requestProduct);
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            TempData["Message"] = "Product has been removed";
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}