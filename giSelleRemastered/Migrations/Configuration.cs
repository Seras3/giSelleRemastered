namespace giSelleRemastered.Migrations
{
    using giSelleRemastered.Models;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<giSelleRemastered.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "giSelleRemastered.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            if (context.UploadFiles.Find(0) == null)
            {
                UploadFile defaultProductImage = new UploadFile
                {
                    FileId = 0,
                    Name = "default",
                    Extension = ".jpg"
                };

                context.UploadFiles.Add(defaultProductImage);
                context.SaveChanges();
            }
        }
    }
}
