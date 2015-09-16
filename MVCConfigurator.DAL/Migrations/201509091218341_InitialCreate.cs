namespace MVCConfigurator.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DeliveryDate = c.DateTime(nullable: false),
                        IsReady = c.Boolean(nullable: false),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.ProductId)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ProductCode = c.String(),
                        ImagePath = c.String(),
                        Category_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductCategories", t => t.Category_Id)
                .Index(t => t.Category_Id);
            
            CreateTable(
                "dbo.ProductCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Parts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LeadTime = c.Int(nullable: false),
                        StockKeepingUnit = c.String(),
                        ImagePath = c.String(),
                        Category_Id = c.Int(),
                        Product_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PartCategories", t => t.Category_Id)
                .ForeignKey("dbo.Products", t => t.Product_Id)
                .Index(t => t.Category_Id)
                .Index(t => t.Product_Id);
            
            CreateTable(
                "dbo.PartCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserDetails_FirstName = c.String(),
                        UserDetails_LastName = c.String(),
                        UserDetails_Phone = c.String(),
                        UserDetails_Address = c.String(),
                        UserDetails_Company = c.String(),
                        Salt = c.Binary(),
                        Hash = c.Binary(),
                        IsAdmin = c.Boolean(nullable: false),
                        UserName = c.String(),
                        RequestPasswordToken = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PartParts",
                c => new
                    {
                        Part_Id = c.Int(nullable: false),
                        Part_Id1 = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Part_Id, t.Part_Id1 })
                .ForeignKey("dbo.Parts", t => t.Part_Id)
                .ForeignKey("dbo.Parts", t => t.Part_Id1)
                .Index(t => t.Part_Id)
                .Index(t => t.Part_Id1);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Orders", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Parts", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.PartParts", "Part_Id1", "dbo.Parts");
            DropForeignKey("dbo.PartParts", "Part_Id", "dbo.Parts");
            DropForeignKey("dbo.Parts", "Category_Id", "dbo.PartCategories");
            DropForeignKey("dbo.Products", "Category_Id", "dbo.ProductCategories");
            DropIndex("dbo.PartParts", new[] { "Part_Id1" });
            DropIndex("dbo.PartParts", new[] { "Part_Id" });
            DropIndex("dbo.Parts", new[] { "Product_Id" });
            DropIndex("dbo.Parts", new[] { "Category_Id" });
            DropIndex("dbo.Products", new[] { "Category_Id" });
            DropIndex("dbo.Orders", new[] { "User_Id" });
            DropIndex("dbo.Orders", new[] { "ProductId" });
            DropTable("dbo.PartParts");
            DropTable("dbo.Users");
            DropTable("dbo.PartCategories");
            DropTable("dbo.Parts");
            DropTable("dbo.ProductCategories");
            DropTable("dbo.Products");
            DropTable("dbo.Orders");
        }
    }
}
