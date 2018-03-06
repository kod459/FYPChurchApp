namespace PIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixingTings1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Donation",
                c => new
                    {
                        DonationId = c.Int(nullable: false, identity: true),
                        TypeOfDonation = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ChurchId = c.Int(nullable: false),
                        HouseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DonationId)
                .ForeignKey("dbo.Church", t => t.ChurchId, cascadeDelete: false)
                .ForeignKey("dbo.House", t => t.HouseId, cascadeDelete: false)
                .Index(t => t.ChurchId)
                .Index(t => t.HouseId);
            
            CreateTable(
                "dbo.House",
                c => new
                    {
                        HouseId = c.Int(nullable: false, identity: true),
                        AddressLine1 = c.String(),
                        AddressLine2 = c.String(),
                        AddressLine3 = c.String(),
                    })
                .PrimaryKey(t => t.HouseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Donation", "HouseId", "dbo.House");
            DropForeignKey("dbo.Donation", "ChurchId", "dbo.Church");
            DropIndex("dbo.Donation", new[] { "HouseId" });
            DropIndex("dbo.Donation", new[] { "ChurchId" });
            DropTable("dbo.House");
            DropTable("dbo.Donation");
        }
    }
}
