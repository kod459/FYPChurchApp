namespace PIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingDatetoDonationandInvoice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Donation", "DateRecieved", c => c.DateTime(nullable: false));
            AddColumn("dbo.Invoice", "DateReceived", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Invoice", "DateReceived");
            DropColumn("dbo.Donation", "DateRecieved");
        }
    }
}
