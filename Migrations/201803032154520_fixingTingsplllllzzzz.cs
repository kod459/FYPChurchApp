namespace PIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixingTingsplllllzzzz : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Donation", "ChurchId", "dbo.Church");
            DropForeignKey("dbo.Donation", "HouseId", "dbo.House");
            DropIndex("dbo.Donation", new[] { "ChurchId" });
            DropIndex("dbo.Donation", new[] { "HouseId" });
            DropTable("dbo.Donation");
            DropTable("dbo.House");
        }
        
        public override void Down()
        {
        
        }
    }
}
