namespace ShopSMS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "TaxVAT", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Products", "ProductNew", c => c.Boolean());
            AddColumn("dbo.Products", "ProductSellingGood", c => c.Boolean());
            AddColumn("dbo.Products", "ProducerID", c => c.Int());
            AddColumn("dbo.ProductCategories", "DisplayOrder", c => c.Int());
            AddColumn("dbo.ProductCategories", "HomeFlag", c => c.Boolean());
            AddColumn("dbo.Categories", "DisplayOrder", c => c.Int());
            AlterColumn("dbo.Products", "ProductDescription", c => c.String(maxLength: 500));
            AlterColumn("dbo.Products", "UpdateBy", c => c.String(maxLength: 1000));
            AlterColumn("dbo.ProductCategories", "UpdateBy", c => c.String(maxLength: 1000));
            AlterColumn("dbo.Categories", "UpdateBy", c => c.String(maxLength: 1000));
            AlterColumn("dbo.Posts", "UpdateBy", c => c.String(maxLength: 1000));
            AlterColumn("dbo.PostCategories", "UpdateBy", c => c.String(maxLength: 1000));
            DropColumn("dbo.Products", "ProductContent");
            DropColumn("dbo.ProductCategories", "ProductCategoryAlias");
            DropColumn("dbo.ProductCategories", "ProductCategoryDescription");
            DropColumn("dbo.ProductCategories", "ProductCategoryDisplayOrder");
            DropColumn("dbo.ProductCategories", "ProductCategoryImage");
            DropColumn("dbo.ProductCategories", "ProductCategoryHomeFlag");
            DropColumn("dbo.Categories", "CategoryAlias");
            DropColumn("dbo.Categories", "CategoryDescription");
            DropColumn("dbo.Categories", "CategoryDisplayOrder");
            DropColumn("dbo.Categories", "CategoryImage");
            DropColumn("dbo.Categories", "CategoryHomeFlag");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Categories", "CategoryHomeFlag", c => c.Boolean());
            AddColumn("dbo.Categories", "CategoryImage", c => c.String());
            AddColumn("dbo.Categories", "CategoryDisplayOrder", c => c.Int());
            AddColumn("dbo.Categories", "CategoryDescription", c => c.String(maxLength: 255));
            AddColumn("dbo.Categories", "CategoryAlias", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.ProductCategories", "ProductCategoryHomeFlag", c => c.Boolean());
            AddColumn("dbo.ProductCategories", "ProductCategoryImage", c => c.String());
            AddColumn("dbo.ProductCategories", "ProductCategoryDisplayOrder", c => c.Int());
            AddColumn("dbo.ProductCategories", "ProductCategoryDescription", c => c.String(maxLength: 255));
            AddColumn("dbo.ProductCategories", "ProductCategoryAlias", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Products", "ProductContent", c => c.String(maxLength: 500));
            AlterColumn("dbo.PostCategories", "UpdateBy", c => c.String(maxLength: 255));
            AlterColumn("dbo.Posts", "UpdateBy", c => c.String(maxLength: 255));
            AlterColumn("dbo.Categories", "UpdateBy", c => c.String(maxLength: 255));
            AlterColumn("dbo.ProductCategories", "UpdateBy", c => c.String(maxLength: 255));
            AlterColumn("dbo.Products", "UpdateBy", c => c.String(maxLength: 255));
            AlterColumn("dbo.Products", "ProductDescription", c => c.String(maxLength: 255));
            DropColumn("dbo.Categories", "DisplayOrder");
            DropColumn("dbo.ProductCategories", "HomeFlag");
            DropColumn("dbo.ProductCategories", "DisplayOrder");
            DropColumn("dbo.Products", "ProducerID");
            DropColumn("dbo.Products", "ProductSellingGood");
            DropColumn("dbo.Products", "ProductNew");
            DropColumn("dbo.Products", "TaxVAT");
        }
    }
}
