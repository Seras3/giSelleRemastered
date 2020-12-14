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
        ApplicationDbContext db = new ApplicationDbContext();

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
            catch (Exception e)
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
            catch (Exception e)
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
            return Redirect("/Category/Index");
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
    }
}