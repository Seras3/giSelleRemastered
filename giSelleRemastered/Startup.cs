using giSelleRemastered.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(giSelleRemastered.Startup))]
namespace giSelleRemastered
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            createAdminUserAndApplicationRoles();
        }

        private void createAdminUserAndApplicationRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            
            foreach (var roleName in UserRoleType.ALL)
            {
                if (!roleManager.RoleExists(roleName))
                {
                    roleManager.Create(new IdentityRole { Name = roleName });
                    if (roleName == UserRoleType.ADMIN)
                    {
                        var defaultAdminEmail = "admin@admin.com";
                        var defaultAdminPassword = "Admin1!";

                        var cart = new Cart();
                        context.Carts.Add(cart);
                        var user = new ApplicationUser { UserName = defaultAdminEmail, Email = defaultAdminEmail, CartId = cart.Id };
                        var adminCreated = userManager.Create(user, defaultAdminPassword);
                        if (adminCreated.Succeeded)
                        {
                            userManager.AddToRole(user.Id, UserRoleType.ADMIN);
                        }
                    }
                }
            }
        }
    }
}
