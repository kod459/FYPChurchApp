namespace PIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixingTings : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Donation", "ChurchId", "dbo.Church");
            DropForeignKey("dbo.Donation", "HouseNumber", "dbo.House");
            DropIndex("dbo.Donation", new[] { "ChurchId" });
            DropIndex("dbo.Donation", new[] { "HouseNumber" });
            DropTable("dbo.Donation");
            DropTable("dbo.House");
        }
        
        public override void Down()
        {
            
        }
    }
}
