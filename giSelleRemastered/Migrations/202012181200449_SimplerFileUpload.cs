namespace giSelleRemastered.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SimplerFileUpload : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.UploadFiles", "Path");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UploadFiles", "Path", c => c.String());
        }
    }
}
