namespace ShopSMS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ErrorLogs",
                c => new
                    {
                        ErrorLogID = c.Int(nullable: false, identity: true),
                        ErrorLogMessage = c.String(),
                        ErrorLogStackTrace = c.String(),
                        ErrorLogCreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ErrorLogID);
            
            CreateTable(
                "dbo.Footers",
                c => new
                    {
                        FooterID = c.Int(nullable: false, identity: true),
                        FooterContent = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.FooterID);
            
            CreateTable(
                "dbo.Menus",
                c => new
                    {
                        MenuID = c.Int(nullable: false, identity: true),
                        MenuName = c.String(nullable: false, maxLength: 100),
                        MenuURL = c.String(nullable: false, maxLength: 255),
                        ImageURL = c.String(),
                        MenuOrderBy = c.Int(),
                        MenuGroupID = c.Int(nullable: false),
                        MenuTarget = c.String(),
                        MenuStatus = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MenuID)
                .ForeignKey("dbo.MenuGroups", t => t.MenuGroupID, cascadeDelete: true)
                .Index(t => t.MenuGroupID);
            
            CreateTable(
                "dbo.MenuGroups",
                c => new
                    {
                        MenuGroupID = c.Int(nullable: false, identity: true),
                        MenuGroupName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.MenuGroupID);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        OrderCode = c.Int(nullable: false),
                        OrderCustomerName = c.String(nullable: false, maxLength: 100),
                        OrderCustomerAddress = c.String(nullable: false, maxLength: 255),
                        OrderCustomerPhone = c.String(nullable: false, maxLength: 20),
                        OrderCustomerEmail = c.String(maxLength: 100),
                        OrderCustomerMessage = c.String(maxLength: 255),
                        OrderPayment = c.String(maxLength: 255),
                        OrderCreateDate = c.DateTime(),
                        OrderCreateBy = c.String(maxLength: 100),
                        OrderPaymentStatus = c.String(maxLength: 100),
                        OrderStatus = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OrderID)
                .Index(t => t.OrderCode, unique: true);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        OrderID = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                        OrderDetailQuantity = c.Int(nullable: false),
                        OrderDetailPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.OrderID, t.ProductID })
                .ForeignKey("dbo.Orders", t => t.OrderID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.OrderID, cascadeDelete: true)
                .Index(t => t.OrderID);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductID = c.Int(nullable: false, identity: true),
                        ProductCode = c.String(nullable: false, maxLength: 255),
                        ProductName = c.String(nullable: false, maxLength: 255),
                        ProductAlias = c.String(nullable: false, maxLength: 255, unicode: false),
                        ProductCategoryID = c.Int(nullable: false),
                        ProductImage = c.String(maxLength: 500),
                        ProductMoreImage = c.String(storeType: "xml"),
                        ProductPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProductPromotionPrice = c.Decimal(precision: 18, scale: 2),
                        ProductWarranty = c.Int(),
                        ProductQuantity = c.Int(),
                        ProductDescription = c.String(maxLength: 255),
                        ProductContent = c.String(maxLength: 500),
                        ProductHomeFlag = c.Boolean(),
                        ProductHotFlag = c.Boolean(),
                        ProductViewCount = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateBy = c.String(maxLength: 255),
                        UpdateDate = c.DateTime(),
                        UpdateBy = c.String(maxLength: 255),
                        MetaKeyword = c.String(maxLength: 500),
                        MetaDescription = c.String(maxLength: 500),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ProductID)
                .ForeignKey("dbo.ProductCategories", t => t.ProductCategoryID, cascadeDelete: true)
                .Index(t => t.ProductCode, unique: true)
                .Index(t => t.ProductCategoryID);
            
            CreateTable(
                "dbo.ProductCategories",
                c => new
                    {
                        ProductCategoryID = c.Int(nullable: false, identity: true),
                        ProductCategoryName = c.String(nullable: false, maxLength: 255),
                        ProductCategoryAlias = c.String(nullable: false, maxLength: 255),
                        ProductCategoryDescription = c.String(maxLength: 255),
                        CategoryID = c.Int(nullable: false),
                        ProductCategoryDisplayOrder = c.Int(),
                        ProductCategoryImage = c.String(),
                        ProductCategoryHomeFlag = c.Boolean(),
                        CreateDate = c.DateTime(),
                        CreateBy = c.String(maxLength: 255),
                        UpdateDate = c.DateTime(),
                        UpdateBy = c.String(maxLength: 255),
                        MetaKeyword = c.String(maxLength: 500),
                        MetaDescription = c.String(maxLength: 500),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ProductCategoryID)
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false, maxLength: 255),
                        CategoryAlias = c.String(nullable: false, maxLength: 255),
                        CategoryDescription = c.String(maxLength: 255),
                        CategoryDisplayOrder = c.Int(),
                        CategoryImage = c.String(),
                        CategoryHomeFlag = c.Boolean(),
                        CreateDate = c.DateTime(),
                        CreateBy = c.String(maxLength: 255),
                        UpdateDate = c.DateTime(),
                        UpdateBy = c.String(maxLength: 255),
                        MetaKeyword = c.String(maxLength: 500),
                        MetaDescription = c.String(maxLength: 500),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        PostsID = c.Int(nullable: false, identity: true),
                        PostCode = c.String(nullable: false, maxLength: 255),
                        PostsName = c.String(nullable: false, maxLength: 255),
                        PostsAlias = c.String(nullable: false, maxLength: 255, unicode: false),
                        PostsCategoryID = c.Int(nullable: false),
                        PostsImage = c.String(maxLength: 255),
                        PostsDescription = c.String(maxLength: 500),
                        PostsContent = c.String(),
                        PostsHomeFlag = c.Boolean(),
                        PostsHotFlag = c.Boolean(),
                        PostsViewCount = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateBy = c.String(maxLength: 255),
                        UpdateDate = c.DateTime(),
                        UpdateBy = c.String(maxLength: 255),
                        MetaKeyword = c.String(maxLength: 500),
                        MetaDescription = c.String(maxLength: 500),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PostsID)
                .ForeignKey("dbo.PostCategories", t => t.PostsCategoryID, cascadeDelete: true)
                .Index(t => t.PostCode, unique: true)
                .Index(t => t.PostsCategoryID);
            
            CreateTable(
                "dbo.PostCategories",
                c => new
                    {
                        PostCategoryID = c.Int(nullable: false, identity: true),
                        PostCategoryName = c.String(nullable: false, maxLength: 255),
                        PostCategoryAlias = c.String(nullable: false, maxLength: 255, unicode: false),
                        PostCategoryDescription = c.String(maxLength: 255),
                        PostCategoryParentID = c.Int(),
                        PostCategoryDisplayOrder = c.Int(),
                        PostCategoryImage = c.String(),
                        PostCategoryHomeFlag = c.Boolean(),
                        CreateDate = c.DateTime(),
                        CreateBy = c.String(maxLength: 255),
                        UpdateDate = c.DateTime(),
                        UpdateBy = c.String(maxLength: 255),
                        MetaKeyword = c.String(maxLength: 500),
                        MetaDescription = c.String(maxLength: 500),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PostCategoryID);
            
            CreateTable(
                "dbo.ProductTags",
                c => new
                    {
                        ProductID = c.Int(nullable: false),
                        TagID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductID, t.TagID })
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.TagID, cascadeDelete: true)
                .Index(t => t.ProductID)
                .Index(t => t.TagID);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        TagID = c.Int(nullable: false, identity: true),
                        TagName = c.String(nullable: false, maxLength: 100),
                        TagType = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.TagID);
            
            CreateTable(
                "dbo.IdentityRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        IdentityRole_Id = c.String(maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.IdentityRoles", t => t.IdentityRole_Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.IdentityRole_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Slides",
                c => new
                    {
                        SlideID = c.Int(nullable: false, identity: true),
                        SlideName = c.String(nullable: false, maxLength: 100),
                        SlideDescription = c.String(maxLength: 255),
                        SlideImage = c.String(maxLength: 500),
                        SlideUrl = c.String(maxLength: 256),
                        SlideDisplayOrder = c.Int(),
                        SlideStatus = c.Boolean(nullable: false),
                        SlideContent = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.SlideID);
            
            CreateTable(
                "dbo.SystemConfigs",
                c => new
                    {
                        SystemConfigID = c.Int(nullable: false, identity: true),
                        SystemConfigContent = c.String(nullable: false, maxLength: 255),
                        SystemConfigStatus = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SystemConfigID);
            
            CreateTable(
                "dbo.ApplicationUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserCode = c.String(maxLength: 50, unicode: false),
                        FullName = c.String(maxLength: 200),
                        FirstName = c.String(maxLength: 50),
                        Address = c.String(maxLength: 255),
                        BirthDay = c.DateTime(),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.IdentityUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IdentityUserRoles", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.IdentityUserLogins", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.IdentityUserClaims", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.IdentityUserRoles", "IdentityRole_Id", "dbo.IdentityRoles");
            DropForeignKey("dbo.ProductTags", "TagID", "dbo.Tags");
            DropForeignKey("dbo.ProductTags", "ProductID", "dbo.Products");
            DropForeignKey("dbo.Posts", "PostsCategoryID", "dbo.PostCategories");
            DropForeignKey("dbo.OrderDetails", "OrderID", "dbo.Products");
            DropForeignKey("dbo.Products", "ProductCategoryID", "dbo.ProductCategories");
            DropForeignKey("dbo.ProductCategories", "CategoryID", "dbo.Categories");
            DropForeignKey("dbo.OrderDetails", "OrderID", "dbo.Orders");
            DropForeignKey("dbo.Menus", "MenuGroupID", "dbo.MenuGroups");
            DropIndex("dbo.IdentityUserLogins", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserClaims", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserRoles", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserRoles", new[] { "IdentityRole_Id" });
            DropIndex("dbo.ProductTags", new[] { "TagID" });
            DropIndex("dbo.ProductTags", new[] { "ProductID" });
            DropIndex("dbo.Posts", new[] { "PostsCategoryID" });
            DropIndex("dbo.Posts", new[] { "PostCode" });
            DropIndex("dbo.ProductCategories", new[] { "CategoryID" });
            DropIndex("dbo.Products", new[] { "ProductCategoryID" });
            DropIndex("dbo.Products", new[] { "ProductCode" });
            DropIndex("dbo.OrderDetails", new[] { "OrderID" });
            DropIndex("dbo.Orders", new[] { "OrderCode" });
            DropIndex("dbo.Menus", new[] { "MenuGroupID" });
            DropTable("dbo.IdentityUserLogins");
            DropTable("dbo.IdentityUserClaims");
            DropTable("dbo.ApplicationUsers");
            DropTable("dbo.SystemConfigs");
            DropTable("dbo.Slides");
            DropTable("dbo.IdentityUserRoles");
            DropTable("dbo.IdentityRoles");
            DropTable("dbo.Tags");
            DropTable("dbo.ProductTags");
            DropTable("dbo.PostCategories");
            DropTable("dbo.Posts");
            DropTable("dbo.Categories");
            DropTable("dbo.ProductCategories");
            DropTable("dbo.Products");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.Orders");
            DropTable("dbo.MenuGroups");
            DropTable("dbo.Menus");
            DropTable("dbo.Footers");
            DropTable("dbo.ErrorLogs");
        }
    }
}
