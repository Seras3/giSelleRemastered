namespace giSelleRemastered.Migrations
{
    using giSelleRemastered.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using System.Web;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "giSelleRemastered.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            if (context.UploadFiles.Find(0) == null)
            {
                UploadFile defaultProductImage = new UploadFile
                {
                    FileId = 0,
                    Path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Images/Products"), "default.jpg"),
                    Name = "default",
                    Extension = ".jpg"
                };

                context.UploadFiles.AddOrUpdate(defaultProductImage);
                context.SaveChanges();
            } 
        }
    }
}
