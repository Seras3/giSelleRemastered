using giSelleRemastered.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            var products = (from product in db.Products.Include("Categories")
                             orderby product.Name
                             select product).ToList();
            return View(products);
        }

        public ActionResult Show(int id)
        {
            Product product = db.Products.Find(id);
            StateInitialisation(product);
            return View(product);
        }

        public ActionResult New()
        {
            StateInitialisation();
            return View();
        }

        [HttpPost]
        public ActionResult New(Product product)
        {
            StateInitialisation();
            try
            {
                if (ModelState.IsValid)
                {
                    product.Categories = new Collection<Category>();
                    foreach(var selectedCategoryId in product.SelectedCategoryIds)
                    {
                        Category category = db.Categories.Find(selectedCategoryId);
                        product.Categories.Add(category);
                    }

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
            StateInitialisation(product);
            return View(product);
        }

        [HttpPut]
        public ActionResult Edit(int id, Product requestProduct)
        {
            Product product = db.Products.Find(id);
            StateInitialisation(product);
            try
            {
                if (TryUpdateModel(product))
                {
                    product.Name = requestProduct.Name;
                    product.Description = requestProduct.Description;
                    product.Currency = requestProduct.Currency;
                    product.HasQuantity = requestProduct.HasQuantity;
                    product.Quantity = requestProduct.Quantity;
                    product.PriceInMu = requestProduct.PriceInMu;

                    product.Categories = new Collection<Category>();
                    foreach (var selectedCategoryId in requestProduct.SelectedCategoryIds)
                    {
                        Category category = db.Categories.Find(selectedCategoryId);
                        product.Categories.Add(category);
                    }

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

        [NonAction]
        public IEnumerable<SelectListItem> GetAllCurrencies()
        {
            var selectList = new List<SelectListItem>();
            var currencies = new string[] {"RON", "EUR", "USD" };
            foreach(var currency in currencies)
            {
                selectList.Add(new SelectListItem
                {
                    Text = currency,
                    Value = currency
                });
            }
            return selectList;
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories()
        {
            var selectList = new List<SelectListItem>();
            var categories = from category in db.Categories
                             select category;
            foreach(var category in categories)
            {
                selectList.Add(new SelectListItem
                {
                    Text = category.Name,
                    Value = category.Id.ToString()
                });
            }
            return selectList;
        }

        [NonAction]
        public int[] GetSelectedCategories(Product product)
        {
            var categories = new List<int>();
            foreach(var category in product.Categories)
            {
                categories.Add(category.Id);
            }
            return categories.ToArray();
        }

        public void StateInitialisation(Product product)
        {
            StateInitialisation();
            product.SelectedCategoryIds = GetSelectedCategories(product);
        }

        public void StateInitialisation()
        {
            ViewBag.Categories = GetAllCategories();
            ViewBag.Currencies = GetAllCurrencies();
        }
    }
}