namespace PIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class plzwork : DbMigration
    {
        public override void Up()
        {
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
            DropTable("dbo.House");
        }
    }
}
