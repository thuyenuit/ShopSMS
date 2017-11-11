namespace ShopSMS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateModelProductv1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "ProductCategoryID", "dbo.ProductCategories");
            DropIndex("dbo.Products", new[] { "ProductCategoryID" });
            AlterColumn("dbo.Products", "ProductAlias", c => c.String(maxLength: 255, unicode: false));
            AlterColumn("dbo.Products", "ProductCategoryID", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "ProductCategoryID", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "ProductAlias", c => c.String(nullable: false, maxLength: 255, unicode: false));
            CreateIndex("dbo.Products", "ProductCategoryID");
            AddForeignKey("dbo.Products", "ProductCategoryID", "dbo.ProductCategories", "ProductCategoryID", cascadeDelete: true);
        }
    }
}
