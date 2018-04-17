namespace PIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Invoice", "DateReceived", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Invoice", "DateReceived", c => c.DateTime(nullable: false));
        }
    }
}
