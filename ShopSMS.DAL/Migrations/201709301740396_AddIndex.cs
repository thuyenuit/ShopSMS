namespace ShopSMS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIndex : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Products", "ProductID", unique: true, name: "Index_Product");
            CreateIndex("dbo.ProductCategories", "ProductCategoryID", unique: true, name: "Index_ProductCategory");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ProductCategories", "Index_ProductCategory");
            DropIndex("dbo.Products", "Index_Product");
        }
    }
}
