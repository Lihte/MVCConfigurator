namespace MVCConfigurator.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CascadePartDelete : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Parts", "Product_Id", "dbo.Products");
            DropIndex("dbo.Parts", new[] { "Product_Id" });
            AlterColumn("dbo.Parts", "Product_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Parts", "Product_Id");
            AddForeignKey("dbo.Parts", "Product_Id", "dbo.Products", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Parts", "Product_Id", "dbo.Products");
            DropIndex("dbo.Parts", new[] { "Product_Id" });
            AlterColumn("dbo.Parts", "Product_Id", c => c.Int());
            CreateIndex("dbo.Parts", "Product_Id");
            AddForeignKey("dbo.Parts", "Product_Id", "dbo.Products", "Id");
        }
    }
}
