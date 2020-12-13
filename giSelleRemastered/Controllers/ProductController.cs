using giSelleRemastered.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace giSelleRemastered.Controllers
{
    public class ProductController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            if (TempData.ContainsKey("Message"))
            {
                ViewBag.Message = TempData["Message"].ToString();
            }

            var products = (from product in db.Products.Include("Categories")
                             orderby product.Name
                             select product).ToList();
            ViewBag.ShowButtons = false;
            if (User.IsInRole("Admin") || User.IsInRole("Partner"))
                ViewBag.ShowButtons = true;
            return View(products);
        }

        public ActionResult Show(int id)
        {
            var product = db.Products.Include(i => i.Image).Include(i => i.User).Where(p => p.Id == id).FirstOrDefault();
            StateInitialisation(product);
            ViewBag.ShowButtons = false;
            if (User.IsInRole("Admin") || (User.IsInRole("Partner") && IsOwner(product.UserId)))
                ViewBag.ShowButtons = true;
            System.Diagnostics.Debug.WriteLine(product.Image.Path);
            return View(product);
        }

        [Authorize(Roles = "Admin,Partner")]
        public ActionResult New()
        {
            if (TempData.ContainsKey("Message"))
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            Product product = new Product();
            StateInitialisation();
            return View(product);
        }

        [Authorize(Roles = "Admin,Partner")]
        [HttpPost]
        public ActionResult New([Bind(Exclude = "ImageId,Image")]Product product, HttpPostedFileBase Image)
        {
            StateInitialisation();
            System.Diagnostics.Debug.WriteLine(product.UserId);
            int imageId = ValidateAddImage(Image);
            product.ImageId = imageId;
            product.UserId = User.Identity.GetUserId();
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

        [Authorize(Roles = "Admin,Partner")]
        public ActionResult Edit(int id)
        {
            Product product = db.Products.Find(id);
            StateInitialisation(product);
            return View(product);
        }

        [Authorize(Roles = "Admin,Partner")]
        [HttpPut]
        public ActionResult Edit(int id, Product requestProduct)
        {
           
            Product product = db.Products.Find(id);
            if (IsOwner(product.UserId) || User.IsInRole("Admin"))
            {
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
            else
            {
                TempData["Message"] = "You don't have permission to change this product";
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "Admin,Partner")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Product product = db.Products.Find(id);
            if (IsOwner(product.UserId) || User.IsInRole("Admin"))
            {
                db.Products.Remove(product);
                TempData["Message"] = "Product has been removed";
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Message"] = "You don't have permission to change this product";
                return RedirectToAction("Index");
            }
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


        public int ValidateAddImage(HttpPostedFileBase image)
        {
            if (image != null && image.ContentLength > 0)
            {
                UploadFile dbImage = new UploadFile();
                try
                {
                    string path = Path.Combine(Server.MapPath("~/Content/Images/Products"),
                                               Path.GetFileName(image.FileName));
                    dbImage.Path = path;
                    // Add extension constraints
                    dbImage.Extension = Path.GetExtension(image.FileName);
                    dbImage.Name = Path.GetFileNameWithoutExtension(image.FileName);
                    dbImage.FileId = GetFileIdToAdd();
                    image.SaveAs(path);
                    db.UploadFiles.Add(dbImage);
                    db.SaveChanges();
                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    return 0;
                }
                return dbImage.FileId;
            }
            else
                return 0;
        }

        public int GetFileIdToAdd()
        {
            int maxId = (from elem in db.UploadFiles
                        select elem.FileId).ToArray().Max();
            return maxId + 1;
        }

        public List<string> GetUserRoles()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            List<string> userRoles = new List<string>();
            foreach (var role in roleManager.Roles)
            {
                if(User.IsInRole(role.Name))
                    userRoles.Add(role.Name);
            }
            return userRoles;
        }
        
        public bool IsOwner(string id)
        {
            return id == User.Identity.GetUserId();
        }
    }
}