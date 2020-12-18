namespace giSelleRemastered.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<giSelleRemastered.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "giSelleRemastered.ApplicationDbContext";
        }

        protected override void Seed(Models.ApplicationDbContext context)
        {
            if (context.UploadFiles.Find(Models.Configuration.DEFAULT_IMAGE_ID) == null)
            {
                Models.UploadFile defaultProductImage = new Models.UploadFile
                {
                    FileId = Models.Configuration.DEFAULT_IMAGE_ID,
                    Name = "default",
                    Extension = ".jpg"
                };

                context.UploadFiles.Add(defaultProductImage);
                context.SaveChanges();
            }
        }
    }
}
