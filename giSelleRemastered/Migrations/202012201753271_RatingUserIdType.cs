namespace giSelleRemastered.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RatingUserIdType : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Ratings", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Ratings", new[] { "User_Id" });
            DropColumn("dbo.Ratings", "UserId");
            RenameColumn(table: "dbo.Ratings", name: "User_Id", newName: "UserId");
            AlterColumn("dbo.Ratings", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Ratings", "UserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Ratings", "UserId");
            AddForeignKey("dbo.Ratings", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ratings", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Ratings", new[] { "UserId" });
            AlterColumn("dbo.Ratings", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Ratings", "UserId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Ratings", name: "UserId", newName: "User_Id");
            AddColumn("dbo.Ratings", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Ratings", "User_Id");
            AddForeignKey("dbo.Ratings", "User_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
