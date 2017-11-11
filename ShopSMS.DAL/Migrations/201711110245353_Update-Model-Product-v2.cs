namespace ShopSMS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateModelProductv2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Avatar", c => c.String(maxLength: 500));
            AddColumn("dbo.Products", "MoreImages", c => c.String(storeType: "xml"));
            AddColumn("dbo.Products", "PriceSell", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Products", "PriceInput", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Products", "PromotionPrice", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Products", "Warranty", c => c.Int());
            AddColumn("dbo.Products", "Quantity", c => c.Int());
            AddColumn("dbo.Products", "Description", c => c.String(maxLength: 500));
            DropColumn("dbo.Products", "ProductImage");
            DropColumn("dbo.Products", "ProductMoreImage");
            DropColumn("dbo.Products", "ProductPrice");
            DropColumn("dbo.Products", "ProductPromotionPrice");
            DropColumn("dbo.Products", "ProductWarranty");
            DropColumn("dbo.Products", "ProductQuantity");
            DropColumn("dbo.Products", "ProductDescription");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "ProductDescription", c => c.String(maxLength: 500));
            AddColumn("dbo.Products", "ProductQuantity", c => c.Int());
            AddColumn("dbo.Products", "ProductWarranty", c => c.Int());
            AddColumn("dbo.Products", "ProductPromotionPrice", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Products", "ProductPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Products", "ProductMoreImage", c => c.String(storeType: "xml"));
            AddColumn("dbo.Products", "ProductImage", c => c.String(maxLength: 500));
            DropColumn("dbo.Products", "Description");
            DropColumn("dbo.Products", "Quantity");
            DropColumn("dbo.Products", "Warranty");
            DropColumn("dbo.Products", "PromotionPrice");
            DropColumn("dbo.Products", "PriceInput");
            DropColumn("dbo.Products", "PriceSell");
            DropColumn("dbo.Products", "MoreImages");
            DropColumn("dbo.Products", "Avatar");
        }
    }
}
