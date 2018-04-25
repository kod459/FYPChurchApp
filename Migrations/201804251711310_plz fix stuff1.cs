namespace PIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class plzfixstuff1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Invoice", "PictureOfInvoice", c => c.Binary());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Invoice", "PictureOfInvoice", c => c.Binary(nullable: false));
        }
    }
}
