namespace PIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class willthiswork : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Donation", "AddressLine1", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Donation", "AddressLine1");
        }
    }
}
