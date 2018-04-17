namespace PIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class invoicestuff : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Invoice", "ImageFileName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Invoice", "ImageFileName");
        }
    }
}
