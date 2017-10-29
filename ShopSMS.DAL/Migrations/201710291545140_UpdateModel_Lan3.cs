namespace ShopSMS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateModel_Lan3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Producer",
                c => new
                    {
                        ProducerID = c.Int(nullable: false, identity: true),
                        ProducerName = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.ProducerID);
            
            CreateTable(
                "dbo.Supplier",
                c => new
                    {
                        SupplierID = c.Int(nullable: false, identity: true),
                        SupplierName = c.String(nullable: false, maxLength: 200),
                        Phone = c.String(maxLength: 20, unicode: false),
                        Address = c.String(maxLength: 250),
                        TaxCode = c.String(maxLength: 100, unicode: false),
                        Description = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.SupplierID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Supplier");
            DropTable("dbo.Producer");
        }
    }
}
