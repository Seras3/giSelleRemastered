using AutoMapper;
using giSelleRemastered.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace giSelleRemastered.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : AccountController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private IMapper mapper = MapperContext.mapper;
        public ActionResult UserIndex()
        {
            var usersWithRoles = from userRole in db.UsersRoles
                                 select userRole;
            List<UserRole> usersRolesList = new List<UserRole>();

            foreach (var usrl in usersWithRoles)
            {
                usersRolesList.Add(new UserRole
                {
                    UserId = usrl.UserId,
                    RoleId = usrl.RoleId,
                    Roles = GetAllRoles()
                });
            }

            foreach(var usrl in usersRolesList)
            {
                usrl.User = db.Users.Find(usrl.UserId);
                usrl.Role = db.Roles.Find(usrl.RoleId);
            }
            return View(usersRolesList);
        }

        [HttpPut]
        public ActionResult UserEdit(string id, string RoleName)
        {
            db.UsersRoles.Remove(db.UsersRoles.Where(i => i.UserId == id).FirstOrDefault());
            UserManager.AddToRole(id, RoleName);
            db.SaveChanges();
            return RedirectToAction("UserIndex");
        }

        public ActionResult CategoryNew()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CategoryNew(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Categories.Add(category);
                    db.SaveChanges();
                    TempData["Message"] = "Category has been successfully added";
                    return Redirect("/Category/Index");
                }
                TempData["Message"] = "Category adding has been failed";
                return View(category);
            }
            catch (Exception)
            {
                TempData["Message"] = "Category adding has been failed";
                return View(category);
            }
        }

        public ActionResult CategoryEdit(int id)
        {
            Category category = db.Categories.Find(id);
            return View(category);
        }

        [HttpPut]
        public ActionResult CategoryEdit(int id, Category requestCategory)
        {
            try
            {
                Category category = db.Categories.Find(id);
                if (TryUpdateModel(category))
                {
                    category.Name = requestCategory.Name;
                    db.SaveChanges();
                    TempData["Message"] = "Category has been successfully changed";
                    return Redirect("/Category/Index");
                }

                TempData["Message"] = "Category editing has been failed";
                return View(requestCategory);

            }
            catch (Exception)
            {
                TempData["Message"] = "Category editing has been failed";
                return View(requestCategory);
            }
        }

        [HttpDelete]
        public ActionResult CategoryDelete(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            TempData["Message"] = "Category has been removed";
            db.SaveChanges();
            return RedirectToAction("Index","Category");
        }

        public ActionResult RequestIndex()
        {
            if (TempData.ContainsKey("Message"))
            {
                ViewBag.Message = TempData["Message"];
            }
            var requests = db.Products.Include("User")
                                      .Include("Image")
                                      .Include("Categories").ToList();
            return View(requests);
        }

        public ActionResult RequestShow(int id)
        {
            var request = db.Products.Include("User")
                                     .Include("Image")
                                     .Include("Categories")
                                     .Where(i => i.Id == id).FirstOrDefault();
            if (request.Accepted)
                return RedirectToAction("RequestIndex");
            ProductView product = mapper.Map<Product,ProductView>(request);
            return View(product);
        }

        [HttpPost]
        public ActionResult RequestResponse(int id, ProductView requestedProduct)
        {
            Product product = db.Products.Find(id);
            if (requestedProduct.Accepted)
            {
                if (TryUpdateModel(product))
                {
                    product.Accepted = true;
                    db.SaveChanges();
                    TempData["Message"] = "Product has been accepted";
                    return Redirect("/Admin/RequestIndex");
                }
                else
                {
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
                        }
                    }
                }
                TempData["Message"] = "Something went wrong";
                return RedirectToAction("RequestIndex");
            }
            else
            {
                db.Products.Remove(product);
                db.SaveChanges();
                TempData["Message"] = "Product has been rejected";
                // Stergem si poza adaugata ? (daca nu e default)
                return Redirect("/Admin/RequestIndex");
            }
            
        }

        public IEnumerable<SelectListItem> GetAllRoles()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var selectList = new List<SelectListItem>();
            var roles = new List<string>();
            foreach (var role in roleManager.Roles)
            {
                roles.Add(role.Name);
            }
            foreach (var role in roles)
            {
                selectList.Add(new SelectListItem
                {
                    Text = role,
                    Value = role,
                });
            }
            return selectList;
        }

        [NonAction]
        public int[] GetSelectedCategories(Product product)
        {
            var categories = new List<int>();
            foreach (var category in product.Categories)
            {
                categories.Add(category.Id);
            }
            return categories.ToArray();
        }
    }
}