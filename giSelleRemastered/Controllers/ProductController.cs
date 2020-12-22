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
using AutoMapper;

namespace giSelleRemastered.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private IMapper mapper = MapperContext.mapper;

        public ActionResult Index()
        {
            if (TempData.ContainsKey("Message"))
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            var products = (from product in db.Products.Include("Categories")
                             orderby product.Name
                             select product).ToList();
            ViewBag.ShowButtons = User.IsInRole("Admin") || User.IsInRole("Partner");
            return View(products);
        }

        public ActionResult Show(int id)
        {
            string userId = User.Identity.GetUserId();
            var product = db.Products.Include(i => i.Image)
                                    .Include(i => i.User)
                                    .Include(i => i.Comments)
                                    .Where(p => p.Id == id).FirstOrDefault();
            if (!product.Accepted)
                return RedirectToAction("Index");

            ViewBag.Message = Session["Message"];
            Session["Message"] = null;

            ViewBag.CartMessage = Session["CartMessage"];
            Session["CartMessage"] = null;

            ViewBag.CartErrorMessage = Session["CartErrorMessage"];
            Session["CartErrorMessage"] = null;

            StateInitialisation();
            
            ViewBag.Comments = GetCommentsForProduct(product);
            ViewBag.CurrentUser = userId;

            Rating userRating = db.Ratings.Where(i => i.UserId == userId)
                                          .Where(i => i.ProductId == product.Id).FirstOrDefault();
            if (userRating != null)
            {
                ViewBag.RateValue = userRating.Value;
            }

            return View(product);
        }


        [Authorize(Roles = "Admin,Partner")]
        public ActionResult New()
        {
            if (TempData.ContainsKey("Message"))
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            ProductWithCategories product = mapper.Map<Product, ProductWithCategories>(new Product());
            StateInitialisation();
            return View(product);
        }

        [Authorize(Roles = "Admin,Partner")]
        [HttpPost]
        public ActionResult New([Bind(Exclude = "ImageId,Image,Accepted")]ProductWithCategories newProduct, HttpPostedFileBase Image)
        {
            StateInitialisation();
            int imageId = UploadAndGetImageId(Image);
            newProduct.ImageId = imageId;
            newProduct.UserId = User.Identity.GetUserId();
            try
            {
                if (ModelState.IsValid)
                {
                    newProduct.Categories = new List<Category>();
                    foreach(var selectedCategoryId in newProduct.SelectedCategoryIds)
                    {
                        Category category = db.Categories.Find(selectedCategoryId);
                        newProduct.Categories.Add(category);
                    }
                    Product dbProduct = mapper.Map<ProductWithCategories, Product>(newProduct);
                    db.Products.Add(dbProduct);
                    db.SaveChanges();
                    TempData["Message"] = "Your request will be submited by admin";
                    return RedirectToAction("Index");
                }
                TempData["Message"] = "Product adding has been failed";
                return View(newProduct);
            }
            catch (Exception)
            {
               TempData["Message"] = "Product adding has been failed";
               return View(newProduct);
            }
        }

        [Authorize(Roles = "Admin,Partner")]
        public ActionResult Edit(int id)
        {
            ProductWithCategories product = mapper.Map<Product, ProductWithCategories>(db.Products.Find(id));
            StateInitialisation(product);
            return View(product);
        }

        [Authorize(Roles = "Admin,Partner")]
        [HttpPut]
        public ActionResult Edit(int id, ProductWithCategories requestProduct)
        {
           
            Product product = db.Products.Find(id);
            if (IsOwner(product.UserId) || User.IsInRole("Admin"))
            {
                StateInitialisation();
                try
                {
                    if (TryUpdateModel(product))
                    {
                        product = mapper.Map<ProductWithCategories, Product>(requestProduct);

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
                catch (Exception)
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
        public int[] GetSelectedCategories(ProductWithCategories product)
        {
            var categories = new List<int>();
            foreach(var category in product.Categories)
            {
                categories.Add(category.Id);
            }
            return categories.ToArray();
        }

        [NonAction]
        public List<Comment> GetCommentsForProduct(Product product)
        {
            List<Comment> comments = (from comm in db.Comments.Include("User")
                                      where comm.ProductId == product.Id
                                     orderby comm.Date descending
                                     select comm).ToList();
            return comments;
        }

        public void StateInitialisation(ProductWithCategories product)
        {
            StateInitialisation();
            product.SelectedCategoryIds = GetSelectedCategories(product);
        }

        public void StateInitialisation()
        {
            ViewBag.Categories = GetAllCategories();
            ViewBag.Currencies = GetAllCurrencies();
        }
        

        public int UploadAndGetImageId(HttpPostedFileBase image)
        {
            if (image != null && image.ContentLength > 0)
            {
                UploadFile dbImage = new UploadFile();
                try
                {
                    string path = Path.Combine(Server.MapPath(Configuration.FILE_UPLOAD_PATH),
                                        Path.GetFileName(image.FileName)); // physical path
                    image.SaveAs(path);

                    // TODO: Add extension constraints
                    dbImage.Extension = Path.GetExtension(image.FileName);
                    dbImage.Name = Path.GetFileNameWithoutExtension(image.FileName);
                    dbImage.FileId = GetFileIdToAdd();
                    db.UploadFiles.Add(dbImage);
                    db.SaveChanges();
                    
                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error while uploading image:" + ex.Message.ToString();
                    return Configuration.DEFAULT_IMAGE_ID;
                }
                return dbImage.FileId;
            }
            else
                return Configuration.DEFAULT_IMAGE_ID;
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